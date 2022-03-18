using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class GridManager : MonoBehaviour
{
    public delegate void TurnOffKinematics();
    public TurnOffKinematics onTurnOffKinematics;

    public bool SwappingGems = false;
    public bool CanMatch = false;
    public bool GridReady = false;
    public Gem[,] gems = new Gem[8, 8];
    public List<Gem> matchedGems = new List<Gem>();
    
    [SerializeField] private List<Gem> tempGems = new List<Gem>();

    private static GridManager instance;
    [SerializeField] private List<Gem> selectedGems = new List<Gem>();

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
        if (!GridReady || SwappingGems)
            return;
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

        if (selectedGems.Count == 0)
        {
            selectedGems.Add(gem);
        }
        else if (selectedGems.Count == 1)
        {
            // Did we select the same gem as the first one?
            if (gem == selectedGems[0])
            {
                selectedGems.Clear();
            }
            else
            {
                selectedGems.Add(gem);
                StartCoroutine(SwapGems());
            }

        }
    }
    private IEnumerator SwapGems()
    {
        SwappingGems = true;
        Gem gem1 = selectedGems[0];
        Gem gem2 = selectedGems[1];

        Vector3 gem1Loc = selectedGems[0].transform.position;
        Vector3 gem2Loc = selectedGems[1].transform.position;

        SetGridPhysicsOn(false);
        float t = 0f;

        if (selectedGems.Count != 2)
            Debug.LogError("Selected gems count dont match 2!");

        //Debug.LogWarning("Here");
        bool sameRow = gem1.row == gem2.row;
        bool sameCol = gem1.col == gem2.col;


        while (t < 0.75f)
        {
            gem1.transform.position = Vector3.Lerp(gem1Loc, gem2Loc, t / 0.75f);         
            gem2.transform.position = Vector3.Lerp(gem2Loc, gem1Loc, t / 0.75f);

            t += Time.deltaTime;
            yield return null;
        }


        SetGridPhysicsOn(true);

        // Horizontal swap
        if (sameRow)
        {
            Debug.Log("comparing rows");


            int col1 = gem1.col;
            int col2 = gem2.col;

            GemType type1 = gem1.gemType;
            GemType type2 = gem2.gemType;
            //
            gems[gem1.row, gem1.col].col = col2;
            gems[gem2.row, gem2.col].col = col1;
            
            //
            gems[gem1.row, gem1.col].gemType = type2;
            gems[gem2.row, gem2.col].gemType = type1;

            //Gem temp1 = Instantiate(gem1, new Vector3(0f, -50f, 0f), Quaternion.identity);
            //Gem temp2 = Instantiate(gem2, new Vector3(0f, -50f, 0f), Quaternion.identity);
            //temp1.col = col2;
            //temp2.col = col1;

            //gems[gem1.row, gem1.col] = temp2;
            //gems[gem2.row, gem2.col] = temp1;

            //Destroy(temp1);
            //Destroy(temp2);

            string prnt = gems[7, 0].Info + " " + gems[7, 1].Info + " " + gems[7, 2].Info + " " + gems[7, 3].Info;
            Debug.Log(prnt.ToString());
        }
        if (sameCol)
        {
            Debug.Log("comparing cols");

            string prnt = gems[2, 1].Info + " " + gems[3, 1].Info + " " + gems[4, 1].Info + " " + gems[5, 1].Info;
            Debug.Log(prnt.ToString());
            //int col1 = gem1.col;
            //int col2 = gem2.col;
            //
            //gem1.col = col2;
            //gem2.col = col1;
        }

        //gems[gem1.row, gem1.col].row = row2;
        //gems[gem1.row, gem1.col].col = col2;
        //
        //gems[gem2.row, gem2.col].row = row1;
        //gems[gem2.row, gem2.col].col = col1;
        yield return new WaitForSeconds(0.5f);

        bool match = CheckForMatches();



        //gem1.gemType = type2;
        //gem2.gemType = type1;


        selectedGems.Clear();

        SwappingGems = false;
    }
    private void SetGridPhysicsOn(bool enabled)
    {
        foreach(Gem g in gems)
        {
            if (g != null)
                g.GetComponent<Rigidbody2D>().isKinematic = !enabled;
        }
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
    public bool CheckForMatches()
    {

        bool matchMade = false;
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
                        matchMade = true;
                        continue;
                    }
                }
                if (AddNeighborColums(row, col, 5))
                {
                    if (CheckForMatchOf5(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }

                if (AddNeighborColums(row, col, 4))
                {
                    if (CheckForMatchOf4(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }
                if (AddNeighborColums(row, col, 3))
                {
                    if (CheckForMatchOf3(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }
                // ROWS
                if (AddNeighborRows(row, col, 6))
                {
                    if (CheckForMatchOf6(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }

                if (AddNeighborRows(row, col, 5))
                {
                    if (CheckForMatchOf5(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }

                if (AddNeighborRows(row, col, 4))
                {
                    if (CheckForMatchOf4(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }
                if (AddNeighborRows(row, col, 3))
                {
                    if (CheckForMatchOf3(row, col))
                    {
                        // TODO: Destroy all matched gems and change the array location of the gems above..
                        matchMade = true;
                        continue;
                    }
                }


            }
        }
        DestroyMatchedGems();
        GridReady = false;
        return matchMade;
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
