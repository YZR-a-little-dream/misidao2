using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : InteractiveBase
{
    //当前格子所需的正确的格子
    public BallName matchBall;
    //当前物体本身的球
    public Ball currentBall;
    //存储能与本Holder连线的其它Holder  HaseSet保证数据唯一
    public HashSet<Holder> linkHolders = new HashSet<Holder>();
    public bool isEmpty;

    public void checkBall(Ball ball)
    {
        currentBall = ball;
        if(ball.ballDetails.ballName == matchBall)
        {
            currentBall.isMatch = true;
            currentBall.SetRight();
        }
        else
        {
            currentBall.isMatch = false;
            currentBall.SetWrong();
        }
    }

    public override void EmeptyClicked()
    {
        foreach (var holder in linkHolders)
        {
            if(holder.isEmpty)
            {
                //移动球
                currentBall.transform.position = holder.transform.position;
                currentBall.transform.SetParent(holder.transform);

                //交换球
                holder.checkBall(currentBall);
                this.currentBall = null;

                //改变状态
                this.isEmpty = true;
                holder.isEmpty = false;

                //每移动一次触发一次事件判定游戏是否成功
                EventHandler.CallCheckGameStateChangeEvent();
            }
        }
    }
}
