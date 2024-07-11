using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : InteractiveBase
{
    //切换图片显示
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    //打开时切换图片
    public Sprite openSprite;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnEnable() {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    

    private void OnDisable() {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }
    
    private void OnAfterSceneLoadedEvent()
    {
        //当互动情况还没结束时
        if(!isDone)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            //图片切换并且关闭碰撞体
            spriteRenderer.sprite = openSprite;
            coll.enabled = false;
        }
    }

    protected override void OnClickedAction()
    {
        spriteRenderer.sprite = openSprite;
        //显示船票
        transform.GetChild(0).gameObject.SetActive(true);
    }

    
}
