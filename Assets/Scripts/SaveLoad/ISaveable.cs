
public interface ISaveable
{
    void SaveableRegister()
    {
        //C#8.0之后支持
        SaveLoadManager.Instance.Register(this);
    }
    
    //生成读取数据
    GameSaveData GenerateSaveData();
    //恢复读取数据
    void RestoreGameData(GameSaveData saveData);
}