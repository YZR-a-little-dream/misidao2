using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;

    public void UpdateItemName(ItemName itemName)
    {
        //当itemName是枚举类型时可以使用swith语句
        itemNameText.text = itemName switch
        {
            ItemName.Key => "信箱钥匙",
            ItemName.Ticket =>"一张船票",
            _ => ""
        };
    }
}
