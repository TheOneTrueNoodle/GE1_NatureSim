using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SnakeWaitState : SCR_SnakeBaseState
{
    float T = 0;
    Vector3 oldpos,newpos;

    public override void EnterState(SCR_SnakeStateManager Snake)
    {
        oldpos = Snake.HeadAim.transform.position;
        T = 0;
    }

    public override void UpdateState(SCR_SnakeStateManager Snake)
    {
        T += 0.2f * Time.deltaTime;
        Snake.HeadAim.transform.position = Vector3.Slerp(oldpos, Snake.transform.position + new Vector3(0, 18, 0), T);
    }

   

    public override void ExitState(SCR_SnakeStateManager Snake)
    {

    }
}
