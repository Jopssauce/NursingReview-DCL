using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;

    private string currentMainScene;
    private string sceneToLoad;

    AsyncOperation unloadOperation;
    AsyncOperation loadOperation;

    private void Awake()
    {
        instance = this;
        currentMainScene = null;
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
        return SceneManager.UnloadSceneAsync(sceneName);
    }

    public void ReplaceActiveScene(string sceneToLoad)
    {
        LoadSceneAdditive(sceneToLoad);
        this.sceneToLoad = sceneToLoad;
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
        obj.completed -= SetActiveOperation;
    }
}
