using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SCR_MonkeyBaby : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;
    private Vector3 Pos;
    float T = 0;

    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent)
    {
        Monkey.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        Monkey.readytomate = false;
        

      
        NavMeshHit Hit;
        if(NavMesh.SamplePosition(Monkey.transform.position, out Hit, 5f, NavMesh.AllAreas))
        {
            Agent = agent;
            Agent.SetDestination(Pos);

        }
        else
        {
            NavMeshTriangulation Triangles = NavMesh.CalculateTriangulation();
            int RandomNumber = Random.Range(0, Triangles.vertices.Length);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(Triangles.vertices[RandomNumber], out hit, 2f, 0))
            {
                Agent = agent;
                Agent.Warp(hit.position);
                Agent.enabled = true;
                Agent.SetDestination(Pos);
            }
            else
            {
                Monkey.transform.gameObject.SetActive(false);   
                
                return;
            }
        }
        agent.speed = 2;
        Monkey.righarm.enabled = true;
        T = 0;

        Monkey.rend.material.color = Color.white;
      

      
       
    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey)
    {
        T += 0.01f * Time.deltaTime;
        Monkey.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(1, 1, 1), T);
        Monkey.HeadAim.transform.position = Pos;
        Monkey.hunger = 25f;
        Agent.SetDestination(Pos);

        if (Agent.remainingDistance < 1)
        {

            Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        }
        if(T> 1)
        {
            Monkey.SwitchState(Monkey.SearchState);
        }


    }

    public override void ExitState(SCR_MonkeyStateManager Monkey)
    {

    }
}
