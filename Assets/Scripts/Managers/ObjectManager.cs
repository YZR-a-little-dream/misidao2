using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 保存物体以及互动
/// </summary>
public class ObjectManager : MonoBehaviour,ISaveable
{
    //bool代表初始化物品是否可用
    private Dictionary<ItemName,bool> itemAvailbleDict = new Dictionary<ItemName, bool>();
    //保存物品互动状态  string代表物品名字 bool为物品本身isDone
    private Dictionary<string,bool> interactiveStateDict = new Dictionary<string,bool>();

    private void OnEnable() {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        
    }
    private void Start() {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnStartNewGameEvent(int obj)
    {
        //清空缓存字典
        itemAvailbleDict.Clear();
        interactiveStateDict.Clear();
    }

    private void OnBeforeSceneUnloadEvent()
    {
        foreach(var item in FindObjectsOfType<Item>())
        {
            if(!itemAvailbleDict.ContainsKey(item.itemName))
                itemAvailbleDict.Add(item.itemName,true);   //true代表在当前场景中显示出来
        }

        foreach (var item in FindObjectsOfType<InteractiveBase>())
        {
            if(interactiveStateDict.ContainsKey(item.name))
            {
                interactiveStateDict[item.name] = item.isDone;
            }
            else
            {
                interactiveStateDict.Add(item.name,item.isDone);
            }  
        }
    }

    private void OnAfterSceneLoadedEvent()
    {
        //如果已经在字典中则更新显示状态，不在则添加
        foreach(var item in FindObjectsOfType<Item>())
        {
            if(!itemAvailbleDict.ContainsKey(item.itemName))
                itemAvailbleDict.Add(item.itemName,true);   //true代表在当前场景中显示出来
            else
                item.gameObject.SetActive(itemAvailbleDict[item.itemName]);
        }

        foreach (var item in FindObjectsOfType<InteractiveBase>())
        {
            if(interactiveStateDict.ContainsKey(item.name))
            {
                item.isDone = interactiveStateDict[item.name];
            }
            else
            {
                interactiveStateDict.Add(item.name,item.isDone);
            }  
        }
    }

    //拾取物品后更新 显示在场景中设置为false
    private void OnUpdateUIEvent(ItemDetails itemDetails, int arg2)
    {
        if(itemDetails != null)
        {
            itemAvailbleDict[itemDetails.itemName] = false;
        }
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.itemAvailbleDict = this.itemAvailbleDict;
        saveData.interactiveStateDict = this.interactiveStateDict;

        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.itemAvailbleDict = saveData.itemAvailbleDict;
        this.interactiveStateDict = saveData.interactiveStateDict;
    }
}
