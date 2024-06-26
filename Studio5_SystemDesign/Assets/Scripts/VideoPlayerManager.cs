using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private float lastResetTime;
    private InputHandler inputHandler;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.isLooping = false; // 确保视频不会循环播放
        videoPlayer.loopPointReached += OnVideoEnded; // 添加视频结束事件的监听器
        inputHandler = InputHandler.Instance;
        lastResetTime = -2f; // Initialize to a value that allows immediate reset
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(videoPlayer == null ? "videoPlayer is null" : "videoPlayer is not null");
        Debug.Log(inputHandler == null ? "inputHandler is null" : "inputHandler is not null");

        if (inputHandler != null && inputHandler.GetInputData().Menu == 1 && Time.time - lastResetTime > 2)
        {
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
                videoPlayer.Play();
                lastResetTime = Time.time;
            }
            else
            {
                Debug.Log("Cannot play video because videoPlayer is null");
            }
        }
    }

    // 当视频播放结束时调用
    void OnVideoEnded(VideoPlayer vp)
    {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}