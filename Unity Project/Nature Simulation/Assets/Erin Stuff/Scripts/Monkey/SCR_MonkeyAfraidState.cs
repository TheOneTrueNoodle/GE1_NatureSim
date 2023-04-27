using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_MonkeyAfraidState : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;
    

    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent)
    {
        Monkey.LeftArm.weight = 0;
        Monkey.RightArm.weight = 0;
        Agent = agent;
        
        Agent.speed = 5;

        Target = GameObject.FindGameObjectWithTag("Fruit");

        Agent.SetDestination(Agent.transform.position + new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40)));
    }
          
    public override void UpdateState(SCR_MonkeyStateManager Monkey) {


        if (Agent.remainingDistance < 10)
        {
            Monkey.SwitchState(Monkey.SearchState);
        }
    }
           
    public override void ExitState(SCR_MonkeyStateManager Monkey) {
    }

 
}
