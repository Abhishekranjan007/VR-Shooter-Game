using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Stats : MonoBehaviour
{
    public static Stats Instance {  get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text enemyCount;
    [SerializeField] private TMP_Text healthCount;
    [SerializeField] private TMP_Text lowHealthWarning;

    private int _score, _hits, _killCount;

    
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void CalCulateScore(int killed)
    {
        _killCount++;
        _score += killed * 100;
        SetScore(_score);
    }


    public void AddScore(int amount = 10)
    {        
        _score +=  amount;
        SetScore(_score);
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetKillCount()
    {
        return _killCount;
    }

    private void ResetKillCount()
    {
        _killCount = 0;
        SetEnemyText(5, _killCount);
    }

    private void ResetScore()
    {
        _score = 0;
        SetScore(_score);        
    }

    private void SetScore(int score)
    {
        scoreText.text = Constants.Score+ score.ToString();
    }

    public void SetEnemyText(int total, int killed)
    {
        if (GameManager.Instance.currentMode == SpawnMode.Wave)
        {
            enemyCount.text = Constants.Killed + killed.ToString();
        }
        else
        {            
            enemyCount.text = Constants.Killed + killed.ToString() + "/" + total.ToString();
        }
            
    }

    public void SetHealthText(float health)
    {
        healthCount.text = Constants.Health + ((int)health).ToString()+"%";
    }

    
    public void ResetStats()
    {
        ResetKillCount();
        ResetScore();
        Timer.Instance.ResetTime();
    }
}
