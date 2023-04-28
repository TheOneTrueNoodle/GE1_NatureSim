using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_MonkeyAfraidState : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;
    private float T = 0;
    

    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent)
    {
        Monkey.righarm.enabled = true;
        Agent = agent;
        Monkey.rend.material.color = Color.yellow;
        
        Agent.speed = 5;
        T = 0;
        Target = GameObject.FindGameObjectWithTag("Enemy");

        Agent.SetDestination(Agent.transform.position + new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40)));
        
    }
          
    public override void UpdateState(SCR_MonkeyStateManager Monkey) {
        T += 0.2f * Time.deltaTime;

        Monkey.HeadAim.transform.position = Vector3.Slerp(Monkey.ArmAim.transform.position, Target.transform.position, T);

        if (Agent.remainingDistance < 10)
        {
            Monkey.SwitchState(Monkey.SearchState);
        }
    }
           
    public override void ExitState(SCR_MonkeyStateManager Monkey) {
    }

 
}
