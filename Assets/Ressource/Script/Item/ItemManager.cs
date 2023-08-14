using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemManager : EncrypteData
{
    private static ItemManager _instance;
    
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
            }
            return _instance;
        }
    }

    public void SaveItemList(Item[] itemList, string filePath = "itemsInventory.json")
    {
        filePath = Application.persistentDataPath + "/"+ filePath;
        string jsonData = JsonUtility.ToJson(new ItemListWrapper(itemList));
        string encryptedData = EncryptData(jsonData);
        System.IO.File.WriteAllText(filePath, encryptedData);
    }

    public Item[] LoadItemList(string filePath = "itemsInventory.json")
    {
        filePath = Application.persistentDataPath + "/"+ filePath;
        if (System.IO.File.Exists(filePath))
        {
            string encryptedData = System.IO.File.ReadAllText(filePath);
            string jsonData = DecryptData(encryptedData);
            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(jsonData);
            return wrapper.itemList;
        }
        else
        {
            return new Item[0];
        }
    }

    [System.Serializable]
    private class ItemListWrapper
    {
        public Item[] itemList;

        public ItemListWrapper(Item[] itemList)
        {
            this.itemList = itemList;
        }
    }
}
