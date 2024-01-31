using UnityEngine;

public class ButtonPressHandler : MonoBehaviour
{
    // 由On Press()事件触发的函数
    public void OnButtonPress()
    {
        // 打开百度网页
        Application.OpenURL("https://www.baidu.com");
    }
}
