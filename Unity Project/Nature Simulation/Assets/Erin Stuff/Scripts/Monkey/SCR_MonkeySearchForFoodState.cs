using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MonkeySearchForFoodState : SCR_MonkeyBaseState
{
 
    private GameObject Target;
    float T = 0;
    private List<GameObject> Fruit = new List<GameObject>();


    public override void EnterState(SCR_MonkeyStateManager Monkey) {


        Fruit.AddRange(GameObject.FindGameObjectsWithTag("Fruit"));

        float dist = 0;
        float lowdist = 10000;
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
 
        Monkey.righarm.enabled = false;
        T = 0;
   
        Monkey.rend.material.color = Color.green;

       

    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey) {
     
        if (Target == null)
        {
            Monkey.SwitchState(Monkey.SearchState);
                return;
        }
            T += 1f * Time.deltaTime;

            Monkey.ArmAim.transform.position = Vector3.Slerp(Monkey.ArmAim.transform.position, Target.transform.position, T);
            Monkey.HeadAim.transform.position = Vector3.Slerp(Monkey.HeadAim.transform.position, Target.transform.position, T);

        if (Vector3.Distance(Monkey.transform.position, Target.transform.position) > 3)
        {
            Monkey.transform.LookAt(Monkey.HeadAim.transform.position);
            Monkey.Rb.AddRelativeForce((Vector3.forward * Monkey.Speed)* Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
                Target.transform.parent = Monkey.ArmAim.transform;
                Monkey.SwitchState(Monkey.EatState);
        }
       
    }

    public override void ExitState(SCR_MonkeyStateManager Monkey) { 
    }



}
