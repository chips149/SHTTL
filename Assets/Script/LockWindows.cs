using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWindows : MonoBehaviour
{
    void Start()
    {
        // 固定目标宽高，和PlayerSettings保持一致
        int fixW = 1080;
        int fixH = 1920;
        
        // 强制窗口模式 + 固定分辨率，不再弹出选择框
        Screen.SetResolution(fixW, fixH, FullScreenMode.Windowed);
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}
