using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveJson : MonoBehaviour
{
    public static void SaveByjson(string SaveFilename, object data)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath + "/game_SaveData", SaveFilename);
        File.WriteAllText(path, json);
    }
    public static T LoadFromJson<T>(string SaveFilename)
    {
        var path = Path.Combine(Application.persistentDataPath + "/game_SaveData", SaveFilename);
        if (!File.Exists(path))
        {
            GameManager.Instance.SaveStar();
        }
        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception expection)
        {
            #if UNITY_EDITOR
            Debug.LogError($"成功儲存在{path}.\n{expection}");
            #endif
            return default;
        }
    }
}
