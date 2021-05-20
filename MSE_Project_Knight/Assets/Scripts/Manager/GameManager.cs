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

    public enum LoadState
    { 
        init, onLoad, done
    };

    private LoadState loadState = LoadState.init;
    public GameState gameState = GameState.start;
    public string progressMessage = "";

    [SerializeField]
    private bool isPaused = false;

    private void Awake()
    {
        cachedTimeScale = Time.timeScale;
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);

        gameState = GameState.loadMain;
        StartCoroutine(UpdateState());

    }

    private AsyncOperation operation = null;

    private void LoadManager()
    {
        ObjectManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
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
                    LoadManager();
                    loadState = LoadState.onLoad;
                    gameState = GameState.loadMain;
                    break;

                case LoadState.onLoad:
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
                            progressMessage = "Touch To Start!!";
                            print(progressMessage);
                            if (Input.GetMouseButtonDown(0))
                            {
                                operation.allowSceneActivation = true;
                                gameState = GameState.main;
                                loadState = LoadState.done;
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

                default:
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            var window = UIManager.Instance.GetWindow<QuitUI>("QuitUI");

            if (window == null)
            {
                return;
            }


            if (!window.IsOpen()) 
            {
                Pause();
                window.Open();
            }
        }

        switch (gameState)
        {
            case GameState.loadIngame:
                SceneManager.LoadScene("Ingame");
                gameState = GameState.ingame;
                break;
        }
    }

    public void Pause()
    {
        isPaused = true;
        cachedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void Play()
    {
        isPaused = false;
        Time.timeScale = cachedTimeScale;
    }
}
