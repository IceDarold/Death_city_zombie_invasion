//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
using DataCenter;
using UnityEngine;
using Zombie3D;

public class GameScript : MonoBehaviour
{
    protected float deltaTime;

    protected int frames;

    protected float lastFrameTime;

    protected float accum;

    private void Start()
    {
        this.lastFrameTime = Time.realtimeSinceStartup;
    }

    public void DoLoadScene(string sceneName)
    {
        CheckpointData selectCheckpoint = CheckpointDataManager.SelectCheckpoint;
        if (selectCheckpoint.SceneID == -1 && selectCheckpoint.ID == -1)
        {
            Singleton<FadeAnimationScript>.Instance.SetColor(Color.black, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            GameApp.GetInstance().Init();
            GameApp.GetInstance().CreateScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else
        {
            Singleton<FadeAnimationScript>.Instance.SetColor(Color.black, Singleton<GlobalData>.Instance.GetText(selectCheckpoint.Mission));
            GameApp.GetInstance().Init();
            GameApp.GetInstance().CreateScene(sceneName);
        }
    }

    private void Update()
    {
        GameApp.GetInstance().Loop(Time.deltaTime);
    }

    public void OnDestroy()
    {
        GameApp.GetInstance().ReleaseGameScene();
    }
}


