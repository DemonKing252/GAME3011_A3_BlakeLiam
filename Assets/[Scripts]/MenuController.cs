using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public enum Difficulty
{
    Easy,
    Normal,
    Hard
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


    void Awake()
    {
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
            Difficulty.Easy   => "Get 25x matches in 3 minutes!\n<size=60%>Match up 25 gems, it doesn't matter what type, your not punished by miss clicking</size>",
            Difficulty.Normal => "Get a 40x chain in 3 minutes!\n<size=60%>Getting a chain requires you to get multiple matches in a row, without miss clicking any jewels!</size>",
            Difficulty.Hard   => "Get a 3x cascade in 3 minutes!\n<size=60%>Getting a cascade requires you to get multiple matches in ONE move!. For example, getting 4 matches in one move would be a x4 cascade!</size>",   
        };
        SetDescriptionText(desc);
    }

}
