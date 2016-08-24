using UnityEngine;
using System.Collections;

public enum PlayerAction
{
    ACTION_STAND = 0,
    ACTION_RUN,
    ACTION_WALK,
    ACTION_JUMP,

    ACTION_NUM
}

public class Player : MonoBehaviour {

    public UpBody BodyUp;
    public LowBody BodyLow;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
    }

    public void PlayAction(PlayerAction tarAction)
    {
        BodyUp.PlayAction(tarAction);
        BodyLow.PlayAction(tarAction);
    }
    
}
