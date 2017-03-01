using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageDirector : MonoBehaviour
{
    // Control options.
    public bool ignoreFastForward = true;

    // Prefabs.
    public GameObject musicPlayerPrefab;
    public GameObject mainCameraRigPrefab;
    public GameObject[] prefabsNeedsActivation;
    public GameObject[] prefabsOnTimeline;
    public GameObject[] miscPrefabs;

    // Camera points.
    public Transform[] cameraPoints;

    // Exposed to animator.
    public float overlayIntensity = 1.0f;

    // Objects to be controlled.
    GameObject musicPlayer;
    CameraSwitcher mainCameraSwitcher;
    ScreenOverlay[] screenOverlays;
    GameObject[] objectsNeedsActivation;
    GameObject[] objectsOnTimeline;

    public Transform Father;
    private Transform RockStar = null;
    private bool ifcheck = true;
    void Awake()
    {
        // Instantiate the prefabs.

        var cameraRig = (GameObject)Instantiate(mainCameraRigPrefab);
        if (SceneManager.GetActiveScene().name == "Scene1")
            mainCameraSwitcher = cameraRig.GetComponentInChildren<CameraSwitcher>();
        screenOverlays = cameraRig.GetComponentsInChildren<ScreenOverlay>();

       

        objectsNeedsActivation = new GameObject[prefabsNeedsActivation.Length];
        for (var i = 0; i < prefabsNeedsActivation.Length; i++)
            objectsNeedsActivation[i] = (GameObject)Instantiate(prefabsNeedsActivation[i]);

        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            objectsOnTimeline = new GameObject[prefabsOnTimeline.Length];
            for (var i = 0; i < prefabsOnTimeline.Length; i++)
                objectsOnTimeline[i] = (GameObject)Instantiate(prefabsOnTimeline[i]);
        }

        foreach (var p in miscPrefabs) Instantiate(p);

        if (SceneManager.GetActiveScene().name == "Scene1")
            musicPlayer = (GameObject)Instantiate(musicPlayerPrefab);

        //Father = GameObject.FindGameObjectWithTag("ModelFather").transform;
        //RockStar = TransformHelper.FindChild(Father, "CandyRockStar");

        //Father.GetComponentInChildren<playmusic>().
        //musicHandler += StartMusic;
        // musicPlayer = (GameObject)Instantiate(musicPlayerPrefab);
    }

    void Update()
    {
        foreach (var so in screenOverlays)
        {
            so.intensity = overlayIntensity;
            so.enabled = overlayIntensity > 0.01f;
        }
        //if (SceneManager.GetActiveScene().name == "Scene2"&& ifcheck == true)
           // Check();
    }

    
    //private void Check()
    //{
    //   //print(ifcheck);
      
            
    //        if (RockStar.gameObject.activeInHierarchy == true)
    //        {
    //            musicPlayer = (GameObject)Instantiate(musicPlayerPrefab);
    //            StartMusic();
    //            ifcheck = false;

    //        }
        
    //}
    public void StartMusic()
    {
        foreach (var source in musicPlayer.GetComponentsInChildren<AudioSource>())
            source.Play();
    }

    public void ActivateProps()
    {
        foreach (var o in objectsNeedsActivation) o.BroadcastMessage("ActivateProps");
    }

    public void SwitchCamera(int index)
    {
        if (mainCameraSwitcher)
            mainCameraSwitcher.ChangePosition(cameraPoints[index], true);
    }

    public void StartAutoCameraChange()
    {
        if (mainCameraSwitcher)
            mainCameraSwitcher.StartAutoChange();
    }

    public void StopAutoCameraChange()
    {
        if (mainCameraSwitcher)
            mainCameraSwitcher.StopAutoChange();
    }

    public void FastForward(float second)
    {
        if (!ignoreFastForward)
        {
            FastForwardAnimator(GetComponent<Animator>(), second, 0);
            foreach (var go in objectsOnTimeline)
                foreach (var animator in go.GetComponentsInChildren<Animator>())
                    FastForwardAnimator(animator, second, 0.5f);
        }
    }

    void FastForwardAnimator(Animator animator, float second, float crossfade)
    {
        for (var layer = 0; layer < animator.layerCount; layer++)
        {
            var info = animator.GetCurrentAnimatorStateInfo(layer);
            if (crossfade > 0.0f)
                animator.CrossFade(info.nameHash, crossfade / info.length, layer, info.normalizedTime + second / info.length);
            else
                animator.Play(info.nameHash, layer, info.normalizedTime + second / info.length);
        }
    }

    public void EndPerformance()
    {
        Application.LoadLevel(0);
    }
}
