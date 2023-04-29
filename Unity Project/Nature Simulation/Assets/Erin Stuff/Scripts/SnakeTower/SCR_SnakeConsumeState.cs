using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SnakeConsumeState : SCR_SnakeBaseState
{
    float T = 0;
    Vector3 oldpos,newpos;
    private GameObject moke;

    public override void EnterState(SCR_SnakeStateManager Snake)
    {
        moke = Snake.Target;
        oldpos = Snake.HeadAim.transform.position;
        
        T = 0;
    }

    public override void UpdateState(SCR_SnakeStateManager Snake)
    {
        T += 0.2f * Time.deltaTime;
        Snake.HeadAim.transform.position = Vector3.Slerp(oldpos, Snake.transform.position + new Vector3(0, 18, 0), T);
        if(T >1f)
        {
           
            moke.gameObject.GetComponent<SCR_MonkeyStateManager>().Die();
            Snake.hunger += 25;
            Snake.Reproduce();
            Snake.SwitchState(Snake.WaitState);
        }
    }

   

    public override void ExitState(SCR_SnakeStateManager Snake)
    {

    }
}
