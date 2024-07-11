using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class InteractiveBase : MonoBehaviour
{
    public ItemName requireItem;
    //互动情况是否完全结束了
    public bool isDone;

    //鼠标选中的物品是否是需求的物品
    public void CheckItem(ItemName itemName)
    {
        if(itemName == requireItem && !isDone)
        {
            isDone = true;
            //使用这个物体
            OnClickedAction();
            //移除背包中的物品
            EventHandler.CallItemUseEvent(itemName);
        }
    }

    //默认是正确的物品的情况执行（当选中的是正确的物品时执行该方法）
    protected virtual void OnClickedAction()
    {
        
    }

    public virtual void EmeptyClicked()
    {
        Debug.Log("空点");
    }

}
