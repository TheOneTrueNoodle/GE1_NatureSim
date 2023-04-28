using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_MonkeyDoNothing : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;

    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent)
    {
        Monkey.righarm.enabled = false;
        Agent = agent;
        agent.speed = 0;
        agent.enabled = false;
        Monkey.rend.material.color = Color.black;


    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey)
    {

        Monkey.HeadAim.transform.position = Monkey.transform.position + new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
        Monkey.ArmAim.transform.position = Monkey.transform.position + new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

    }

    public override void ExitState(SCR_MonkeyStateManager Monkey)
    {

    }
}
