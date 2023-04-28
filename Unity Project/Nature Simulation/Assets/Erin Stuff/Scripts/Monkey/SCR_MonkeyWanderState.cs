using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SCR_MonkeyWanderState : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;
    private Vector3 Pos;
    float T = 0;
    float time;

    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent)
    {
        Agent = agent;
        time = Random.Range(10, 30);
        
        Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        Debug.Log(Pos);

        agent.speed = 2.5f;
        Monkey.righarm.enabled = true;
        T = 0;

        Monkey.rend.material.color = Color.white;

        Agent.SetDestination(Pos);


    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey)
    {
        T += 1 * Time.deltaTime;

        Monkey.HeadAim.transform.position = Pos;
       Agent.SetDestination(Pos);

        if (Agent.remainingDistance < 1)
        {

            Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        }
        if (T > time)
        {
            Monkey.SwitchState(Monkey.SearchState);
        }


    }

    public override void ExitState(SCR_MonkeyStateManager Monkey)
    {

    }
}
