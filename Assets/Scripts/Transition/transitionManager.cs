using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class transitionManager : Singleton<transitionManager>,ISaveable
{
    //设置开始的初始场景
    [SceneName] public string startScene;

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;

    private bool isFade;

    //当对话完成后才能进行场景切换
    private bool canTransition;

    private void OnEnable() {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }
    private void OnDisable() 
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int obj)
    {
        StartCoroutine(TransitionToScene("Menu",startScene));
    }

    private void Start() {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnGameStateChangeEvent(GameState gameState)
    {
        canTransition = gameState == GameState.GamePlay;
    }

    //从哪来到哪去
    public void Transition(string from,string to)
    {
        if(!isFade && canTransition)
            StartCoroutine(TransitionToScene(from,to));
    }

    private IEnumerator TransitionToScene(string from,string to)
    {
        //先变黑 然后卸载场景
        yield return Fade(1);
        if(from != string.Empty)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return SceneManager.UnloadSceneAsync(from);
        }
        
        yield return SceneManager.LoadSceneAsync(to,LoadSceneMode.Additive);
        
        //找到新场景
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        //设置新场景为激活场景
        SceneManager.SetActiveScene(newScene);

        EventHandler.CallAfterSceneLoadEvent();
        yield return Fade(0);
    }

    /// <summary>
    /// 淡入淡出场景
    /// </summary>
    /// <param name="targetAlpha">1是黑,0是透明</param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha)
    {
        isFade  = true;

        fadeCanvasGroup.blocksRaycasts = true;

        yield return fadeCanvasGroup.DOFade(targetAlpha,fadeDuration).WaitForCompletion();

        //Debug.Log(fadeCanvasGroup.alpha);
        
        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;

        //执行完后清除动画效果
        //DOTween.Clear();
    }

    public GameSaveData GenerateSaveData()
    {
       GameSaveData saveData = new GameSaveData();
       saveData.currentScene = SceneManager.GetActiveScene().name;
       return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        Transition("Menu",saveData.currentScene);
    }
}
