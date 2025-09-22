using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;
    public bool LoadStartScene;
    public string StartScene;

    private string currentMainScene;
    private string sceneToLoad;
    private string lastScene;

    AsyncOperation unloadOperation;
    AsyncOperation loadOperation;

    // Im too lazy to put this anywhere else
    public DataSubTopic topic;

    private void Awake()
    {
        instance = this;
        currentMainScene = null;
    }

    private void Start()
    {
        if (LoadStartScene == true)
        {
            LoadActiveAdditive(StartScene);
            lastScene = StartScene;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().name == "Persistent Scene Manager" || SceneManager.GetActiveScene().name == "Body Systems UI" || SceneManager.GetActiveScene().name == "Patients UI")
            {
                //Application.Quit();
                return;
            }
            ReplaceActiveScene(lastScene);
        }
    }

    public AsyncOperation LoadActiveAdditive(string sceneName)
    {
        loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        sceneToLoad = sceneName;
        loadOperation.completed += SetActiveOperation;
        return loadOperation;
    }

    public AsyncOperation LoadSceneAdditive(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public AsyncOperation UnloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Debug.Assert(SceneManager.GetSceneByName(sceneName).isLoaded, "Scene Not Loaded");
            return SceneManager.UnloadSceneAsync(sceneName);
        }
        else
        {
            return null;
        }
    }

    public void ReplaceActiveScene(string sceneToLoad)
    {
        if (unloadOperation != null) return;
        LoadSceneAdditive(sceneToLoad);
        this.sceneToLoad = sceneToLoad;
        lastScene = SceneManager.GetActiveScene().name;
        unloadOperation = UnloadScene(SceneManager.GetActiveScene().name);

        unloadOperation.completed += SetActiveOperation;
    }

    private void SetActive(string sceneName)
    {
        currentMainScene = sceneName;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
    }

    private void SetActiveOperation(AsyncOperation obj)
    {
        SetActive(sceneToLoad);
        if(unloadOperation != null)
        {
            unloadOperation.completed -= SetActiveOperation;
            unloadOperation = null;
        }
        
    }
}
