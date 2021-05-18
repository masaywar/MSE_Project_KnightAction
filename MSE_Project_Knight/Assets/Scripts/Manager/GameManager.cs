using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isOver = false;

    [SerializeField]
    public float deltaTime {
        get
        {
            return Time.deltaTime;
        }
    }

    [SerializeField]
    private float _cahcedTimeScale = 1f;
    public float cachedTimeScale
    {
        get
        {
            return _cahcedTimeScale;
        }

        set
        {
            _cahcedTimeScale = value;
        }
    }

    public enum GameState
    { 
        start, loadMain, main, loadIngame, ingame, idle
    };

    public GameState gameState = GameState.start;

    //For test
    private void Awake()
    {
        cachedTimeScale = Time.timeScale;
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);

        LoadManager();
    }

    private AsyncOperation operation = null;

    private void LoadManager()
    {
        ObjectManager.Instance.Initialize();
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.start:
                if (!ObjectManager.Instance.IsLoaded())
                    print("로딩 중...");

                else 
                {
                    print("로딩 끝...");
                    gameState = GameState.loadMain;
                }
                break;

            case GameState.loadMain:
                if (operation == null)
                {
                    operation = SceneManager.LoadSceneAsync("Menu");
                    operation.allowSceneActivation = false;
                }
                else
                {
                    if (operation.progress >= 0.9f)
                    {
#if UNITY_EDITOR
                        if (Input.GetMouseButtonDown(0))
                        {
                            operation.allowSceneActivation = true;
                            gameState = GameState.main;
                            operation = null;
                        }
#else
                        if (Input.touchCount > 0)
                        {
                            operation.allowSceneActivation = true;
                            gameState = GameState.main;
                            operation = null;
                        }
#endif
                    }
                }
                break;

            case GameState.main:
                break;

            case GameState.loadIngame:
                break;

            case GameState.ingame:
                break;

            default:
                break;
        }
    }
    
    public void Pause()
    {
        cachedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void Play()
    {
        Time.timeScale = cachedTimeScale;
    }
}
