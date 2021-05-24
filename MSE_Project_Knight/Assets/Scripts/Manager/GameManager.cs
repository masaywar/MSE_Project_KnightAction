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
        start, signin ,loadMain, main, loadIngame, ingame, idle
    };

    public enum LoadState
    {
        init, onLoad, done
    };

    private LoadState loadState = LoadState.init;
    public GameState gameState = GameState.start;
    public string progressMessage = "";

    [SerializeField]
    private bool isPaused = false;

    public void Awake()
    {
        cachedTimeScale = 1;
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);

        LoadManager();
        StartCoroutine(UpdateState());
        DontDestroyOnLoad(this);
    }

    private AsyncOperation operation = null;

    private void LoadManager()
    {
        ObjectManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        progressMessage = "Loading...";
    }

    private IEnumerator UpdateState()
    {
        while (loadState != LoadState.done)
        {
            yield return null;
            switch (loadState)
            {
                case LoadState.init:
                    
                    loadState = LoadState.onLoad;
                    break;

                case LoadState.onLoad:
                    if (operation == null)
                    {
                        operation = SceneManager.LoadSceneAsync("SignIn");
                        operation.allowSceneActivation = false;
                    }
                    else
                    {
                        if (operation.progress >= 0.9f)
                        {
#if UNITY_EDITOR        
                            progressMessage = "Touch To Start!!";
                            if (Input.GetMouseButtonDown(0))
                            {
                                operation.allowSceneActivation = true;
                                gameState = GameState.signin;
                                loadState = LoadState.done;
                                operation = null;
                            }
#else
                            progressMessage = "Touch To Start!!";
                            if (Input.touchCount > 0)
                            {
                                operation.allowSceneActivation = true;
                                gameState = GameState.signin;
                                loadState = LoadState.done;
                                operation = null;
                            }
#endif
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private void Update()
    {
        print(gameState);

        switch (gameState)
        {
            case GameState.signin:
                gameState = GameState.idle;
                break;

            case GameState.main:
                SceneManager.LoadScene("Menu");
                gameState = GameState.idle;
                break;

            case GameState.loadIngame:
                SceneManager.LoadScene("Ingame");
                gameState = GameState.ingame;
                break;
        }
    }

    public void Pause()
    {
        if (isPaused) return;

        isPaused = true;
        cachedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Play()
    {
        if (!isPaused) return;

        isPaused = false;

        if (cachedTimeScale == 0)
            cachedTimeScale = 1;

        Time.timeScale = cachedTimeScale;
    }
}
