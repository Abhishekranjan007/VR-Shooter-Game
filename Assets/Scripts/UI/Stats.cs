using System.Collections;
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

    private int _score, _hits, _killCount, _total;

    
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

    public void SetTotal(int num)
    {
        _total = num;
    }

    private void ResetKillCount()
    {
        _killCount = 0;
        SetEnemyText(_total, _killCount);
    }

    private void ResetScore()
    {
        _score = 0;
        SetScore(_score);        
    }

    private void SetScore(int score)
    {
        scoreText.text = Constants.Score+ score.ToString();
        scoreText.transform.localScale = Vector3.one * 1.2f;
        StartCoroutine(ResetScale(scoreText.transform));        
    }

    private IEnumerator ResetScale(Transform target)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 enlargedScale = Vector3.one * 1.2f;

        float t = 0f;

        // Scale up quickly
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            target.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
            yield return null;
        }

        target.localScale = originalScale;
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

        if (health > 60)
            healthCount.color = Color.green;
        else if (health > 30)
            healthCount.color = Color.yellow;
        else
            healthCount.color = Color.red;
    }

    
    public void ResetStats()
    {
        ResetKillCount();
        ResetScore();
        Timer.Instance.ResetTime();
    }
}
