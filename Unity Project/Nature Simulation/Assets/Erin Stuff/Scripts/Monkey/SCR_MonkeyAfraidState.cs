using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_MonkeyAfraidState : SCR_MonkeyBaseState
{
 
    private GameObject Target;
    private float T = 0;

    

    public override void EnterState(SCR_MonkeyStateManager Monkey)
    {
        Monkey.righarm.enabled = true;
        
        Monkey.rend.material.color = Color.yellow;
        
      
        T = 0;
        Target = GameObject.FindGameObjectWithTag("Enemy");

       
    }
          
    public override void UpdateState(SCR_MonkeyStateManager Monkey) {
        T += 0.2f * Time.deltaTime;


        Monkey.HeadAim.transform.position = Vector3.Slerp(Monkey.ArmAim.transform.position, Target.transform.position, T);


        if (Vector3.Distance(Monkey.transform.position, Target.transform.position) > 1)
        {
            Monkey.transform.LookAt(Target.transform);
            Monkey.Rb.AddRelativeForce(Vector3.forward * 50, ForceMode.Force);
        }
        else
        {
            Monkey.SwitchState(Monkey.SearchState);
        }
    }
           
    public override void ExitState(SCR_MonkeyStateManager Monkey) {
    }

 
}
