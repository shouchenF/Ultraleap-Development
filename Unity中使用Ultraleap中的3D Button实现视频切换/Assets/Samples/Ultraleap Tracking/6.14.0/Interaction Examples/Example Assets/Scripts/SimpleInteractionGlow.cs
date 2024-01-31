/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2024.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using Leap.Unity;
using Leap.Unity.Interaction;
using UnityEngine;
using System.Diagnostics; // 引入System.Diagnostics命名空间

using System.Collections;
using System.Collections.Generic;
// using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Leap.Unity.InteractionEngine.Examples
{
    /// <summary>
    /// This simple script changes the color of an InteractionBehaviour as
    /// a function of its distance to the palm of the closest hand that is
    /// hovering nearby.
    /// </summary>
    [AddComponentMenu("")]
    [RequireComponent(typeof(InteractionBehaviour))]
    public class SimpleInteractionGlow : MonoBehaviour
    {
        private VideoPlayer videoplayer;
        private RawImage rawImage;
        private int currentClipIndex;

        public VideoClip[] videoClips;//定义了一个数组
        
        [Tooltip("If enabled, the object will lerp to its hoverColor when a hand is nearby.")]
        public bool useHover = true;

        [Tooltip("If enabled, the object will use its primaryHoverColor when the primary hover of an InteractionHand.")]
        public bool usePrimaryHover = false;

        [Header("InteractionBehaviour Colors")]
        public Color defaultColor = Color.Lerp(Color.black, Color.white, 0.1F);

        public Color suspendedColor = Color.red;
        public Color hoverColor = Color.Lerp(Color.black, Color.white, 0.7F);
        public Color primaryHoverColor = Color.Lerp(Color.black, Color.white, 0.8F);

        [Header("InteractionButton Colors")]
        [Tooltip("This color only applies if the object is an InteractionButton or InteractionSlider.")]
        public Color pressedColor = Color.white;

        private Material[] _materials;

        private InteractionBehaviour _intObj;

        [SerializeField]
        private Rend[] rends;

        [System.Serializable]
        public class Rend
        {
            public int materialID = 0;
            public Renderer renderer;
        }

        public void Press()
        {
            //     // 功能一：输出信息，可在控制台【Console】查看
            //    // Debug.Log("按下");
            //     // 功能二：打开百度网页
            //     Application.OpenURL("https://www.baidu.com");
            //     // 功能三：使用Process.Start()函数打开指定程序
            //     string filePath = @"C:\Program Files (x86)\Thunder Network\Thunder\Program"; // 要打开的程序路径
            //     // 对文件路径中的反斜杠进行转义
            //     string escapedPath = filePath.Replace("\\", "\\\\");
           // Process.Start(escapedPath);
            UnityEngine.Debug.Log("按下");
          if (videoplayer.isPlaying)
            { 
                UnityEngine.Debug.Log("Pause");
                videoplayer.Pause();
            }
          else if (!videoplayer.isPlaying)
            {
                UnityEngine.Debug.Log("Play");
                videoplayer.Play();
            }
        }

        public void Unpress()
        {
           UnityEngine.Debug.Log("抬起");
        }

        public void OnpreVideo() {
            currentClipIndex -= 1;
            if (currentClipIndex < 0) {
                currentClipIndex = videoClips.Length - 1;
            }
            videoplayer.clip = videoClips[currentClipIndex];
    
        }

        public void OnnextVideo()
        {
            currentClipIndex += 1;
            currentClipIndex = currentClipIndex % videoClips.Length;
            videoplayer.clip = videoClips[currentClipIndex];
            
        }

        public void OpenExe()
        {
            string filePath = @"C:\Program Files\Ultraleap\TouchFree\TouchFree\TouchFree.exe"; // 要打开的程序路径
            // 对文件路径中的反斜杠进行转义
            string escapedPath = filePath.Replace("\\", "\\\\");
            Process.Start(escapedPath);

            string ApplicationfilePath = @"F:\嵌入式项目\衍视科技\资料\资料\ultraleap-touchfree-bakery-1-0-0-cursorless\Ultraleap TouchFreeBakery 1-0-0 - Cursorless\TouchFreeBakery.exe";
            string ApplicationPath = ApplicationfilePath.Replace("\\", "\\\\");
            Process.Start(ApplicationPath);
        }

        void Start()
        {
            _intObj = GetComponent<InteractionBehaviour>();
            UnityEngine.Debug.Log("_intObj的值是：" + _intObj);

            videoplayer = FindObjectOfType<VideoPlayer>();//获取组件
            UnityEngine.Debug.Log("videoplayer的值是：" + videoplayer);

            rawImage = FindObjectOfType<RawImage>();
            UnityEngine.Debug.Log("rawImage的值是：" + rawImage);

            currentClipIndex = 0;

            if (rends.Length > 0)
            {
                _materials = new Material[rends.Length];

                for (int i = 0; i < rends.Length; i++)
                {
                    _materials[i] = rends[i].renderer.materials[rends[i].materialID];
                }
            }
        }

        void Update()
        {
            if (_materials != null)
            {

                // The target color for the Interaction object will be determined by various simple state checks.
                Color targetColor = defaultColor;

                // "Primary hover" is a special kind of hover state that an InteractionBehaviour can
                // only have if an InteractionHand's thumb, index, or middle finger is closer to it
                // than any other interaction object.
                if (_intObj.isPrimaryHovered && usePrimaryHover)
                {
                    targetColor = primaryHoverColor;
                }
                else
                {
                    // Of course, any number of objects can be hovered by any number of InteractionHands.
                    // InteractionBehaviour provides an API for accessing various interaction-related
                    // state information such as the closest hand that is hovering nearby, if the object
                    // is hovered at all.
                    if (_intObj.isHovered && useHover)
                    {
                        float glow = _intObj.closestHoveringControllerDistance.Map(0F, 0.2F, 1F, 0.0F);
                        targetColor = Color.Lerp(defaultColor, hoverColor, glow);
                    }
                }

                if (_intObj.isSuspended)
                {
                    // If the object is held by only one hand and that holding hand stops tracking, the
                    // object is "suspended." InteractionBehaviour provides suspension callbacks if you'd
                    // like the object to, for example, disappear, when the object is suspended.
                    // Alternatively you can check "isSuspended" at any time.
                    targetColor = suspendedColor;
                }

                // We can also check the depressed-or-not-depressed state of InteractionButton objects
                // and assign them a unique color in that case.
                if (_intObj is InteractionButton && (_intObj as InteractionButton).isPressed)
                {
                    targetColor = pressedColor;
                }

                // Lerp actual material color to the target color.
                for (int i = 0; i < _materials.Length; i++)
                {
                    _materials[i].color = Color.Lerp(_materials[i].color, targetColor, 30F * Time.deltaTime);
                }
            }
        }

    }
}