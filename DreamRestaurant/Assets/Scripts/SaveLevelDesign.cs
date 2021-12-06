using System.IO;
using UnityEngine;
public static class SaveLevelDesign
{    
    public static readonly string path = Application.dataPath + "/TemporarySaves/";
    const string TEMPORARYJSON = "temporarySave.json";
    const string PERMANENTJSON = "Saves";

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
        TextAsset file = Resources.Load("Saves/" + PERMANENTJSON) as TextAsset;
        string json = file.text;
        return json;
        //if (File.Exists(path + PERMANENTJSON))
        //{
        //    var s = File.ReadAllText(path + PERMANENTJSON);
        //    return s;
        //}
        //else
        //{
        //    return null;
        //}
    }

}
