using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndDetector : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoScene;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.UnloadSceneAsync(videoScene);
    }

    public void SkipVideo()
    {
        SceneManager.UnloadSceneAsync(videoScene);
    }
}
