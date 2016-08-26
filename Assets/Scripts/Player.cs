using UnityEngine;
using System.Collections;

public enum PlayerAction
{
    ACTION_STAND = 0,
    ACTION_RUN,
    ACTION_WALK,
    ACTION_JUMP,
    ACTION_FIRE,

    ACTION_NUM
}

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    public enum State
    {
        STATE_NORMAL,
        STATE_FIRING,
    }

    public UpBody BodyUp;
    public LowBody BodyLow;
    private State m_CurState = State.STATE_NORMAL;
    public State CurState
    {
        get
        {
            return m_CurState;
        }
        set
        {
            SetState(value);
        }
    }

    private PlayerAction m_CurAction = PlayerAction.ACTION_STAND;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }

    void SetState(State tarState)
    {
        if (m_CurState == tarState)
            return;
        switch (m_CurState)
        {
            case State.STATE_NORMAL:
                break;
            case State.STATE_FIRING:
                BodyUp.PlayAction(m_CurAction);
                break;
            default:
                return;
        }

        m_CurState = tarState;
    }

    public void PlayAction(PlayerAction tarAction)
    {
        switch (m_CurState)
        {
            case State.STATE_NORMAL:
                BodyUp.PlayAction(tarAction);
                BodyLow.PlayAction(tarAction);
                break;
            case State.STATE_FIRING:
                BodyLow.PlayAction(tarAction);
                break;
        }

        m_CurAction = tarAction;
    }

    public void Move(Vector3 desiredMove)
    {
        m_CurSpeed = IsWalking ? WalkSpeed : RunSpeed;

        PlayerAction tarAction = PlayerAction.ACTION_STAND;
        m_IsMoving = Mathf.Abs(m_MoveInput.y) > float.Epsilon || Mathf.Abs(m_MoveInput.x) > float.Epsilon;
        if (m_IsMoving)
        {
            tarAction = PlayerAction.ACTION_RUN;
            Vector3 desiredMove = Vector3.Scale(HostPlayerCamera.forward, new Vector3(1, 0, 1)).normalized * m_MoveInput.y + HostPlayerCamera.right * m_MoveInput.x;

            m_PlayerTarForward = desiredMove.normalized;
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, ~0, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * m_CurSpeed;
            m_MoveDir.z = desiredMove.z * m_CurSpeed;
        }
        else
        {
            m_MoveDir.x = 0;
            m_MoveDir.z = 0;
            tarAction = PlayerAction.ACTION_STAND;
        }

        if (m_CharacterController.isGrounded)
        {
            if (m_Jump)
            {
                m_MoveDir.y = JumpSpeed;
                m_Jump = false;
                m_Jumping = true;
                tarAction = PlayerAction.ACTION_JUMP;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
            if (m_Jumping)
                tarAction = PlayerAction.ACTION_JUMP;
        }

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
        HostPlayer.PlayAction(tarAction);
    }
}
