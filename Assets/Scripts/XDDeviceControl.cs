using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class XDDeviceControl : MonoBehaviour
{
    //初始化，加载dll后调用一次
    [DllImport("XDinterface", CharSet = CharSet.Auto)]
    static extern bool InitInstance();

    //反初始化，卸载dll前调用一次
    [DllImport("XDinterface", CharSet = CharSet.Auto)]
    static extern bool ExitInstance();

    //游戏开始, 一局游戏开始后调用一次
    [DllImport("XDinterface", CharSet = CharSet.Auto)]
    static extern bool GameStart();

    //游戏结束，一局游戏结束后调用一次
    [DllImport("XDinterface", CharSet = CharSet.Auto)]
    static extern bool GameStop();





    //  - 实时控制设备运动，每50ms的间隔实时调用
    //  - 参数说明
    //    - Time：时间点，单位：秒，调用GameStart时为0
    //    - Pitch：前后倾斜角度，单位：弧度，取值范围：-18/180PI ~ +18/180PI，0为水平位置，值递增为从前往后倾斜，
    //    - Roll：左右倾斜角度，单位：弧度，取值范围：-18/180PI ~ +18/180PI，0为水平位置，值递增为从左往右倾斜
    //    - Yaw：水平旋转角度，单位：弧度，取值范围：-18/180PI ~ +18/180PI，0为正前方位置，值递增为从左往右旋转，6自由度平台或带旋转设备特有
    //    - Surge：前后位移距离，单位：毫米，取值范围：-100 ~ +100，0为初始位置,值递增为从后往前移动，6自由度平台特有
    //    - Sway：左右位移距离，单位：毫米，取值范围：-100 ~ +100，0为初始位置，值递增为从左往右移动，6自由度平台特有
    //    - Heave：上下位移距离，单位：毫米，取值范围：-100 ~ +100，0为初始位置，值递增为从下往上移动，3自由度、6自由度平台特有
    //    - ShakeType：震动类型，取值范围：0~6
    //      - 0：震动关闭，忽略Cycle和Amplitude参数
    //      - 1：前后倾斜方位震动，Pitch
    //      - 2：左右倾斜方位震动，Roll
    //      - 3：水平旋转方位震动，Yaw，不支持的设备会自动忽略
    //      - 4：前后位移方位震动，Surge，不支持的设备会自动转换成类型为1的震动
    //      - 5：左右位移方位震动，Sway，不支持的设备会自动转换成类型为2的震动
    //      - 6：上下位移方位震动，Heave，不支持的设备会自动转换成类型为2的震动
    //      - 震动时，会忽略相应方位的动感数据
    //    - Cycle：震动周期倍数，取值范围：1~4，乘以100ms后为从静止运动到最大幅度的时间
    //    - Amplitude：一个周期震动的幅度，ShakeType为1、2、3时，单位为弧度，ShakeType为4、5、6时，单位为毫米
    //  
    //- Speed：游戏内主角移动的前行速度，单位：米/秒，没有可以填0      
    //- 返回值
    //  - true：控制成功
    //  - false：控制失败
    [DllImport("XDinterface", CharSet = CharSet.Auto)]
    static extern bool MotionControl(float Time, float Pitch, float Roll, float Yaw, float Surge, float Sway, float Heave, float ShakeType, float Cycle, float Amplitude, float Speed);



    //- 环境特效的开关控制
    //- 参数
    //  - Status：开关
    //    - 1：开启
    //    - 0：关闭
    //  - EffectType：特效类型
    //    - 0： 风
    //    - 1： 雨
    //    - 2： 雪
    //    - 3： 电
    //    - 4： 气
    //    - 5： 泡
    //    - 6： 烟
    //    - 7： 扫腿
    //    - 8： 捶背
    //    - 9： 捶屁股
    //    - 10： 香水
    //    - 11： 火焰
    //    - 12： 鬼
    //- 返回值
    //  - true：特效控制成功
    //  - false：特效控制失败
    [DllImport("XDinterface", CharSet = CharSet.Auto)]
    static extern bool EffectControl(int EffectType, int Status);

    static public bool StartGame
    {
        get { return m_startGame; }
    }
    static private bool m_initInstance = false;
    static private bool m_startGame = false;

    private void Awake()
    {
        m_initInstance = InitInstance();
    }
    private void Start()
    {
        BeginStart();
    }
    static public void BeginStart()
    {
        m_startGame = GameStart();
    }
    static public void StopGame()
    {
        GameStop();
    }
    static public void SendMotionControl(float time, float Pitch, float Roll, float Yaw, float Surge, float Sway, float Heave, float ShakeType, float Cycle, float Amplitude, float Speed)
    {
        if (m_startGame)
            MotionControl(time, -Pitch, Roll, Yaw, Surge, Sway, Heave, ShakeType, Cycle, Amplitude, Speed);
    }
    static public void SendEffectControl(int EffectType, int Status)
    {        
        if (m_startGame)
            EffectControl(EffectType, Status);
    }

    private void OnApplicationQuit()
    {
        ExitInstance();
    }

}