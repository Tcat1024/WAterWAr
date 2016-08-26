using UnityEngine;
using System.Collections;

public class UpBody : MonoBehaviour {
    public Transform Body;
    public Transform LeftArm;
    public Transform LeftUpArm;
    public Transform LeftLowArm;
    public Transform RightArm;
    public Transform RightUpArm;
    public Transform RightLowArm;
    public Transform Head;

    public float RunActionDuration = 1f;
    public float Smooth = 10f;

    private float m_AniDuration;
    private PlayerAction m_CurAction = PlayerAction.ACTION_STAND;

    private delegate void ActionInitFunc(UpBody player);
    private ActionInitFunc[] m_ActionInitFuncs = new ActionInitFunc[(int)PlayerAction.ACTION_NUM]{
        InitStandAction,
        InitRunAction,
        null,
        InitJumpAction,
        InitFireAction,
    };

    private delegate void ActionUpdateFunc(UpBody player);
    private ActionUpdateFunc[] m_ActionUpdateFuncs = new ActionUpdateFunc[(int)PlayerAction.ACTION_NUM]{
        null,
        UpdateRunAction,
        null,
        null,
        null
    };

    private Quaternion m_LeftArmRoate;
    private Quaternion m_RightArmRoate;
    private Quaternion m_LeftLowArmRoate;
    private Quaternion m_RightLowArmRoate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_AniDuration += Time.deltaTime;
        if (m_ActionUpdateFuncs[(int)m_CurAction] != null)
        {
            m_ActionUpdateFuncs[(int)m_CurAction](this);
        }

        LeftArm.localRotation = Quaternion.Slerp(LeftArm.localRotation, m_LeftArmRoate, Smooth * Time.deltaTime);
        RightArm.localRotation = Quaternion.Slerp(RightArm.localRotation, m_RightArmRoate, Smooth * Time.deltaTime);
        LeftLowArm.localRotation = Quaternion.Slerp(LeftLowArm.localRotation, m_LeftLowArmRoate, Smooth * Time.deltaTime);
        RightLowArm.localRotation = Quaternion.Slerp(RightLowArm.localRotation, m_RightLowArmRoate, Smooth * Time.deltaTime);
    }

    public void PlayAction(PlayerAction tarAction)
    {
        if (tarAction > 0 && tarAction == m_CurAction)
            return;
        m_AniDuration = 0f;
        m_CurAction = tarAction;
        if (m_ActionInitFuncs[(int)m_CurAction] != null)
        {
            m_ActionInitFuncs[(int)m_CurAction](this);
        }
    }

    static void InitStandAction(UpBody body)
    {
        body.m_LeftArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_LeftLowArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightLowArmRoate = Quaternion.Euler(0, 0, 0);
    }

    static void InitRunAction(UpBody body)
    {
        body.m_LeftLowArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightLowArmRoate = Quaternion.Euler(0, 0, 0);
        UpdateRunAction(body);
    }

    static void UpdateRunAction(UpBody body)
    {
        float fPer = (body.m_AniDuration - (int)(body.m_AniDuration / body.RunActionDuration) * body.RunActionDuration) / body.RunActionDuration;
        fPer = Mathf.Sin(fPer * 2 * Mathf.PI);
        float fRAD = 35f * fPer;
        body.m_RightArmRoate = Quaternion.Euler(fRAD, 0, 0);
        body.m_LeftArmRoate = Quaternion.Euler(-fRAD, 0, 0);
    }

    static void InitJumpAction(UpBody body)
    {
        body.m_LeftArmRoate = Quaternion.Euler(0, 75, -80);
        body.m_RightArmRoate = Quaternion.Euler(30, 0, 30);
        body.m_LeftLowArmRoate = Quaternion.Euler(-105, 0, 0);
        body.m_RightLowArmRoate = Quaternion.Euler(-115, 0, 0);
    }


    static void InitFireAction(UpBody body)
    {
        body.m_LeftLowArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightLowArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_LeftArmRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightArmRoate = Quaternion.Euler(0, 0, 0);
    }
}
