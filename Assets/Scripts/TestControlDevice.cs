using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestControlDevice : MonoBehaviour
{
    private float gametime, Pitch, Roll, Yaw, Surge, Sway, Heave, ShakeType, Cycle, Amplitude, Speed;

    private Vector3 m_OldPos;
    private int m_DeviceFXType;

    void Start()
    {
        m_OldPos = transform.position;
        ShakeType = 0;
        m_DeviceFXType = 0;
        Cycle = 0.2f;
        Amplitude = 0.8f;
        InvokeRepeating("ChangeDeviceShakeType", 3, 3);

        InvokeRepeating("ChangeDeviceFX", 2, 2);
    }

    void FixedUpdate()
    {
        if (XDDeviceControl.StartGame)
        {
            Pitch = ConvertValue(transform.localEulerAngles.x) * Mathf.Deg2Rad;
            Roll = ConvertValue(transform.localEulerAngles.z) * Mathf.Deg2Rad;
            Yaw = ConvertValue(transform.localEulerAngles.y) * Mathf.Deg2Rad;

            Surge = transform.localPosition.z;
            Sway = transform.localPosition.x;
            Heave = transform.localPosition.y;

            gametime += Time.deltaTime;
            Speed = (transform.position - m_OldPos).magnitude / Time.deltaTime;
            m_OldPos = transform.position;
            XDDeviceControl.SendMotionControl(gametime, Pitch, Roll, Yaw, Surge, Sway, Heave, ShakeType, Cycle, Amplitude, Speed);
        }
    }

    private float ConvertValue(float inFloat)
    {
        inFloat = inFloat > 90 ? inFloat - 360 : inFloat;
        inFloat = inFloat < -90 ? inFloat + 360 : inFloat;

        return inFloat;
    }

    void ChangeDeviceShakeType()
    {
        ShakeType++;
        ShakeType = ShakeType % 7;
    }

    public void ChangeDeviceFX()
    {
        XDDeviceControl.SendEffectControl(m_DeviceFXType, 0);
        m_DeviceFXType++;
        m_DeviceFXType = m_DeviceFXType % 13;
        XDDeviceControl.SendEffectControl(m_DeviceFXType, 1);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Pitch:" + Pitch);
        GUILayout.Label("Roll:" + Roll);
        GUILayout.Label("Yaw:" + Yaw);
        GUILayout.Label("Surge:" + Surge);
        GUILayout.Label("Sway:" + Sway);
        GUILayout.Label("Heave:" + Heave);
        GUILayout.Label("ShakeType:" + ShakeType);
        GUILayout.Label("FXType:" + m_DeviceFXType);
        GUILayout.EndVertical();
    }
}
