using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public RectTransform hand;
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint
        (new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
    
    private ItemName currentItem;   //临时变量用来存放当前物品

    private bool canClick;
    private bool holdItem;

    private void OnEnable() {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }

    private void OnDisable() {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }

    private void Update() {
        canClick = objectAtMousePosition();

        //当手在场景中被激活时设置手跟随鼠标操作
        if(hand.gameObject.activeInHierarchy)
            hand.position = Input.mousePosition;
        
        if(InteractWithUI()) return;

        if(canClick && Input.GetMouseButtonDown(0))
        {
            //检测鼠标互动情况
            ClickAction(objectAtMousePosition().gameObject);
        }
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        holdItem = isSelected;
        //拿了这个物品
        if(isSelected)
        {
            currentItem = itemDetails.itemName;
        }
        hand.gameObject.SetActive(holdItem);
    }

    private void OnItemUsedEvent(ItemName name)
    {
        currentItem = ItemName.None;
        holdItem = false;
        hand.gameObject.SetActive(false);
    }

    private void ClickAction(GameObject clickObject)
    {
       switch(clickObject.tag)
        {
            case "Teleport":
                var teleport = clickObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Item":
                var item = clickObject.GetComponent<Item>();
                item?.ItemClicked();
                break;
            case "Interactive":
                var interactive = clickObject.GetComponent<InteractiveBase>();
                if(holdItem)
                    interactive?.CheckItem(currentItem);
                else
                    interactive?.EmeptyClicked();
                break;
        }
    }
    
    /// <summary>
    /// 检测鼠标点击范围的碰撞体
    /// </summary>
    /// <returns></returns>
    private Collider2D objectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }

    //是否跟UI互动用来解决进入H3后 点击UI能跳转到H2
    private bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        else
            return false;
    }
}
