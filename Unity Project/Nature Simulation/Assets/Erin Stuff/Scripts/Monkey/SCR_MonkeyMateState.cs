using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SCR_MonkeyMateState : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;
    float T = 0;
    private GameObject OtherMonkey;
    private List<GameObject> Mates = new List<GameObject>();
  
    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent) {

        Mates.AddRange(GameObject.FindGameObjectsWithTag("Monkey"));
        Monkey.readytomate = true;
        Mates.Remove(Monkey.gameObject);


        float dist = 0;
        float lowdist = 100000;
        for (int i = 0; i < Mates.Count; i++)
        {
          
            dist = Vector3.Distance(Monkey.transform.position, Mates[i].transform.position);

            if (dist < lowdist)
            {
                
                lowdist = dist;
                Target = Mates[i];
                OtherMonkey = Target;
              
            }

        }

        Mates.Clear();

        Agent = agent;
        agent.speed = 2;
        Monkey.righarm.enabled = true;
        T = 0;
        
        Monkey.rend.material.color = Color.white + Color.red;
        if(Target == null)
        {
            Monkey.SwitchState(Monkey.WanderState);
            return;
        }
        Agent.SetDestination(Target.transform.position);
    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey)
    {
        Agent.SetDestination(Target.transform.position);

        T += 1f * Time.deltaTime;

        Monkey.ArmAim.transform.position = Vector3.Slerp(Monkey.ArmAim.transform.position, Target.transform.position, T);
        Monkey.HeadAim.transform.position = Vector3.Slerp(Monkey.HeadAim.transform.position, Target.transform.position, T);

        if (Agent.remainingDistance < 1)
        {
            if (OtherMonkey.GetComponent<SCR_MonkeyStateManager>().readytomate == true)
            {
                Debug.Log(OtherMonkey);
               
                Monkey.hunger -= 25;
                Monkey.SwitchState(Monkey.WanderState);
                Monkey.Mate();
            }
            else
            {
                Monkey.SwitchState(Monkey.WanderState);
            }
        }
     
       
    }
    public override void ExitState(SCR_MonkeyStateManager Monkey) {
        Monkey.readytomate = false;
    }


     
}
