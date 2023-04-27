using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SCR_MonkeySearchForFoodState : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;

    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent) {
        Agent = agent;
        agent.speed = 2;

        Monkey.LeftArm.weight = 1;
        Monkey.RightArm.weight = 1;
        Target = GameObject.FindGameObjectWithTag("Fruit");
    
        Agent.SetDestination(Target.transform.position);

    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey) {



        Debug.Log(Target);
        Agent.SetDestination(Target.transform.position);

        
        if (Agent.remainingDistance < 10)
        {
            Monkey.SwitchState(Monkey.AfraidState);   
        }
    }

    public override void ExitState(SCR_MonkeyStateManager Monkey) { 
    }



}
