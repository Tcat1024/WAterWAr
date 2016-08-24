using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class HostPlayerController : MonoBehaviour
{
    // Variables
    private bool m_OnGround = false;
    public bool OnGround
    {
        get
        {
            return m_OnGround;
        }
        private set
        {
            m_OnGround = value;
        }
    }
    public Player HostPlayer;
    public Transform HostPlayerCamera;
    public bool IsFocus;
    public bool IsWalking;
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpSpeed;
    public float ViewXTurnSpeed;
    public float ViewYTurnSpeed;
    public float ViewYMaxAngle = 85f;
    public float ViewYMinAngle = -85f;
    public float ViewAutoSmooth;
    public float ViewAdjustSmooth;
    public float TurnSmooth;
    [Range(0f,1f)]
    public float GravityMultiplier = 1f;
    public float CameraEpsilon = 0.2f;


    private Vector2 m_MoveInput;
    private Vector3 m_MoveDir;
    private Vector2 m_CameraInput;
    private Vector3 m_CameraFocusCenter;
    private float m_CameraDist;
    
    private Vector3 m_PlayerTarForward;
    private bool m_Jump;
    private bool m_Jumping;
    private bool m_IsPreGrounding;
    private bool m_IsAdjustingCamera;
    private float m_CurSpeed;
    private CollisionFlags m_CollisionFlags;

    private CharacterController m_CharacterController;

    // Functions

    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CameraFocusCenter = new Vector3(0, HostPlayerCamera.localPosition.y -  Mathf.Abs(HostPlayerCamera.localPosition.z) * Mathf.Tan(HostPlayerCamera.localRotation.eulerAngles.x * Mathf.PI / 180f), 0);
        m_CameraDist = Vector3.Distance(m_CameraFocusCenter, HostPlayerCamera.transform.localPosition);
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!m_Jumping)
            m_Jump = InputManager.Instance.Jump;

        if(!m_IsPreGrounding && m_CharacterController.isGrounded)
        {
            m_Jumping = false;
        }

        m_IsPreGrounding = m_CharacterController.isGrounded;

        HostPlayer.transform.forward = Vector3.Slerp(HostPlayer.transform.forward, m_PlayerTarForward, Time.deltaTime * TurnSmooth);

        if(InputManager.Instance.FireAxisPushed)
        {
            m_IsAdjustingCamera = true;
            m_CameraInput = new Vector2(InputManager.Instance.FireAxisX, InputManager.Instance.FireAxisY);
        }
        else if(InputManager.Instance.ViewAxisPushed)
        {
            m_IsAdjustingCamera = true;
            m_CameraInput = new Vector2(InputManager.Instance.ViewAxisX, InputManager.Instance.ViewAxisY);
        }
        else
        {
            m_IsAdjustingCamera = false;
        }
        if (m_IsAdjustingCamera)
        {
            float eulerx = -ViewYTurnSpeed * m_CameraInput.y + HostPlayerCamera.localRotation.eulerAngles.x;
            eulerx -= (int)(eulerx / 360) * 360;
            eulerx -= (int)(eulerx / 180) * 360;
            HostPlayerCamera.localRotation = Quaternion.Slerp(HostPlayerCamera.localRotation, Quaternion.Euler(Mathf.Clamp(eulerx, ViewYMinAngle, ViewYMaxAngle), ViewXTurnSpeed * m_CameraInput.x + HostPlayerCamera.localRotation.eulerAngles.y, 0), Time.deltaTime * ViewAdjustSmooth);
            HostPlayerCamera.localPosition = m_CameraFocusCenter - HostPlayerCamera.forward * m_CameraDist;
        }
    }

    void FixedUpdate()
    {
        m_CurSpeed = IsWalking ? WalkSpeed : RunSpeed;
        m_MoveInput = new Vector2(InputManager.Instance.MoveAxisX, InputManager.Instance.MoveAxisY);
        if (m_MoveInput.sqrMagnitude > 1)
        {
            m_MoveInput.Normalize();
        }

        PlayerAction tarAction = PlayerAction.ACTION_STAND;
        bool isMoving = Mathf.Abs(m_MoveInput.y) > float.Epsilon || Mathf.Abs(m_MoveInput.x) > float.Epsilon;
        if (isMoving)
        {
            tarAction = PlayerAction.ACTION_RUN;
            Vector3 cameraXZforward = Vector3.Scale(HostPlayerCamera.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 desiredMove = cameraXZforward * m_MoveInput.y + HostPlayerCamera.right * m_MoveInput.x;

            m_PlayerTarForward = desiredMove.normalized;
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, ~0, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * m_CurSpeed;
            m_MoveDir.z = desiredMove.z * m_CurSpeed;

            if (!m_IsAdjustingCamera)
            {
                if (Mathf.Abs(Mathf.Abs(Vector3.Dot(m_PlayerTarForward, cameraXZforward)) - m_PlayerTarForward.magnitude) > CameraEpsilon)
                {
                    Quaternion tarRoate = Quaternion.LookRotation(Vector3.Slerp(cameraXZforward, m_PlayerTarForward, Time.fixedDeltaTime * ViewAutoSmooth));
                    tarRoate = Quaternion.Euler(HostPlayerCamera.localRotation.eulerAngles.x, tarRoate.eulerAngles.y, tarRoate.eulerAngles.z);
                    HostPlayerCamera.localRotation = tarRoate;
                    HostPlayerCamera.localPosition = m_CameraFocusCenter - HostPlayerCamera.forward * m_CameraDist;
                }
            }
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
            if(m_Jumping)
                tarAction = PlayerAction.ACTION_JUMP;
        }

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
        HostPlayer.PlayAction(tarAction);
    }
}
