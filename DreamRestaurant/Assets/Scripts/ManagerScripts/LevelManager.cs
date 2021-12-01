using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int checkLevel;

    public int currentLevel, levelCount;
    public const int TOTALLEVELS = 10;
    public GameObject loadedLevel;
    public string levelStartType;
    public List<int> totalLevels;
    public string dayLevelProgress;

    public int LEVELCOUNT { get { levelCount = PlayerPrefs.GetInt("levelCount"); return levelCount; } set { levelCount = value; PlayerPrefs.SetInt("levelCount", value); } }
    public int CURRENTLEVEL { get { currentLevel = PlayerPrefs.GetInt("Level"); return currentLevel; } set { currentLevel = value; PlayerPrefs.SetInt("Level", value); } }

    public const int TOTALLEVELCOUNT = 10;

    public List<LevelData> levelData;
    [HideInInspector]
    public List<LevelData> data;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        AssignInstance();
        LoadLevelDesign();
    }
    private void Start()
    {
        StartCoroutine(Init());
    }
    public IEnumerator Init()
    {
        yield return null;
        LoadLevel();
    }
    public void LoadLevel()
    {
        if (loadedLevel)
        {
            Destroy(loadedLevel);
            loadedLevel = null;
        }
        SpawnLevel();
      
    }
    public void SpawnLevel()
    {
        for (int i = 0; i < LevelManager.Instance.data.Count; i++)
        {
            if (CURRENTLEVEL == i)
            {
                var count = LevelManager.Instance.data[i].objectsInLevels.Count;

                for (int j = 0; j < count; j++)
                {

                    for (int k = 0; k < LevelManager.Instance.data[i].objectsInLevels[j].sceneObjects.Count; k++)
                    {
                       
                        var gameObject = LeanPool.Spawn(Resources.Load("Levels/" + LevelManager.Instance.data[i].objectsInLevels[j].SceneObjectName) as GameObject);
                        

                        gameObject.transform.position = LevelManager.Instance.data[i].objectsInLevels[j].sceneObjectTransformPosition[k];
                        gameObject.transform.localScale = LevelManager.Instance.data[i].objectsInLevels[j].sceneObjectTransformScale[k];
                        gameObject.transform.rotation = LevelManager.Instance.data[i].objectsInLevels[j].sceneObjectTransformRotation[k];
                    }

                }
            }
        }
    }
    public void ChangeLevel()
    {
        LEVELCOUNT++;
        CURRENTLEVEL++;
        if (CURRENTLEVEL >= TOTALLEVELS)
        {
            CURRENTLEVEL = 0;
        }
        LoadLevel();
    }
    private void AssignInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
    private void LoadLevelDesign()
    {
        SaveLevelDesign.Init();
        AssignSceneObjectsTransforms();

        string load = SaveLevelDesign.Load();

        data = JsonHelper.ListFromJson<LevelData>(load);

       // AssignLevelData(data);
    }

    public void AssignLevelData(List<LevelData> data)
    {
        for (int i = 0; i < 10; i++)
        {
            levelData[i] = data[i];
        }
    }
    public void SaveLevelData()
    {
        var levelData = JsonHelper.ToJson(this.levelData, true);
        SaveLevelDesign.Save(levelData);
    }
    public void AssignSceneObjectsTransforms()
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            for (int j = 0; j < levelData[i].objectsInLevels.Count; j++)
            {
                levelData[i].objectsInLevels[j].AssignSceneObjectsTransform();
            }
        }
    }
}

[System.Serializable]
public class LevelData
{
    public string name;
    public float rayDistance;
    //public Stage stage;
    //public DragObjectTypes dragObjectTypes;
    public List<ObjectsInLevel> objectsInLevels;

}

[System.Serializable]
public struct ObjectsInLevel
{
    public string name;

    public List<GameObject> sceneObjects;
    public string SceneObjectName;
    public List<Vector3> sceneObjectTransformPosition;
    public List<Quaternion> sceneObjectTransformRotation;
    public List<Vector3> sceneObjectTransformScale;
    public void AssignSceneObjectsTransform()
    {
        for (int i = 0; i < sceneObjects.Count; i++)
        {
            sceneObjectTransformPosition.Add(sceneObjects[i].transform.position);
            sceneObjectTransformRotation.Add(sceneObjects[i].transform.rotation);
            sceneObjectTransformScale.Add(sceneObjects[i].transform.localScale);
        }
        
    }
}