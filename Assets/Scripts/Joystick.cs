using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour {
    private float m_Radius2 = 0f;
    public float Radius;

    public GameObject Handle;
    public UISprite NormalSprite;
    public UISprite PushedSprite;
    public string AxisXName;
    public string AxisYName;

    private float m_AxisX = 0f;
    public float AxisX
    {
        get
        {
            return m_AxisX;
        }
        private set
        {
            m_AxisX = value;
        }
    }
    private float m_AxisY = 0f;
    public float AxisY
    {
        get
        {
            return m_AxisY;
        }
        private set
        {
            m_AxisY = value;
        }
    }

    private bool m_Pushed = false;
    public bool Pushed
    {
        get
        {
            return m_Pushed;
        }
        private set
        {
            m_Pushed = value;
            NormalSprite.enabled = !value;
            PushedSprite.enabled = value;
        }
    }

    Vector3 m_StartPos;
    Vector2 m_DeltaPos;
    Vector2 m_TruePos;

    void Awake()
    {
        m_Radius2 = Radius * Radius;
        UIEventListener.Get(Handle).onDrag = OnBtnDrag;
        UIEventListener.Get(Handle).onDragEnd = OnBtnDragEnd;
        UIEventListener.Get(Handle).onPress = OnBtnPress;
        m_StartPos = Handle.transform.localPosition;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(AxisXName.Length > 0 && AxisYName.Length > 0 && !Pushed)
        {
            if(Radius > 0)
            {
                m_DeltaPos = new Vector2(0, 0);
                m_TruePos = new Vector2(Input.GetAxis(AxisXName) * Radius, Input.GetAxis(AxisYName) * Radius);
            }
            else
            {
                m_DeltaPos = new Vector2(Input.GetAxis(AxisXName), Input.GetAxis(AxisYName)) - m_TruePos;
            }
            UpdateAxis();
        }
        if(Pushed)
            UpdateAxis();
    }

    void OnBtnPress(GameObject go, bool state)
    {
        Pushed = state;
    }
    
    void OnBtnDrag(GameObject tarObj, Vector2 delta)
    {
        m_DeltaPos += delta;
    }

    void UpdateAxis()
    {
        Vector2 handlePos = m_StartPos;
        m_TruePos += m_DeltaPos;
        if (Radius > 0)
        {
            if (m_TruePos.sqrMagnitude > m_Radius2)
            {
                handlePos = m_TruePos.normalized * Radius;
            }
            else
            {
                handlePos = m_TruePos;
            }

            m_AxisX = handlePos.x / Radius;
            m_AxisY = handlePos.y / Radius;
        }
        else
        {
            m_AxisX = m_DeltaPos.x;
            m_AxisY = m_DeltaPos.y;
            handlePos = m_TruePos;
        }

        Handle.transform.localPosition = new Vector3(handlePos.x, handlePos.y, m_StartPos.z);
        m_DeltaPos = new Vector2(0f, 0f);
    }

    void OnBtnDragEnd(GameObject tarObj)
    {
        m_AxisX = 0;
        m_AxisY = 0;
        Handle.transform.localPosition = m_StartPos;
        m_TruePos = new Vector2(0f, 0f);
        m_DeltaPos = new Vector2(0f, 0f);
    }
}
