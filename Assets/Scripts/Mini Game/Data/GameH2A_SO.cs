using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameH2A_SO", menuName = "Mini Game Data/GameH2A_SO")]
public class GameH2A_SO : ScriptableObject {
    //小游戏所在的场景 以后可能会有多个小游戏
    [SceneName] public string gameName;

    [Header("球的名字和对应的图片")]
    public List<BallDetails> ballDataList;
    
    [Header("游戏逻辑数据")]
    public List<connections> lineConnections;       //连接线
    public List<BallName> startBallOrder;           //开始游戏球的顺序

    public BallDetails GetBallDetails(BallName ballName)
    {
        return ballDataList.Find(b => b.ballName == ballName);
    }
}

[System.Serializable]
public class BallDetails
{
    public BallName ballName;
    public Sprite wrongSprite;
    public Sprite rightSprite;
}

//连线
[System.Serializable]
public class connections
{
    public int from;
    public int to;
}