using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveable
{
    //string记录小游戏的名字  bool用来存储游戏的状态
    private Dictionary<string,bool> miniGameStateDict = new Dictionary<string,bool>();

    private GameController currentGame;

    //第几周目
    private int gameWeek;

    private void OnEnable() {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.GamePassEvent += OnGamePassEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.GamePassEvent -= OnGamePassEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void Start() {
        //将Menu菜单作为一开始的启动项
        SceneManager.LoadSceneAsync("Menu",LoadSceneMode.Additive);
        //用来设置游戏运行的初始状态，避免对话框不显示
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        
        //保存数据
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    //场景中有多个小游戏时
    private void OnAfterSceneLoadedEvent()
    {
       foreach (var miniGame in FindObjectsByType<MiniGame>(FindObjectsSortMode.None))
       {
            if(miniGameStateDict.TryGetValue(miniGame.gameName,out bool isPass))
            {
                miniGame.isPass = isPass;
                miniGame.UpdateMiniGameState();
            }
       }
        //设置周目信息
       currentGame = FindObjectOfType<GameController>();
       currentGame?.SetGameWeekData(gameWeek);
    }
    
    //更新对应小游戏场景的通过状态
    private void OnGamePassEvent(string gameName)
    {
        miniGameStateDict[gameName] = true;
    }

    private void OnStartNewGameEvent(int gameWeek)
    {
        this.gameWeek = gameWeek;
        miniGameStateDict.Clear();
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <returns></returns>
    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.gameWeek = this.gameWeek;
        saveData.miniGameStateDict = this.miniGameStateDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.gameWeek = saveData.gameWeek;
        this.miniGameStateDict = saveData.miniGameStateDict;
    }
}
