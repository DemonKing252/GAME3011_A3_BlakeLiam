using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GemType
{
    Blue,
    Cyan,
    Green,
    Lilac,
    Red,
    Brown,
    Yellow,
    NumTypes
}

public class GemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gemPrefabs;

    [SerializeField]
    private Transform[] gemSpawners;

    // Spawn under the mask of the grid
    [SerializeField]
    private Transform gridMaskTransform;

    private static GemSpawner instance;
    public static GemSpawner Instance { get { return instance; } }
    private IEnumerator SpawnGrid()
    {
        GridManager.Instance.GridReady = false;
        for (int rows = 0; rows < 8; rows++)
        {
            // Spawn this row
            for (int cols = 0; cols < 8; cols++)
            {
                int randIdx = Random.Range(0, gemPrefabs.Length);
                GameObject go = Instantiate(gemPrefabs[randIdx], gemSpawners[cols].position, Quaternion.identity, gridMaskTransform);
                Gem gemComp = go.GetComponent<Gem>();

                // Since the first row spawning will be the last row, we have to do 8 - rows
                gemComp.row = 7 - rows;
                gemComp.col = cols;
                gemComp.gemType = (GemType)randIdx;

                GridManager.Instance.gems[7 - rows, cols] = gemComp;
            }
            // Wait a 1/2 second then spawn the next row
            yield return new WaitForSeconds(0.25f);
        }
        GridManager.Instance.GridReady = true;
    }
    public void SpawnEntireGrid()
    {
        StartCoroutine(SpawnGrid());
    }
    public void SpawnGemAtColumn(int columnIdx)
    {
        int randIdx = Random.Range(0, gemPrefabs.Length);
        GameObject go = Instantiate(gemPrefabs[randIdx], gemSpawners[columnIdx].position, Quaternion.identity, gridMaskTransform);
        Gem gemComp = go.GetComponent<Gem>();

        gemComp.gemType = (GemType)randIdx;
        gemComp.col = columnIdx;
        
        // As the gem falls through the grid, it will intersect the row triggers and it will change row index accordingly.
        gemComp.row = 0;

        GridManager.Instance.gems[gemComp.row, gemComp.col] = gemComp;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SpawnEntireGrid();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
