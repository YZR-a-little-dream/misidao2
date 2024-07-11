using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniGame : MonoBehaviour
{
    public UnityEvent onGameFinishUEvent;
    [SceneName] public string gameName;
    //游戏是否通关
    public bool isPass;

    public void UpdateMiniGameState()
    {
        if (isPass)
        {
            //关闭碰撞体
            GetComponent<Collider2D>().enabled = false;
            //把物体变成透明
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
            onGameFinishUEvent?.Invoke();
        }

        
    }
}
