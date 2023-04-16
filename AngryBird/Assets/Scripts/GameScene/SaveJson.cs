using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveJson : MonoBehaviour
{
    public static void SaveByjson(string SaveFilename, object data)
    {
        if (!Directory.Exists(Application.dataPath + "/Resources/gameSaveData/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/gameSaveData/");
        }
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.dataPath + "/Resources/gameSaveData/", SaveFilename);
        File.WriteAllText(path, json);
    }
    public static T LoadFromJson<T>(string SaveFilename)
    {
        var path = Path.Combine( Application.dataPath + "/Resources/gameSaveData/", SaveFilename);
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
        catch(System.Exception expection)
        {
            #if UNITY_EDITOR
            Debug.LogError($"成功儲存在{path}.\n{expection}");
            #endif
            return default;
        }
    }
}
