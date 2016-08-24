using UnityEngine;
using System.Collections;

public class TouchArea : MonoBehaviour {
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
        }
    }

    Vector2 m_PrePos;
    int m_TouchId = -4;

    void Awake()
    {
        UIEventListener.Get(gameObject).onPress = OnBtnPress;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Pushed && m_TouchId > -4)
        {
            Vector2 position = (m_TouchId >= 0 && Input.touchCount > m_TouchId) ? Input.GetTouch(m_TouchId).position : new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            AxisX = position.x - m_PrePos.x;
            AxisY = position.y - m_PrePos.y;

            m_PrePos = position;
        }
    }

    void OnBtnPress(GameObject go, bool state)
    {
        Pushed = state;
        if(Pushed)
        {
            m_TouchId = UICamera.currentTouchID;
            if(m_TouchId >=0 && Input.touchCount > m_TouchId)
            {
                m_PrePos = Input.GetTouch(m_TouchId).position;
            }
            else
            {
                m_PrePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
        else
        {
            m_TouchId = -4;
            AxisX = 0;
            AxisY = 0;
        }
    }
}
