using System.IO;
using UnityEngine;

public static class SaveLevelDesign
{    
    public static readonly string path = Application.dataPath + "/Saves/";
    const string TEMPORARYJSON = "tenporarySave.json";
    const string PERMANENTJSON = "Saves.json";

    public static void Init()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    public static void Save(string saveString)
    {        
        File.WriteAllText(path + TEMPORARYJSON ,saveString);
    }
 
    public static string Load()
    {
        if (File.Exists(path + PERMANENTJSON))
        {
            var s = File.ReadAllText(path + PERMANENTJSON);
            return s;
        }
        else
        {
            return null;
        }
    }

}
