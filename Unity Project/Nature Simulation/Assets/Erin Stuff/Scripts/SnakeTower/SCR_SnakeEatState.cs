using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SnakeEatState : SCR_SnakeBaseState
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
       if(Snake.Target == null)
        {
            Snake.SwitchState(Snake.WaitState);
               return;

            
        }
            T += 0.5f * Time.deltaTime;
            Snake.HeadAim.transform.position = Vector3.Slerp(oldpos, Snake.Target.transform.position, T);
        if(T >1)
        {
            Snake.Target.transform.parent = Snake.HeadAim.transform;
            Snake.Target.gameObject.GetComponent<SCR_MonkeyStateManager>().SwitchState(new SCR_MonkeyDoNothing());
            Snake.Target.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Snake.SwitchState(Snake.ConsumeState);
        }
        
    }



    public override void ExitState(SCR_SnakeStateManager Snake)
    {

    }

    
}
