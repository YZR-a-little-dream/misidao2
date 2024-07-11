using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //保存物体的名字
    public ItemName itemName;

    //拾取物体到背包
    public void ItemClicked()
    {
        //添加到背包后隐藏物体
        InventoryManager.Instance.AddItem(itemName);
        this.gameObject.SetActive(false);
    }
}
