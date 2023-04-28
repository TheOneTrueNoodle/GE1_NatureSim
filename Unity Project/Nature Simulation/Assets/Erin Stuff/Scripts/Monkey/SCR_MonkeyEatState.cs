using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SCR_MonkeyEatState : SCR_MonkeyBaseState
{
    private NavMeshAgent Agent;
    private GameObject Target;
    float T = 0;
    Transform originalpos;

    private List<GameObject> Fruit = new List<GameObject>();
    public override void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent) {

        Fruit.AddRange(GameObject.FindGameObjectsWithTag("Fruit"));

        float dist = 0;
        float lowdist = 100;
        for (int i = 0; i < Fruit.Count; i++)
        {
            dist = Vector3.Distance(Monkey.transform.position, Fruit[i].transform.position);
          
            if (dist < lowdist)
            {
                lowdist = dist;
                Target = Fruit[i];
             
            }

        }

        Fruit.Clear();
        Agent = agent;
        agent.speed = 0;
        Monkey.righarm.enabled = false;
        T = 0;
       
        Monkey.rend.material.color = Color.blue;
        originalpos = Target.transform;
        Agent.SetDestination(Target.transform.position);

    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey) {

        T += 1f * Time.deltaTime;
   
        Monkey.ArmAim.transform.position = Vector3.Slerp(originalpos.position, Monkey.Mouth.transform.position, T/5);
      
        Agent.SetDestination(Target.transform.position);

        
        if (T>1)
        {
            
            Target.gameObject.GetComponent<R_ElementClass>().SwitchState(Target.gameObject.GetComponent<R_ElementClass>().dyingState);
            Monkey.hunger += 10;
            if (Monkey.hunger > 50)
            {
                Monkey.SwitchState(Monkey.MateState);
            }
            else
            {
                Monkey.SwitchState(Monkey.SearchState);
            }
        }
    }

    public override void ExitState(SCR_MonkeyStateManager Monkey) { 

    }



}
