using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        //加载游戏进度
        SaveLoadManager.Instance.Load();
    }

    //Inventory的右上角菜单
    public void GoBackToMenu()
    {
        //获得当前场景的名称
        var currentScene = SceneManager.GetActiveScene().name;
        transitionManager.Instance.Transition(currentScene,"Menu");

        // 保存游戏进度
        SaveLoadManager.Instance.Save();
    }

    public void StartGameWeek(int gameWeek)
    {
        EventHandler.CallStartNewGameEvent(gameWeek);
    }
}
