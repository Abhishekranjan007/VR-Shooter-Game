using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }
    private float elapsedTime = 0f;
    private int lastSecond = -1;
    private bool isTimerActive = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTime();
    }

    public void StartTime()
    {
        isTimerActive = true;
    }

    public void ResetTime()
    {
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerActive && !GameManager.Instance.IsGameOver())
        {
            elapsedTime +=  Time.deltaTime;            
        }
    }

    //Time in min:sec format
    public string GetTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        string time = minutes + ":" + seconds+"s";

        return time;
    }
}
