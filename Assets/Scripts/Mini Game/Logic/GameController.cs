using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class GameController : Singleton<GameController>
{
    public UnityEvent OnFinishUEvent;

    [Header("游戏数据")]
    public GameH2A_SO gameData;
    public GameH2A_SO[] gameDataArray;
    public GameObject lineParent;
    //各种预制体
    public LineRenderer linePrefab;
    public Ball ballPrefab;

    //黑色圆框的的位置
    public Transform[] holderTransforms;

    private void OnEnable() {
        EventHandler.CheckGameStateEvent += OnCheckGameStateEvent;
    }

    private void OnDisable() {
        EventHandler.CheckGameStateEvent -= OnCheckGameStateEvent;
    }

    private void Start() {
        //DrawLine();
        //CreateBall();
    }

    //检查游戏是否通关
    private void OnCheckGameStateEvent()
    {
        foreach (var ball in FindObjectsOfType<Ball>())
        {
            if(!ball.isMatch)
                return;
        }
        Debug.Log("游戏结束");
        
        //关闭碰撞体避免玩家可以进行连点操作
        foreach (var holder in holderTransforms)
        {
            holder.GetComponent<Collider2D>().enabled = false;
        }
        EventHandler.CallGamePassEvent(gameData.gameName);
        OnFinishUEvent?.Invoke();
    }
    
    //重置游戏
    public void ResetGame()
    {
        //不用删除线，只用删除重建球
        // for (int i = 0; i < lineParent.transform.childCount; i++)
        // {
        //     Destroy(lineParent.transform.GetChild(i).gameObject);            
        // }

        foreach (var holder in holderTransforms)
        {
            if(holder.childCount > 0)
                Destroy(holder.GetChild(0).gameObject);
        }
        //DrawLine();
        CreateBall();
    }

    public void DrawLine()
    {
        foreach (var connections in gameData.lineConnections)
        {
            var line = Instantiate(linePrefab,lineParent.transform);
            line.SetPosition(0,holderTransforms[connections.from].position);
            line.SetPosition(1,holderTransforms[connections.to].position);

            //创建每个Holder的连接关系  从from到to  从to到from
            holderTransforms[connections.from].GetComponent<Holder>().linkHolders
                .Add(holderTransforms[connections.to].GetComponent<Holder>());
            holderTransforms[connections.to].GetComponent<Holder>().linkHolders
                .Add(holderTransforms[connections.from].GetComponent<Holder>());
        }
    }

    //创建初始球
    public void CreateBall()
    {
        for (int i = 0; i < gameData.startBallOrder.Count; i++)
        {
            //跳过初始位置球的绘制
            if(gameData.startBallOrder[i] == BallName.None)
            {
                holderTransforms[i].GetComponent<Holder>().isEmpty = true;
                continue;
            }
            Ball ball = Instantiate(ballPrefab,holderTransforms[i]);
            //检查当前球是否为目标球
            holderTransforms[i].GetComponent<Holder>().checkBall(ball);
            //设置球的初始值与状态
            holderTransforms[i].GetComponent<Holder>().isEmpty = false;
            ball.SetupBall(gameData.GetBallDetails(gameData.startBallOrder[i]));
        }
    }

    //设置周目信息
    public void SetGameWeekData(int week)
    {
        gameData = gameDataArray[week];
        DrawLine();
        CreateBall();
    }
}
