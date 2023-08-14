using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private void Update()
    {
        SavePosition("playerPosition",transform.position);
    }

    public static void SavePosition(string key, Vector3 position)
    {
        PlayerPrefs.SetFloat(key + "_X", position.x);
        PlayerPrefs.SetFloat(key + "_Y", position.y);
        PlayerPrefs.SetFloat(key + "_Z", position.z);
        PlayerPrefs.Save();
    }

    public static bool isFirstPosition(string key)
    {
        return PlayerPrefs.GetFloat(key + "_X")==0 && PlayerPrefs.GetFloat(key + "_Y")==0 && PlayerPrefs.GetFloat(key + "_Z")==0;
    }

    public static Vector3 LoadPosition(string key)
    {
        float x = PlayerPrefs.GetFloat(key + "_X", 0f);
        float y = PlayerPrefs.GetFloat(key + "_Y", 0f);
        float z = PlayerPrefs.GetFloat(key + "_Z", 0f);
        return new Vector3(x, y, z);
    }
}
