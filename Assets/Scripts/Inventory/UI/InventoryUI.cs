using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Button leftButton,rightButton;
    public SlotUI slotUI;
    //显示UI当前物品序号
    public int currentIndex;

    private void OnEnable() {
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

    private void OnDisable() {
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int index)
    {
        if(itemDetails == null)
        {
            slotUI.SetEmpty();
            currentIndex = -1;
            leftButton.interactable = false;
            rightButton .interactable = false;
        }
        else
        {
            currentIndex = index;           //用于切换物品栏
            slotUI.SetItem(itemDetails);

            if(index > 0)
               leftButton.interactable = true;
            if(index == -1)
            {
                leftButton.interactable = false;
                rightButton.interactable = false;
            }
        }
    }

    /// <summary>
    /// 左右按钮事件
    /// </summary>
    /// <param name="amount"></param>
    public void SwitchItem(int amount)
    {
        //FIXME: 当前逻辑只适用于两个道具的切换 
        int index = currentIndex + amount;
        
        if(index < currentIndex)
        {
            leftButton.interactable = false;
            rightButton .interactable = true;
        }
        else if(index > currentIndex)
        {
            leftButton.interactable = true;
            rightButton .interactable = false;
        }
        else    //多于两个物体的情况
        {
            leftButton.interactable = true;
            rightButton .interactable = true;
        }

        //触发左右按钮的事件
        EventHandler.CallChangeItemEvent(index);
    }

    
}
