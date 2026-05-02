using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject uIParent;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject howtoPlayMenu;
    [SerializeField] private GameObject objectiveMenu;
    [SerializeField] private GameObject controlMenu;
    [SerializeField] private GameObject gamePlayMenu;
    [SerializeField] private GameObject scoringMenu;
    [SerializeField] private GameObject tipsMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject settingsBtn;
    [SerializeField] private GameObject scoreParent;
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject modes;

    [SerializeField] private TMP_Text g_heading;
    [SerializeField] private TMP_Text g_score;
    [SerializeField] private TMP_Text g_enemiesKilled;
    [SerializeField] private TMP_Text g_time;

    
    private int _menu;    

    void OnEnable()
    {
        GameManager.Instance.onGameOver += GameOverUI;
    }

    void OnDisable()
    {
        GameManager.Instance.onGameOver -= GameOverUI;
    }
    

    public void SetHeading(string heading)
    {
        g_heading.text = heading;
    }

    public void SetScore(int score)
    {
        g_score.text = "Score : "+score.ToString();
    }

    public void SetEnemyKilled(int killedEnemies)
    {
        g_enemiesKilled.text = "Enemies Killed : "+ killedEnemies.ToString();
    }

    public void SetTime(float time)
    {
        int min = (int)time / 60;
        if(min != 0)
        {
            int sec = (int)time % 60;
            g_time.text = "Time : " + min + " mins" + " " + sec + " secs";
        }
        else
        {
            g_time.text = "Time : " + time + " secs";
        }            
    }

    public void SettingsClicked()
    {
        GameManager.Instance.SetGamePause(true);
        settingsBtn.SetActive(false);
        uIParent.SetActive(true);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        scoreParent.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.Instance.RestartGame();

        GameManager.Instance.SetGamePause(false);
        settingsBtn.SetActive(true);
        scoreParent.SetActive(true);
        uIParent.SetActive(false);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void ShowMainMenu()
    {
        uIParent.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        scoreParent.SetActive(false);
        mainMenu.SetActive(true);        
    }    

    public void ShowScreen(GameObject go)
    {
        go.SetActive(true);
    }

    public void HideScreen(GameObject go)
    {
        go.SetActive(false);
    }

    public void Restart()
    {        
        GameManager.Instance.RestartGame();
        Stats.Instance.ResetStats();
        StartGame();
    }

    public void ShowSettingVol(int val)
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingMenu.SetActive(true);
        _menu = val;
    }

    public void HideSettingVol()
    {
        if (_menu == 1)
        {
            mainMenu.SetActive(true);
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            settingMenu.SetActive(false);
            levels.SetActive(false);
            modes.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            settingMenu.SetActive(false);
            levels.SetActive(false);
            modes.SetActive(false);
        }        
    }


    public void ShowLevels()
    {
        mainMenu.SetActive(false);
        levels.SetActive(true);
        modes.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingMenu.SetActive(false);
    }

    public void HideLevelAndModes()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingMenu.SetActive(false);
        levels.SetActive(false);
        modes.SetActive(false);
    }

    public void ShowModes()
    {
        mainMenu.SetActive(false);
        levels.SetActive(false);
        modes.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingMenu.SetActive(false);
    }  

    public void ShowHowToPlay()
    {
        howtoPlayMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void HideHowToPlay()
    {
        howtoPlayMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowGoChilds(GameObject go)
    {
        go.SetActive(true);
        howtoPlayMenu.SetActive(false);
    }

    public void HideGoChilds(GameObject go)
    {
        go.SetActive(false);
        howtoPlayMenu.SetActive(true);
    }

    public void GameOverUI(bool flag)
    {        
        uIParent.SetActive(true);
        gameOverMenu.SetActive(true);
        if (flag)
        {
            g_heading.text = "Mission Successful";
            g_score.text = "Score : "+Stats.Instance.GetScore().ToString();
            g_enemiesKilled.text = "Enemies Killed : " + Stats.Instance.GetKillCount().ToString();
            g_time.text = "Time : " + Timer.Instance.GetTime();
        }
        else
        {            
            g_heading.text = "Mission Failed";
            g_score.text = "Score : "+Stats.Instance.GetScore().ToString();
            g_enemiesKilled.text = "Enemies Killed : "+Stats.Instance.GetKillCount().ToString();
            g_time.text = "Time : " + Timer.Instance.GetTime();
        }
    }

    public void SetLevel(int level)
    {
        GameManager.Instance.ChangeLevel(level);
    }

    public void SetMode(int t)
    {
        if (t == 0)
        {
            GameManager.Instance.SetTimedMode();
        }
        else
        {
            GameManager.Instance.SetWaveMode();
        }       

        HideLevelAndModes();
    }

    public void Exit()
    {
        GameManager.Instance.Exit();
    }

}
