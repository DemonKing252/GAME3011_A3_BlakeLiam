using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GemType
{
    Blue,
    Brown,
    Cyan,
    Green,
    Lilac,
    Red,
    Yellow,
    NumTypes
}
[System.Serializable]
public class SpawnQueue
{
    public List<Vector3> gemQueue;
    public SpawnQueue()
    {
        gemQueue = new List<Vector3>();
    }
}
public class GemSpawner : MonoBehaviour
{
    [SerializeField]
    private int seed;

    [SerializeField]
    private int gemCount;

    [SerializeField]
    private GameObject[] gemPrefabs;

    [SerializeField]
    private Transform[] gemSpawners;

    // Spawn under the mask of the grid
    [SerializeField]
    private Transform gridMaskTransform;

    public SpawnQueue[] spawnPoints = new SpawnQueue[8];
    private static GemSpawner instance;
    public static GemSpawner Instance { get { return instance; } }
    public int i = 0;
    private IEnumerator SpawnGrid()
    {
        /*
        Valid Seeds:
            2,19,24
         
        */
        // Seed values that I know have no matches from the initial grid
        // In the interest of time, I decided to prevent no cascades from the beginning using this method
        // because the user will never notice this.
        int[] seeds = { 2, 19, 24 };

        Random.seed = seeds[Random.Range(0, seeds.Length)];

        GridManager.Instance.CanMatch = false;
        GridManager.Instance.GridReady = false;
        for (int rows = 0; rows < 8; rows++)
        {
            // Spawn this row
            for (int cols = 0; cols < 8; cols++)
            {
                int randIdx = Random.Range(0, gemCount);

                {
                    GameObject go = Instantiate(gemPrefabs[randIdx], gemSpawners[cols].position, Quaternion.identity, gridMaskTransform);
                    i++;
                    go.name = "Gem " + i.ToString();
                    
                    Gem gemComp = go.GetComponent<Gem>();

                    // Since the first row spawning will be the last row, we have to do 8 - rows
                    gemComp.row = 7 - rows;
                    gemComp.col = cols;
                    gemComp.gemType = (GemType)randIdx;

                    GridManager.Instance.gems[7 - rows, cols] = gemComp;
                    GridManager.Instance.gemList.Add(gemComp);

                }
            }
            // Wait a 1/2 second then spawn the next row
            yield return new WaitForSeconds(0.4f);
        }

        while (true)
        {
            bool boardSettled = true;
            foreach (Gem g in GridManager.Instance.gems)
            {
                if (!g.IsStill)
                    boardSettled = false;
            }

            if (boardSettled)
            {
                yield return new WaitForSeconds(0.3f);
                break;
            }

            yield return null;
        }
        GridManager.Instance.runTimer = true;
        GridManager.Instance.GridReady = true;
        GridManager.Instance.CanMatch = true;
        GridManager.Instance.SetDesc();
        GridManager.Instance.themeSfx.loop = true;
        GridManager.Instance.themeSfx.Play();
    }
    public void SpawnEntireGrid()
    {
        StartCoroutine(SpawnGrid());
    }
    public void SpawnGemAtColumn(Gem g, int columnIdx)
    {
        int randIdx = Random.Range(0, gemCount);
        GameObject go = Instantiate(gemPrefabs[randIdx], g.spawnLoc, Quaternion.identity, gridMaskTransform);
        i++;
        go.name = "Gem " + i.ToString(); 
        Gem gemComp = go.GetComponent<Gem>();

        gemComp.gemType = (GemType)randIdx;
        gemComp.col = columnIdx;

        int weight = GridManager.Instance.Difficulty switch
        {
            Difficulty.Begginer     => 0,       // 0% chance of ticking bomb on Beginner
            Difficulty.Intermediate => 5,       // 5% chance of ticking bomb on Intermediate
            Difficulty.Advanced     => 10,      // 10% chance of ticking bomb on Advanced
            Difficulty.Expert       => 20       // 20% chance of ticking bomb on Expert
        
        };


        if (Random.Range(1, 100) < weight)
        {
            gemComp.gemBehaviour = GemBehaviour.TickingBomb;
            gemComp.spriteRen.SetActive(true);
        }
        else
        {
            gemComp.gemBehaviour = GemBehaviour.Normal;
        }

        // As the gem falls through the grid, it will intersect the row triggers and it will change row index accordingly.
        gemComp.row = 0;

        GridManager.Instance.gemList.Add(gemComp);
        GridManager.Instance.gems[gemComp.row, gemComp.col] = gemComp;
    }

    private void Awake()
    {

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnEntireGrid();
    }
    public void AddGemsToSpawnQueue(List<Gem> gems)
    {
        foreach (Gem g in gems)
        {
            //Debug.LogWarning(g.col + " - " + spawnPoints.Length);
            float offsetY = (float)spawnPoints[g.col].gemQueue.Count * 0.1315f;

            Vector3 spawnP = gemSpawners[g.col].position;
            spawnP.y += offsetY;
            g.spawnLoc = spawnP;

            spawnPoints[g.col].gemQueue.Add(g.spawnLoc);
        }
    }

    public void ClearSpawnQueue()
    {
        foreach (SpawnQueue q in spawnPoints)
            q.gemQueue.Clear();
    }

}
