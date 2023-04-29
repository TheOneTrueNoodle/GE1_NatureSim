using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SCR_MonkeyBaby : SCR_MonkeyBaseState
{

    private GameObject Target;
    private Vector3 Pos;
    float T = 0;
    float N = 0;
    RaycastHit hit;

    public override void EnterState(SCR_MonkeyStateManager Monkey)
    {
        Monkey.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 100, Random.Range(-20, 20));
        
        if(Physics.Raycast(Pos,Vector3.down, out hit, Mathf.Infinity, Monkey.layermask))
        {
            Pos = hit.point;
           

        }
        Monkey.readytomate = false;
        

      
        Monkey.righarm.enabled = true;
        T = 0;

        Monkey.rend.material.color = Color.white;




    }

    public override void UpdateState(SCR_MonkeyStateManager Monkey)
    {


        N += 0.5f * Time.deltaTime;
        Pos = Vector3.Slerp(Pos, hit.point, N);
  
        T += 0.01f * Time.deltaTime;
        Monkey.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(1, 1, 1), T);
        
        Monkey.HeadAim.transform.position = Vector3.Slerp(Monkey.HeadAim.transform.position, hit.point, N);
        Monkey.hunger = 25f;

        if (Vector3.Distance(Monkey.transform.position, Pos) > 5)
        {
            Monkey.transform.LookAt(Monkey.HeadAim.transform.position);
            Monkey.Rb.AddRelativeForce((Vector3.forward * Monkey.Speed)* Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {

            Pos = Monkey.transform.position + new Vector3(Random.Range(-20, 20), 200, Random.Range(-20, 20));
           
            if (Physics.Raycast(Pos, Vector3.down, out hit, Mathf.Infinity, Monkey.layermask))
            {
                N = 0;
                
            }
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
