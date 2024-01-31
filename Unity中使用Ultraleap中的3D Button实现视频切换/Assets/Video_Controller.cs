using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
 
public class Video_Controller : MonoBehaviour {
    private VideoPlayer videoplayer;
    private RawImage rawImage;
    private int currentClipIndex;
 
    public Text text_playorpause;
    public Button button_playorpause;
    public Button button_pre;
    public Button button_next;
    public VideoClip[] videoClips;//定义了一个数组
	// Use this for initialization
	void Start () {
        videoplayer = this.GetComponent<VideoPlayer>();//获取组件
        rawImage = this.GetComponent<RawImage>();
        currentClipIndex = 0;
        button_playorpause.onClick.AddListener(OnplayorpauseVideo);
        button_pre.onClick.AddListener(OnpreVideo);
        button_next.onClick.AddListener(OnnextVideo);//调用方法
	}
	
	// Update is called once per frame
	void Update () {
        if (videoplayer.texture == null)
        {
            return;
        }
            rawImage.texture = videoplayer.texture;
        
        
		
	}
    private void OnplayorpauseVideo() {
        if (videoplayer.enabled == true)
       {
            if (videoplayer.isPlaying) { 
                videoplayer.Pause();
            text_playorpause.text = "播放";
            Debug.Log("2322");//用于调试脚本不能正常运行
 
           }
          else if (!videoplayer.isPlaying)
          {
            videoplayer.Play();
            Debug.Log("111");
            text_playorpause.text = "暂停";
          }
        }
    }
    private void OnpreVideo() {
        currentClipIndex -= 1;
        if (currentClipIndex < 0) {
            currentClipIndex = videoClips.Length - 1;
        }
        videoplayer.clip = videoClips[currentClipIndex];
        text_playorpause.text = "暂停";
        }
    private void OnnextVideo()
    {
        currentClipIndex += 1;
        currentClipIndex = currentClipIndex % videoClips.Length;
        videoplayer.clip = videoClips[currentClipIndex];
        text_playorpause.text = "暂停";
    }
    }
    
 