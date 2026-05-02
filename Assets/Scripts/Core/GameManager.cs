using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }

    private bool isGameOver = false;
    private bool isPaused = true;

    private int _level;

    public event Action onRestartGame;
    public event Action<bool> onGameOver;

    public GameObject[] levels;
    public Pistol pistol;
    public Rifle rifle;
    public SpawnMode currentMode;


    void Awake() 
    { 
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;            
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
        
    

    public void GameOver(int r)
    {
        SetGameOver(true);
        SetGamePause(true);
        if (r == 0)
        {
            onGameOver?.Invoke(false);
            Debug.Log("You Lose!!!");
        }
        else
        {
            onGameOver?.Invoke(true);
            Debug.Log("You Win!!!");
        }            
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void SetGameOver(bool val)
    {
        isGameOver = val;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void SetGamePause(bool val)
    {
        isPaused = val;
    }

    public void RestartGame()
    {
        isGameOver = false;
        isPaused = false;
        //Debug.Log("Inside GameManager RestartGame");
        onRestartGame?.Invoke();
    }

    //Level Change
    public void ChangeLevel(int lev)
    {
        _level = lev;
        for(int i = 0; i < levels.Length; i++)
        {
            if(i == _level)
            {
                levels[i].SetActive(true);
            }
            else
            {
                levels[i].SetActive(false);
            }
            
        }
        SetWeapons();
        SetMode();
        EnemySpawner spawner = levels[_level].GetComponentInChildren<EnemySpawner>();
        if (spawner != null)
        {
            spawner.ResetSpawnerDirect(); // new method
        }        

    }



    private void SetWeapons()
    {
        LevelRefences lev_ref = levels[_level].GetComponent<LevelRefences>();

        SetWeaponTransform(pistol, lev_ref.pistolRef);
        SetWeaponTransform(rifle, lev_ref.rifleRef);
    }

    private void SetWeaponTransform(Weapons weapon, Transform tr)
    {
        weapon.transform.SetPositionAndRotation(tr.position, tr.rotation);        
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        
    }

    public void SetTimedMode()
    {
        currentMode = SpawnMode.Timed;
        SetMode();
    }

    public void SetWaveMode()
    {
        currentMode = SpawnMode.Wave;
        SetMode();
    }

    //Mode Change
    public void SetMode()
    {
        isGameOver = false;
        isPaused = true;

        EnemySpawner spawner = levels[_level].GetComponentInChildren<EnemySpawner>();

        if (spawner != null)
        {
            spawner.ResetSpawnerDirect();
            spawner.SetMode(currentMode);            
        }
        
    }


    public void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


}
