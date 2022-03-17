using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class GridManager : MonoBehaviour
{
    public delegate void TurnOffKinematics();
    public TurnOffKinematics onTurnOffKinematics;

    public bool CanMatch = false;
    public bool GridReady = false;
    public Gem[,] gems = new Gem[8, 8];
    public List<Gem> matchedGems = new List<Gem>();
    
    [SerializeField] private List<Gem> tempGems = new List<Gem>();

    private static GridManager instance;
    [SerializeField] private Gem selectedGem1 = null;
    [SerializeField] private Gem selectedGem2 = null;

    public static GridManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMatches();

        //bool still = true;
        //foreach(Gem g in gems)
        //{
        //    if (!g.IsStill)
        //        still = false;
        //}
        //CanMatch = still;
    }

    public void OnGemSelected(Gem gem)
    {
        if (!CanMatch)
            return;

        if (selectedGem1 == null)
        {
            selectedGem1 = gem;
        }
        else if (selectedGem2 == null)
        {
            if (selectedGem1 == gem)
                selectedGem1 = null;
            else
                selectedGem2 = gem;
        }
        else
        {
            // TODO: Both gems selected, set them back to null, and select the first one
            selectedGem1 = null;
            selectedGem2 = null;

            selectedGem1 = gem;
        }
        //if (selectedGem1 != null)
        //    selectedGem1.GetComponent<SpriteRenderer>().color = Color.red;
        //if (selectedGem2 != null)
        //    selectedGem2.GetComponent<SpriteRenderer>().color = Color.red;
    }


    private bool CheckForMatchOf3(int row, int col)
    {
        if (tempGems.Count != 3)
            return false;

        if (tempGems[0] != null && tempGems[1] != null && tempGems[2] != null)
        {
            //Debug.Log("Got here A2");
            if (tempGems[0].gemType == tempGems[1].gemType && tempGems[1].gemType == tempGems[2].gemType)
            {
                //Debug.Log("Got here A3");
                foreach(Gem g in tempGems)
                {
                    bool noDuplicates = true;

                    foreach(Gem h in matchedGems)
                    {
                        if (h.row == g.row && h.col == g.col)
                        {
                            noDuplicates = false;
                        }
                    }
                    if (noDuplicates)
                    {
                        matchedGems.Add(g);
                    }
                }

                return true;
            }
        }

        return false;
    }
    private bool CheckForMatchOf4(int row, int col)
    {
        if (tempGems.Count != 4)
            return false;

        if (tempGems[0] != null && tempGems[1] != null && tempGems[2] != null && tempGems[3] != null)
        {
            if (tempGems[0].gemType == tempGems[1].gemType && tempGems[1].gemType == tempGems[2].gemType && tempGems[2].gemType == tempGems[3].gemType)
            {
                //Debug.Log("We got a match of (4) at row: " + row.ToString() + " col: " + col.ToString());
                foreach (Gem g in tempGems)
                {
                    bool noDuplicates = true;

                    foreach (Gem h in matchedGems)
                    {
                        if (h.row == g.row && h.col == g.col)
                        {
                            noDuplicates = false;
                        }
                    }
                    if (noDuplicates)
                    {
                        matchedGems.Add(g);
                    }
                }
                return true;
            }
        }
        return false;
    }
    private bool CheckForMatchOf5(int row, int col)
    {
        if (tempGems.Count != 5)
        {
            return false;
        }

        if (tempGems[0] != null && tempGems[1] != null && tempGems[2] != null && tempGems[3] != null && tempGems[4] != null)
        {
            if (tempGems[0].gemType == tempGems[1].gemType && tempGems[1].gemType == tempGems[2].gemType && tempGems[2].gemType == tempGems[3].gemType && tempGems[3].gemType == tempGems[4].gemType)
            {
                //Debug.Log("We got a match of (5) at row: " + row.ToString() + " col: " + col.ToString());

                foreach (Gem g in tempGems)
                {
                    bool noDuplicates = true;

                    foreach (Gem h in matchedGems)
                    {
                        if (h.row == g.row && h.col == g.col)
                        {
                            noDuplicates = false;
                        }
                    }
                    if (noDuplicates)
                    {
                        matchedGems.Add(g);
                    }
                }

                return true;
            }
        }
        return false;
    }
    private bool CheckForMatchOf6(int row, int col)
    {
        if (tempGems.Count != 6)
        {
            return false;
        }

        if (tempGems[0] != null && tempGems[1] != null && tempGems[2] != null && tempGems[3] != null && tempGems[4] != null && tempGems[5] != null)
        {
            if (tempGems[0].gemType == tempGems[1].gemType && tempGems[1].gemType == tempGems[2].gemType && tempGems[2].gemType == tempGems[3].gemType && tempGems[3].gemType == tempGems[4].gemType && tempGems[4].gemType == tempGems[5].gemType)
            {
                //Debug.Log("We got a match of (6) at row: " + row.ToString() + " col: " + col.ToString());

                foreach (Gem g in tempGems)
                {
                    bool noDuplicates = true;

                    foreach (Gem h in matchedGems)
                    {
                        if (h.row == g.row && h.col == g.col)
                        {
                            noDuplicates = false;
                        }
                    }
                    if (noDuplicates)
                    {
                        matchedGems.Add(g);
                    }
                }

                return true;
            }
        }
        return false;
    }

    private bool AddNeighborColums(int row, int col, int numColums)
    {
        tempGems.Clear();
        bool safe = true;
        for (int i = 0; i < numColums; i++)
        {
            if (col + i > 7)
                safe = false;
        }
        if (!safe)
            return false;

        for (int i = 0; i < numColums; i++)
        {
            tempGems.Add(gems[row, col + i]);
        }
        
        return true;
    }
    private bool AddNeighborRows(int row, int col, int numRows)
    {
        tempGems.Clear();
        bool safe = true;
        for (int i = 0; i < numRows; i++)
        {
            if (row + i > 7)
                safe = false;
        }
        if (!safe)
            return false;

        for (int i = 0; i < numRows; i++)
        {
            tempGems.Add(gems[row + i, col]);
        }

        return true;
    }
    public void CheckForMatches()
    {
        if (!GridReady)
            return;

        
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                // COLS
                if (AddNeighborColums(row, col, 6))
                {
                    if (CheckForMatchOf6(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..

                        continue;
                    }
                }
                if (AddNeighborColums(row, col, 5))
                {
                    if (CheckForMatchOf5(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..

                        continue;
                    }
                }

                if (AddNeighborColums(row, col, 4))
                {
                    if (CheckForMatchOf4(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..

                        continue;
                    }
                }
                if (AddNeighborColums(row, col, 3))
                {
                    if (CheckForMatchOf3(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..

                        continue;
                    }
                }
                // ROWS
                if (AddNeighborRows(row, col, 6))
                {
                    if (CheckForMatchOf6(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        continue;
                    }
                }

                if (AddNeighborRows(row, col, 5))
                {
                    if (CheckForMatchOf5(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        continue;
                    }
                }

                if (AddNeighborRows(row, col, 4))
                {
                    if (CheckForMatchOf4(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        continue;
                    }
                }
                if (AddNeighborRows(row, col, 3))
                {
                    if (CheckForMatchOf3(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        continue;
                    }
                }


            }
        }
        DestroyMatchedGems();
        GridReady = false;
    }
    public void DestroyMatchedGems()
    {
        StartCoroutine(EnumerateGems());
    }
    public IEnumerator EnumerateGems()
    {
        SetGemsKinematic();
        foreach (Gem g in matchedGems)
            Destroy(g.gameObject);


        // This has some issues
        //yield return new WaitForSeconds(0.5f);

        GemSpawner.Instance.AddGemsToSpawnQueue(matchedGems);
        
        foreach (Gem g in matchedGems)
        {
            GemSpawner.Instance.SpawnGemAtColumn(g, g.col);
            //yield return new WaitForSeconds(0.2f);
        }
        GemSpawner.Instance.ClearSpawnQueue();
        //Debug.Log("Respawned gems");
        while (true)
        {
            bool boardSettled = true;
            foreach (Gem g in gems)
            {
                if (!g.IsStill)
                    boardSettled = false;
            }

            if (boardSettled)
            {
                yield return new WaitForSeconds(0.8f);
                break;
            }

            yield return null;
        }
        matchedGems.Clear();
        GridReady = true;
    }

    public void SetGemsKinematic()
    {
        onTurnOffKinematics?.Invoke();
    }

    public string getDimension(Gem g)
    {
        return "(X:" + g.col + ", Y: " + g.row + ", Type: " + g.gemType + ")"; 
    }
}
