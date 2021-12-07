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

                        gameObject.transform.position = data[i].gamePlayUis[a].moneyGetPlacesTransformPosition[b];
                        gameObject.transform.localScale = data[i].gamePlayUis[a].moneyGetPlacesTransformScale[b];
                        gameObject.transform.rotation = data[i].gamePlayUis[a].moneyGetPlacesTransformRotation[b];
                    }
                    int index = 0;
                    for (int b = 0; b < data[i].gamePlayUis[a].moneySpendPlaceToUnlockObjects.Count; b++)
                    {
                        var gameObject = LeanPool.Spawn(Resources.Load("MoneyPlaces/" + data[i].gamePlayUis[a].moneySpendPlaceToUnlockObjects[b].spendPlacesToUnlockObject.tag) as GameObject);

                        gameObject.transform.position = data[i].gamePlayUis[a].moneySpendToUnlockObjectTransformPosition[b];
                        gameObject.transform.localScale = data[i].gamePlayUis[a].moneySpendToUnlockObjectTransformScale[b];
                        gameObject.transform.rotation = data[i].gamePlayUis[a].moneySpendToUnlockObjectTransformRotation[b];

                        var spendplace = gameObject.GetComponent<SpendPlaceForUnlockObject>();
                        spendplace.moneyAmount = data[i].gamePlayUis[a].moneySpendPlaceToUnlockObjects[b].count;
                        spendplace.index = index;
                        spendplace.restaurantObjects = data[i].gamePlayUis[a].moneySpendPlaceToUnlockObjects[b].restaurantObjects;
                        index++;
                    }
                    index = 0;
                    for (int b = 0; b < data[i].gamePlayUis[a].moneySpendPlaceToUnlockAreas.Count; b++)
                    {
                        var gameObject = LeanPool.Spawn(Resources.Load("MoneyPlaces/" + data[i].gamePlayUis[a].moneySpendPlaceToUnlockAreas[b].spendPlacesToUnlockArea.tag) as GameObject);

                        gameObject.transform.position = data[i].gamePlayUis[a].moneySpendToUnlockAreaTransformPosition[b];
                        gameObject.transform.localScale = data[i].gamePlayUis[a].moneySpendToUnlockAreaTransformScale[b];
                        gameObject.transform.rotation = data[i].gamePlayUis[a].moneySpendToUnlockAreaTransformRotation[b];

                        var spendplace = gameObject.GetComponent<SpendPlaceForUnlockArea>();
                        spendplace.moneyAmount = data[i].gamePlayUis[a].moneySpendPlaceToUnlockObjects[b].count;
                        spendplace.index = index;
                        spendplace.unloackableAreas = data[i].gamePlayUis[a].moneySpendPlaceToUnlockAreas[b].unloackableAreas;
                        index++;
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
                levelData[i].gamePlayUis[k].AssignSpendPlaceToUnlockObjectData();
                levelData[i].gamePlayUis[k].AssignSpendPlaceTounlockAreaData();
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

    public List<MoneyGetPlace> moneyGetPlaces;

    public List<MoneySpendPlaceToUnlockObject> moneySpendPlaceToUnlockObjects;

    public List<MoneyspendToUnlockArea> moneySpendPlaceToUnlockAreas;

    [HideInInspector] public List<Vector3> moneyGetPlacesTransformPosition;
    [HideInInspector] public List<Quaternion> moneyGetPlacesTransformRotation;
    [HideInInspector] public List<Vector3> moneyGetPlacesTransformScale;

    [HideInInspector] public List<Vector3> moneySpendToUnlockObjectTransformPosition;
    [HideInInspector] public List<Quaternion> moneySpendToUnlockObjectTransformRotation;
    [HideInInspector] public List<Vector3> moneySpendToUnlockObjectTransformScale;

    [HideInInspector] public List<Vector3> unlockableObjectPrefabTransformPosition;
    [HideInInspector] public List<Quaternion> unlockableObjectPrefabTransformRotation;
    [HideInInspector] public List<Vector3> unlockableObjectPrefabTransformScale;

    [HideInInspector] public List<Vector3> moneySpendToUnlockAreaTransformPosition;
    [HideInInspector] public List<Quaternion> moneySpendToUnlockAreaTransformRotation;
    [HideInInspector] public List<Vector3> moneySpendToUnlockAreaTransformScale;

    [HideInInspector] public List<Vector3> unlockableAreaPrefabTransformPosition;
    [HideInInspector] public List<Quaternion> unlockableAreaPrefabTransformRotation;
    [HideInInspector] public List<Vector3> unlockableAreaPrefabTransformScale;
    public void AssignUIObjectsTransform()
    {
        for (int i = 0; i < moneyGetPlaces.Count; i++)
        {
            moneyGetPlacesTransformPosition.Add(moneyGetPlaces[i].getPlaces.transform.position);
            moneyGetPlacesTransformRotation.Add(moneyGetPlaces[i].getPlaces.transform.rotation);
            moneyGetPlacesTransformScale.Add(moneyGetPlaces[i].getPlaces.transform.localScale);
        }
        for (int j = 0; j < moneySpendPlaceToUnlockObjects.Count; j++)
        {
            moneySpendToUnlockObjectTransformPosition.Add(moneySpendPlaceToUnlockObjects[j].spendPlacesToUnlockObject.transform.position);
            moneySpendToUnlockObjectTransformRotation.Add(moneySpendPlaceToUnlockObjects[j].spendPlacesToUnlockObject.transform.rotation);
            moneySpendToUnlockObjectTransformScale.Add(moneySpendPlaceToUnlockObjects[j].spendPlacesToUnlockObject.transform.localScale);

            unlockableObjectPrefabTransformPosition.Add(moneySpendPlaceToUnlockObjects[j].prefab.transform.position);
            unlockableObjectPrefabTransformRotation.Add(moneySpendPlaceToUnlockObjects[j].prefab.transform.rotation);
            unlockableObjectPrefabTransformScale.Add(moneySpendPlaceToUnlockObjects[j].prefab.transform.localScale);
        }
        for (int k = 0; k < moneySpendPlaceToUnlockAreas.Count; k++)
        {
            moneySpendToUnlockAreaTransformPosition.Add(moneySpendPlaceToUnlockAreas[k].spendPlacesToUnlockArea.transform.position);
            moneySpendToUnlockAreaTransformRotation.Add(moneySpendPlaceToUnlockAreas[k].spendPlacesToUnlockArea.transform.rotation);
            moneySpendToUnlockAreaTransformScale.Add(moneySpendPlaceToUnlockAreas[k].spendPlacesToUnlockArea.transform.localScale);

            unlockableAreaPrefabTransformPosition.Add(moneySpendPlaceToUnlockAreas[k].prefab.transform.position);
            unlockableAreaPrefabTransformRotation.Add(moneySpendPlaceToUnlockAreas[k].prefab.transform.rotation);
            unlockableAreaPrefabTransformScale.Add(moneySpendPlaceToUnlockAreas[k].prefab.transform.localScale);

        }
    }
    public void AssignSpendPlaceToUnlockObjectData()
    {
        for (int i = 0; i < moneySpendPlaceToUnlockObjects.Count; i++)
        {
            SpendPlaceForUnlockObject spendPlace = moneySpendPlaceToUnlockObjects[i].spendPlacesToUnlockObject.GetComponent<SpendPlaceForUnlockObject>();
            MoneySpendPlaceToUnlockObject moneySpendPlaceToUnlockObject = moneySpendPlaceToUnlockObjects[i];
            moneySpendPlaceToUnlockObject.count = spendPlace.moneyAmount;

            moneySpendPlaceToUnlockObject.restaurantObjects = spendPlace.restaurantObjects;
            moneySpendPlaceToUnlockObjects[i] = moneySpendPlaceToUnlockObject;
        }

    }
    public void AssignSpendPlaceTounlockAreaData()
    {
        for (int i = 0; i < moneySpendPlaceToUnlockAreas.Count; i++)
        {
            SpendPlaceForUnlockArea spendPlace = moneySpendPlaceToUnlockAreas[i].spendPlacesToUnlockArea.GetComponent<SpendPlaceForUnlockArea>();
            MoneyspendToUnlockArea moneySpendPlaceToUnlockObject = moneySpendPlaceToUnlockAreas[i];
            moneySpendPlaceToUnlockObject.count = spendPlace.moneyAmount;

            moneySpendPlaceToUnlockObject.unloackableAreas = spendPlace.unloackableAreas;
            moneySpendPlaceToUnlockAreas[i] = moneySpendPlaceToUnlockObject;
        }
    }
}
[System.Serializable]
public struct MoneyGetPlace
{
    public GameObject getPlaces;    
}
[System.Serializable]
public struct MoneySpendPlaceToUnlockObject
{
    public GameObject spendPlacesToUnlockObject;
    public GameObject prefab;
    [HideInInspector] public int count;
    [HideInInspector] public RestaurantObjects restaurantObjects;
}
[System.Serializable]
public struct MoneyspendToUnlockArea
{
    public GameObject spendPlacesToUnlockArea;
    public GameObject prefab;
    [HideInInspector] public int count;
    [HideInInspector] public UnloackableAreas unloackableAreas;
}