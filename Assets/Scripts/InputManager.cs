using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    [HideInInspector]
    static public InputManager Instance;

    public Joystick MoveJoy;
    
    public float MoveAxisX
    {
        get
        {
            return MoveJoy.AxisX;
        }
    }
    public float MoveAxisY
    {
        get
        {
            return MoveJoy.AxisY;
        }
    }

    public PressButton JumpBtn;
    public bool Jump
    {
        get
        {
            return JumpBtn.Pressed;
        }
    }

    public TouchArea ViewArea;
    public float ViewAxisX
    {
        get
        {
            return ViewArea.AxisX;
        }
    }
    public float ViewAxisY
    {
        get
        {
            return ViewArea.AxisY;
        }
    }

    public bool ViewAxisPushed
    {
        get
        {
            return ViewArea.Pushed;
        }
    }

    public Joystick FireJoy;
    public float FireAxisX
    {
        get
        {
            return FireJoy.AxisX;
        }
    }

    public float FireAxisY
    {
        get
        {
            return FireJoy.AxisY;
        }
    }

    public bool FireAxisPushed
    {
        get
        {
            return FireJoy.Pushed;
        }
    }

    void Awake()
    {
        Instance = this;
    }
}
