using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    //游戏进度存储路径
    private string jsonFolder;
    //各个脚本中的存储数据列表
    private List<ISaveable> saveableList = new List<ISaveable>();
    //游戏进度数据 manager对应具体名称 GameSaveData对应具体数据
    private Dictionary<string,GameSaveData> saveDataDic = new Dictionary<string, GameSaveData>();

    //Awake继承自单例模式，要重写单例模式中的Awake
    protected override void Awake()
    {
        base.Awake();
        jsonFolder = Application.persistentDataPath + "/SAVE/";
    }
    private void OnEnable() {
        EventHandler.StartNewGameEvent += ONStartNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.StartNewGameEvent += ONStartNewGameEvent;
    }

    private void ONStartNewGameEvent(int obj)
    {
        //随便起的.sav后缀
        var resultPath = jsonFolder + "data.sav";
        if(File.Exists(resultPath))
        {
            File.Delete(resultPath);
        }
    }

    public void Register(ISaveable saveable)
    {
        saveableList.Add(saveable);
    }

    public void Save()
    {
        saveDataDic.Clear();

        foreach (var saveable in saveableList)
        {
            saveDataDic.Add(saveable.GetType().Name,saveable.GenerateSaveData());
        }

        //将存储字典序列化路径
        var resultPath = jsonFolder + "data.sav";

        string jsonData = JsonConvert.SerializeObject(saveDataDic,Formatting.Indented);

        //假如不存在
        if(!File.Exists(resultPath))
        {
            //创建对应路径的文件夹
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);
    }

    public void Load()
    {
        var resultPath = jsonFolder + "data.sav";

        if(!File.Exists(resultPath)) return;
        
        //读取文件
        var stringData = File.ReadAllText(resultPath);

        var jsonData = JsonConvert.DeserializeObject<Dictionary<string,GameSaveData>>(stringData);

        foreach (var saveable in saveableList)
        {
            saveable.RestoreGameData(jsonData[saveable.GetType().Name]);
        }
    }
}
