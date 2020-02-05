using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Canvas Canvas;
    public Vector2 ReferenceResolution = new Vector2(1920, 1080);

    PersistentSceneManager PersistentSceneManager;

    private void Awake()
    {
        CanvasScaler canvasScaler = Canvas.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = ReferenceResolution;
    }

    private void Start()
    {
        Initialize();
    }
    
    public virtual void Initialize()
    {
        Debug.Assert(Canvas != null, "No Canvas Detected!");
        PersistentSceneManager = PersistentSceneManager.instance;
    }

    public void LoadScene(string sceneToLoad)
    {
        PersistentSceneManager.LoadSceneAdditive(sceneToLoad);
    }

    public void LoadActiveScene(string sceneToLoad)
    {
        PersistentSceneManager.ReplaceActiveScene(sceneToLoad);
    }
}
