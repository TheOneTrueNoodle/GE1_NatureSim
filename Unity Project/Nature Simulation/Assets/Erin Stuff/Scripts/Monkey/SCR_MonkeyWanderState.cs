using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_MonkeyWanderState : SCR_MonkeyBaseState
{

    private GameObject Target;
    private Vector3 Pos;
    float T = 0;
    RaycastHit hit;
    float time;
    float N = 0;
  

    public override void EnterState(SCR_MonkeyStateManager Monkey)
    {
      
        time = Random.Range(10, 30);
        
        Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        Debug.Log(Pos);


        if (Physics.Raycast(Pos, Vector3.down, out hit, Mathf.Infinity, Monkey.layermask))
        {
            Pos = hit.point;


        }


        Monkey.righarm.enabled = true;
        T = 0;

        Monkey.rend.material.color = Color.white;
      
       
        


    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey)
    { 
        T += 1 * Time.deltaTime;

        N += 0.5f * Time.deltaTime;
        Pos = Vector3.Slerp(Pos, hit.point, N);
      
   

        Monkey.HeadAim.transform.position = Vector3.Slerp(Monkey.HeadAim.transform.position, hit.point, N);
        Monkey.hunger = 25f;

        if (Vector3.Distance(Monkey.transform.position, Pos) > 1)
        {
            Monkey.transform.LookAt(Monkey.HeadAim.transform.position);
            Monkey.Rb.AddRelativeForce((Vector3.forward * Monkey.Speed) * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {

            Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 200, Random.Range(-20, 20));

            if (Physics.Raycast(Pos, Vector3.down, out hit, Mathf.Infinity, Monkey.layermask))
            {
                N = 0;

            }
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
