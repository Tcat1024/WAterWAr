using UnityEngine;
using System.Collections;

public class LowBody : MonoBehaviour {
    public Transform Body;

    public Transform LeftLeg;
    public Transform LeftUpLeg;
    public Transform LeftLowLeg;

    public Transform RightLeg;
    public Transform RightUpLeg;
    public Transform RightLowLeg;

    public float RunActionDuration = 1f;
    public float Smooth = 10f;

    private float m_AniDuration;
    private PlayerAction m_CurAction = PlayerAction.ACTION_STAND;

    private delegate void ActionInitFunc(LowBody player);
    private ActionInitFunc[] m_ActionInitFuncs = new ActionInitFunc[(int)PlayerAction.ACTION_NUM]{
        InitStandAction,
        InitRunAction,
        null,
        InitJumpAction,
        null,
    };

    private delegate void ActionUpdateFunc(LowBody player);
    private ActionUpdateFunc[] m_ActionUpdateFuncs = new ActionUpdateFunc[(int)PlayerAction.ACTION_NUM]{
        null,
        UpdateRunAction,
        null,
        null,
        null,
    };

    private Quaternion m_LeftLegRoate;
    private Quaternion m_RightLegRoate;
    private Quaternion m_LeftLowLegRoate;
    private Quaternion m_RightLowLegRoate;

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

        LeftLeg.localRotation = Quaternion.Slerp(LeftLeg.localRotation, m_LeftLegRoate, Time.deltaTime * Smooth);
        RightLeg.localRotation = Quaternion.Slerp(RightLeg.localRotation, m_RightLegRoate, Time.deltaTime * Smooth);
        LeftLowLeg.localRotation = Quaternion.Slerp(LeftLowLeg.localRotation, m_LeftLowLegRoate, Time.deltaTime * Smooth);
        RightLowLeg.localRotation = Quaternion.Slerp(RightLowLeg.localRotation, m_RightLowLegRoate, Time.deltaTime * Smooth);
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

    static void InitStandAction(LowBody body)
    {
        body.m_LeftLegRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightLegRoate = Quaternion.Euler(0, 0, 0);
        body.m_LeftLowLegRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightLowLegRoate = Quaternion.Euler(0, 0, 0);
    }

    static void InitRunAction(LowBody body)
    {
        UpdateRunAction(body);
    }

    static void UpdateRunAction(LowBody body)
    {
        float fPer = (body.m_AniDuration - (int)(body.m_AniDuration / body.RunActionDuration) * body.RunActionDuration) / body.RunActionDuration;
        fPer = Mathf.Sin(fPer * 2 * Mathf.PI);
        float fLLD = 40f * fPer;
        body.m_LeftLegRoate = Quaternion.Euler(fLLD, 0, 0);
        body.m_RightLegRoate = Quaternion.Euler(-fLLD, 0, 0);
        body.m_LeftLowLegRoate = Quaternion.Euler(0, 0, 0);
        body.m_RightLowLegRoate = Quaternion.Euler(0, 0, 0);
    }

    static void InitJumpAction(LowBody body)
    {
        body.m_LeftLegRoate = Quaternion.Euler(-75, -20, 0);
        body.m_RightLegRoate = Quaternion.Euler(-75, 20, 0);
        body.m_LeftLowLegRoate = Quaternion.Euler(120, 0, 0);
        body.m_RightLowLegRoate = Quaternion.Euler(120, 0, 0);
    }
}
