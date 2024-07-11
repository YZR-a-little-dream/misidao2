using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>,ISaveable
{
    public ItemDataList_SO itemData;
    [SerializeField] private List<ItemName> itemList = new List<ItemName>();

    private void OnEnable() 
    {
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
        EventHandler.ChangeItemEvent += OnchangeItemEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable() 
    {
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
        EventHandler.ChangeItemEvent -= OnchangeItemEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void Start() {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnStartNewGameEvent(int obj)
    {
        itemList.Clear();
    }

    private void OnItemUsedEvent(ItemName itemName)
    {
        //当物品使用后移除物品
        var index = GetItemIndex(itemName);
        itemList.RemoveAt(index);

        //TODO: 暂时实现单一使用物品效果
        if(itemList.Count == 0)
            EventHandler.CallUpdateUIEvent(null,-1);
    }

    //改变物品栏所触发的事件
    private void OnchangeItemEvent(int index)
    {
        if(index >= 0 && index < itemList.Count)
        {
            ItemDetails item = itemData.GetItemDetails(itemList[index]);
            EventHandler.CallUpdateUIEvent(item,index);
        }
    }

    //解决刚开始加载时SlotUI左右按钮显示的问题
    private void OnAfterSceneLoadedEvent()
    {
        if(itemList.Count == 0)
           EventHandler.CallUpdateUIEvent(null,-1);
        else    //当以后背包中有很多物品时
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemList[i]),i);
            }
        }
    }

    public void AddItem(ItemName itemName)
    {
        if(!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            // UI对应显示   触发事件
            EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemName),itemList.Count - 1);
        }
    }

    //获得背包物体中当前物体的序号
    private int GetItemIndex(ItemName itemName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if(itemList[i] == itemName)
                return i;
        }
        return -1;
    }

    public GameSaveData GenerateSaveData()
    {
       GameSaveData saveData = new GameSaveData();
       saveData.itemList = this.itemList;
       return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.itemList = saveData.itemList;
    }
}
