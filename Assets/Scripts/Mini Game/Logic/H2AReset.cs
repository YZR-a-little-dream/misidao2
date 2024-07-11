using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class H2AReset : InteractiveBase
{
    //子物体位置
    private Transform gearSprite;

    private void Awake() {
        gearSprite = transform.GetChild(0);
    }
    
    public override void EmeptyClicked()
    {
        //重置游戏  让小齿轮转过去在转过来  沿着z轴旋转
        GameController.Instance.ResetGame();
        gearSprite.DOPunchRotation(Vector3.forward * 180,1,1,0);
    }
}
