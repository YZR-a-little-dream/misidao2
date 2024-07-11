using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveData 
{
    public int gameWeek;            //当前游戏周目
    public string currentScene;     //当前游戏场景
    public Dictionary<string,bool> miniGameStateDict;  //小游戏状态

    /// <summary>
    /// ObjectManager中的存储数据
    /// </summary>
    //bool代表初始化物品是否可用
    public Dictionary<ItemName,bool> itemAvailbleDict;
    //保存物品互动状态  string代表物品名字 bool为物品本身isDone
    public Dictionary<string,bool> interactiveStateDict;

    //ObjectManager中存储数据
    public List<ItemName> itemList;
}
