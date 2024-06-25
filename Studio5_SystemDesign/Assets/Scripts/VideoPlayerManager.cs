using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private float lastResetTime;
    private InputHandler inputHandler;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.isLooping = true;
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
}