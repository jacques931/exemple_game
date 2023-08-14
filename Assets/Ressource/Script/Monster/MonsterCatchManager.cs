using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterCatchManager : EncrypteData
{
    private static MonsterCatchManager _instance;

    public static MonsterCatchManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MonsterCatchManager();
            }
            return _instance;
        }
    }

    public void SaveMonsterList(Monster[] monsterList, string filePath = "monstersCatch.json")
    {
        filePath = Application.persistentDataPath + "/"+ filePath;
        string jsonData = JsonUtility.ToJson(new MonsterListWrapper(monsterList));
        string encryptedData = EncryptData(jsonData);
        System.IO.File.WriteAllText(filePath, encryptedData);
    }

    public Monster[] LoadMonsterList(string filePath = "monstersCatch.json")
    {
        filePath = Application.persistentDataPath + "/"+ filePath;
        if (System.IO.File.Exists(filePath))
        {
            string encryptedData = System.IO.File.ReadAllText(filePath);
            string jsonData = DecryptData(encryptedData);
            MonsterListWrapper wrapper = JsonUtility.FromJson<MonsterListWrapper>(jsonData);
            return wrapper.monsterList;
        }
        else
        {
            return new Monster[0];
        }
    }

    [System.Serializable]
    private class MonsterListWrapper
    {
        public Monster[] monsterList;

        public MonsterListWrapper(Monster[] monsterList)
        {
            this.monsterList = monsterList;
        }
    }

    private MonsterCatchManager() { }
}
