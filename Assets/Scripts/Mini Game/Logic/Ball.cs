using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //用于更改球的图片
    private SpriteRenderer spriteRenderer;
    public BallDetails ballDetails;
    public bool isMatch;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupBall(BallDetails ballDetails)
    {
        this.ballDetails = ballDetails;

        if(isMatch)
        {
            SetRight();
        }
        else
        {
            SetWrong();
        }
    }

    public void SetWrong()
    {
        spriteRenderer.sprite = ballDetails.wrongSprite;
    }

    public void SetRight()
    {
        spriteRenderer.sprite = ballDetails.rightSprite;
    }
}
