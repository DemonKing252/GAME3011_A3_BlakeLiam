using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class GridManager : MonoBehaviour
{
    public delegate void TurnOffKinematics();
    public TurnOffKinematics onTurnOffKinematics;

    public bool SwappingGems = false;
    public bool CanMatch = false;
    public bool GridReady;
    public Gem[,] gems = new Gem[8, 8];
    public List<Gem> gemList = new List<Gem>();

    public List<Gem> matchedGems = new List<Gem>();
    
    [SerializeField] private List<Gem> tempGems = new List<Gem>();

    private static GridManager instance;
    [SerializeField] private List<Gem> selectedGems = new List<Gem>();

    [SerializeField] private TMP_Text matchesandScoreText;
    [SerializeField] private TMP_Text chainText;
    [SerializeField] private TMP_Text cascadeText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text objectiveText;

    [SerializeField] public AudioSource btnClickSfx;
    [SerializeField] public AudioSource themeSfx;

    private int currentScore = 0;
    private int currentMatches = 0;
    private int currentChain = 0;
    private int currentCascade = 0;

    private int bestChain = 0;
    private int bestCascade = 0;

    [SerializeField] private float startingTime = 180f;

    private float time = 0f;

    private int minutes = 0;
    private int seconds = 0;

    public bool runTimer = false;

    public int Score
    {
        get { return currentScore; }
        set 
        {
            currentScore = value; 
            matchesandScoreText.text = "Matches:\n" + currentMatches.ToString() + "\nScore:\n" + currentScore.ToString(); 
        }
    }
    public int Matches
    {
        get { return currentMatches; }
        set 
        { 
            currentMatches = value; 
            matchesandScoreText.text = "Matches:\n" + currentMatches.ToString() + "\nScore:\n" + currentScore.ToString(); 
        }
    }
    public int Chain
    {
        get { return currentChain; }
        set 
        { 
            currentChain = value;
            if (currentChain > bestChain)
                bestChain = value;

            chainText.text = "Chain:\n" + currentChain + "\n<size=90%><color=grey>Best:\n" + bestChain.ToString() + "</size></color>";
        }
    }
    public int Cascade
    {
        get { return currentCascade; }
        set
        {
            currentCascade = value;
            if (currentCascade > bestCascade)
                bestCascade = value;

            cascadeText.text = "Cascade:\n" + currentCascade + "\n<size=90%><color=grey>Best:\n" + bestCascade.ToString() + "</size></color>";
        }
    }

    private Difficulty difficulty;
    public Difficulty Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }

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
        time = startingTime;
    }

    public void SetDesc()
    {
        objectiveText.text = MenuController.Instance.DescName;
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer)
        {
            time -= Time.deltaTime;

            if (time < 0f)
                MenuController.Instance.OnAction((int)Action.Defeat);

            time = Mathf.Clamp(time, 0f, Mathf.Infinity);
            minutes = Mathf.FloorToInt(time / 60f);
            seconds = (int)time % 60;
            timeText.text = minutes.ToString() + ":" + seconds.ToString("00");
        }

        if (!GridReady || SwappingGems)
            return;
        CheckForMatches();

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

        foreach(Gem g in gemList)
        {
            if (g.gemBehaviour == GemBehaviour.TickingBomb)
            {
                g.NumMoves--;

                if (g.NumMoves <= 0)
                    MenuController.Instance.OnAction((int)Action.Defeat);
            }
        }
        if (!sameRow && !sameCol)
        {
            selectedGems.Clear();
            yield break;
        }

        // Horizontal swap
        if (sameRow)
        {
            // The gems selected are not next to eachother
            if (Mathf.Abs(gem1.col - gem2.col) > 1)
            {
                selectedGems.Clear();
                yield break;
            }
        }
        // Vertical swap
        if (sameCol)
        {
            // The gems selected are not next to eachother
            if (Mathf.Abs(gem1.row - gem2.row) > 1)
            {
                print("not next to eachother");
                selectedGems.Clear();
                yield break;
            }
        }
        Cascade = 0;
        //print("this shouldbe be here");
        while (t < 0.75f)
        {
            gem1.transform.position = Vector3.Lerp(gem1Loc, gem2Loc, t / 0.75f);         
            gem2.transform.position = Vector3.Lerp(gem2Loc, gem1Loc, t / 0.75f);

            t += Time.deltaTime;
            yield return null;
        }
        SetGridPhysicsOn(true);
        // Check for matches on the board
        bool match = CheckForMatches();
        if (!match)
        {
            // There is no match, swap them back
            Chain = 0;
            SetGridPhysicsOn(false);
            while (t > 0f)
            {
                gem1.transform.position = Vector3.Lerp(gem1Loc, gem2Loc, t / 0.75f);
                gem2.transform.position = Vector3.Lerp(gem2Loc, gem1Loc, t / 0.75f);

                t -= Time.deltaTime;
                yield return null;
            }
            SetGridPhysicsOn(true);
        }

        selectedGems.Clear();
        SwappingGems = false;
    }
    private void SetGridPhysicsOn(bool enabled)
    {
        foreach(Gem g in gemList)
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
                if (difficulty == Difficulty.Advanced)
                    MenuController.Instance.OnAction((int)Action.Victory);
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

                if (difficulty == Difficulty.Advanced)
                    MenuController.Instance.OnAction((int)Action.Victory);
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
                if (difficulty == Difficulty.Advanced)
                    MenuController.Instance.OnAction((int)Action.Victory);

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

        if (matchedGems.Count != 0)
        {
            Score += 100;
            Matches++;
            Chain++;
            Cascade++;

            if (difficulty == Difficulty.Begginer && Matches >= 20)
            {
                MenuController.Instance.OnAction((int)Action.Victory);
            }
            if (difficulty == Difficulty.Intermediate && Chain >= 30)
            {
                MenuController.Instance.OnAction((int)Action.Victory);
            }
            if (difficulty == Difficulty.Expert && Cascade >= 3)
            {
                MenuController.Instance.OnAction((int)Action.Victory);
            }
        }

        foreach (Gem g in matchedGems)
        {
            btnClickSfx.Play();
            gemList.Remove(g);
            Destroy(g.gameObject);
        }


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
