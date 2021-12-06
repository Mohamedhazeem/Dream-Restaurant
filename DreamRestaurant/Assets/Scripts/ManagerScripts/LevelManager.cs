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
        for (int i = 0; i < data.Count; i++)
        {
            if (CURRENTLEVEL == i)
            {
                var objectCount = data[i].objectsInLevels.Count;
                var GameUiCount = data[i].gamePlayUis.Count;

                for (int j = 0; j < objectCount; j++)
                {

                    for (int k = 0; k < data[i].objectsInLevels[j].sceneObjects.Count; k++)
                    {
                       
                        var gameObject = LeanPool.Spawn(Resources.Load("Levels/" + data[i].objectsInLevels[j].sceneObjectName) as GameObject);

                        gameObject.transform.position = data[i].objectsInLevels[j].sceneObjectTransformPosition[k];
                        gameObject.transform.localScale = data[i].objectsInLevels[j].sceneObjectTransformScale[k];
                        gameObject.transform.rotation = data[i].objectsInLevels[j].sceneObjectTransformRotation[k];
                    }

                }
                for (int a = 0; a < GameUiCount; a++)
                {
                    for (int b = 0; b < data[i].gamePlayUis[a].moneyGetPlaces.Count; b++)
                    {
                        var gameObject = LeanPool.Spawn(Resources.Load("MoneyPlaces/" + data[i].gamePlayUis[a].moneyGetPlaces[b].getPlaces.tag) as GameObject);

                        gameObject.transform.position = data[i].gamePlayUis[a].moneyPlacesTransformPosition[b];
                        gameObject.transform.localScale = data[i].gamePlayUis[a].moneyPlacesTransformScale[b];
                        gameObject.transform.rotation = data[i].gamePlayUis[a].moneyPlacesTransformRotation[b];
                    }
                    for (int c = 0; c < data[i].gamePlayUis[a].moneySpendPlaces.Count; c++)
                    {
                        var gameObject = LeanPool.Spawn(Resources.Load("MoneyPlaces/" + data[i].gamePlayUis[a].moneySpendPlaces[c].spendPlaces.tag) as GameObject);

                        gameObject.transform.position = data[i].gamePlayUis[a].spendPlacesTransformPosition[c];
                        gameObject.transform.localScale = data[i].gamePlayUis[a].spendPlacesTransformScale[c];
                        gameObject.transform.rotation = data[i].gamePlayUis[a].spendPlacesTransformRotation[c];
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
        //LoadLevel();
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
            for (int k = 0; k < levelData[i].gamePlayUis.Count; k++)
            {
                levelData[i].gamePlayUis[k].AssignUIObjectsTransform();
                levelData[i].gamePlayUis[k].AssignSpendPlaceData();
            }
        }
    }
}

[System.Serializable]
public class LevelData
{
    public string LevelName;
    public List<GamePlayUI> gamePlayUis;
    public List<ObjectsInLevel> objectsInLevels;
}

[System.Serializable]
public struct ObjectsInLevel
{
    public string name;

    public List<GameObject> sceneObjects;
    public string sceneObjectName;
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
[System.Serializable]
public struct GamePlayUI
{
    [SerializeField] private string GamePlayUIname;
    //public List<GameObject> moneyPlaces;
    public List<MoneyGetPlace> moneyGetPlaces;
    //public List<GameObject> spendPlaces;
    public List<MoneySpendPlace> moneySpendPlaces;

    [HideInInspector] public List<Vector3> moneyPlacesTransformPosition;
    [HideInInspector] public List<Quaternion> moneyPlacesTransformRotation;
    [HideInInspector] public List<Vector3> moneyPlacesTransformScale;

    [HideInInspector] public List<Vector3> spendPlacesTransformPosition;
    [HideInInspector] public List<Quaternion> spendPlacesTransformRotation;
    [HideInInspector] public List<Vector3> spendPlacesTransformScale;
    public void AssignUIObjectsTransform()
    {
        for (int i = 0; i < moneyGetPlaces.Count; i++)
        {
            moneyPlacesTransformPosition.Add(moneyGetPlaces[i].getPlaces.transform.position);
            moneyPlacesTransformRotation.Add(moneyGetPlaces[i].getPlaces.transform.rotation);
            moneyPlacesTransformScale.Add(moneyGetPlaces[i].getPlaces.transform.localScale);
        }
        for (int j = 0; j < moneySpendPlaces.Count; j++)
        {
            spendPlacesTransformPosition.Add(moneySpendPlaces[j].spendPlaces.transform.position);
            spendPlacesTransformRotation.Add(moneySpendPlaces[j].spendPlaces.transform.rotation);
            spendPlacesTransformScale.Add(moneySpendPlaces[j].spendPlaces.transform.localScale);
        }
    }
    public void AssignSpendPlaceData()
    {
        for (int i = 0; i < moneySpendPlaces.Count; i++)
        {
            SpendPlace spendPlace = moneySpendPlaces[i].spendPlaces.GetComponent<SpendPlace>();
            MoneySpendPlace moneySpendPlace = moneySpendPlaces[i];
            moneySpendPlace.count = spendPlace.count;
            moneySpendPlace.restaurantObjects = spendPlace.restaurantObjects;
            moneySpendPlaces[i] = moneySpendPlace;
        }
    }
}
[System.Serializable]
public struct MoneyGetPlace
{
    public GameObject getPlaces;    
}
[System.Serializable]
public struct MoneySpendPlace
{
    public GameObject spendPlaces;
    [HideInInspector] public int count;
    [HideInInspector] public RestaurantObjects restaurantObjects;
}
