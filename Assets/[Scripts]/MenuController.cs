using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public enum Difficulty
{
    Begginer,
    Intermediate,
    Advanced,
    Expert
}
public enum Action
{
    Start      = 0,
    Quit       = 1,
    PlayAgain  = 2,
    MainMenu   = 3,
    Victory    = 4,
    Defeat     = 5
}
public class MenuController : MonoBehaviour
{
    private GridManager gridMgr;

    [SerializeField] TMP_Text descriptionText;

    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas loseCanvas;
    [SerializeField] Canvas winCanvas;

    private static MenuController instance;
    public static MenuController Instance
    {
        get { return instance; }
    }
    public string DescName
    {
        get { return descriptionText.text; }
    }

    void Awake()
    {
        instance = this;
        gridMgr = FindObjectOfType<GridManager>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        OnDifficultyDropDown(0);
    }
    public void OnAction(int action)
    {
        switch ((Action)action)
        {
            case Action.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
                break;
            case Action.MainMenu:
                menuCanvas.gameObject.SetActive(true);
                loseCanvas.gameObject.SetActive(false);
                winCanvas.gameObject.SetActive(false);
                Time.timeScale = 0f;
                break;
            case Action.Start:
                menuCanvas.gameObject.SetActive(false);
                loseCanvas.gameObject.SetActive(false);
                winCanvas.gameObject.SetActive(false);
                Time.timeScale = 1f;

                break;
            case Action.Victory:
                menuCanvas.gameObject.SetActive(false);
                loseCanvas.gameObject.SetActive(false);
                winCanvas.gameObject.SetActive(true);
                Time.timeScale = 0f;
                break;
            case Action.Defeat:
                menuCanvas.gameObject.SetActive(false);
                loseCanvas.gameObject.SetActive(true);
                winCanvas.gameObject.SetActive(false);
                Time.timeScale = 0f;
                break;
            case Action.PlayAgain:
                SceneManager.LoadScene("SampleScene");

                break;
        }

    }

    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }

    public void OnDifficultyDropDown(int val)
    {
        gridMgr.Difficulty = (Difficulty)val;
        string desc = (Difficulty)val switch
        {
            Difficulty.Begginer       => "Get 10x matches in 3 minutes!\n<size=60%>Get atleast 10 matches. Your not punished by miss clicking</size>",
            Difficulty.Intermediate   => "Get a 15x chain in 3 minutes!\n<size=60%>In order to get and MAINTAIN a chain, you need to get multiple matches in a row. If you make a move and theres no matches, your chain is reset</size>",   
            Difficulty.Advanced       => "Destroy 4 gems in one move!\n<size=60%>You have to get a match of 4 of any type of gem.</size>",   
            Difficulty.Expert         => "Get a 4x cascade in 3 minutes!\n<size=60%>This one WILL be difficult I can assure you. Getting a cascade requires you to get MULTIPLE matches in ONE move!. For example, getting 4 matches in one move would be a x4 cascade!</size>",   
        };
        SetDescriptionText(desc);
    }

}
