using UnityEngine;
using System.Collections;

public class PressButton : MonoBehaviour {

    public UISprite NormalSprite;
    public UISprite PushedSprite;
    public string ButtonName;

    private bool m_Pressed = false;
    public bool Pressed
    {
        get
        {
            return m_Pressed;
        }
        private set
        {
            m_Pressed = value;
            NormalSprite.enabled = !value;
            PushedSprite.enabled = value;
        }
    }
    private bool m_UseInputButton = false;


    void Awake()
    {
        Pressed = false;
        UIEventListener.Get(gameObject).onPress = OnBtnPress;
        m_UseInputButton = ButtonName.Length != 0;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(m_UseInputButton)
        {
            if(Input.GetButtonDown(ButtonName))
            {
                Pressed = true;
            }
            else if(Input.GetButtonUp(ButtonName))
            {
                Pressed = false;
            }
        }
	}

    void OnBtnPress(GameObject go, bool state)
    {
        Pressed = state;
    }

}
