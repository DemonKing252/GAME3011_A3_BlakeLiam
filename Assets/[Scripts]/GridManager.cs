using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class GridManager : MonoBehaviour
{
    public bool GridReady = false;
    public Gem[,] gems = new Gem[8, 8];
    public List<Gem> matchedGems = new List<Gem>();
    
    [SerializeField] private List<Gem> tempGems = new List<Gem>();

    private static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMatches();
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
                Debug.Log("We got a match of (4) at row: " + row.ToString() + " col: " + col.ToString());
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
                Debug.Log("We got a match of (5) at row: " + row.ToString() + " col: " + col.ToString());

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
                Debug.Log("We got a match of (6) at row: " + row.ToString() + " col: " + col.ToString());

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
        string h = "[BROKEN MATCH]: " + getDimension(gems[7, 2]) + " " + getDimension(gems[7, 3]) + " " + getDimension(gems[7, 4]) + " " + getDimension(gems[7, 5]);

        //
        //
        //
        //

        Debug.Log(h);

        DestroyMatchedGems();
        GridReady = false;

    }
    public void DestroyMatchedGems()
    {
        StartCoroutine(EnumerateGems());
    }
    public IEnumerator EnumerateGems()
    {
        foreach (Gem g in matchedGems)
            Destroy(g.gameObject);
        
        // This has some issues
        yield return new WaitForSeconds(0.5f);
        foreach (Gem g in matchedGems)
        {
            GemSpawner.Instance.SpawnGemAtColumn(g.col);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        GridReady = true;


        matchedGems.Clear();
    }
    public string getDimension(Gem g)
    {
        return "(X:" + g.col + ", Y: " + g.row + ", Type: " + g.gemType + ")"; 
    }
}
