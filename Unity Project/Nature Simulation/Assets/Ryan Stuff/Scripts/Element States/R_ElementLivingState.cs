using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_ElementLivingState : R_ElementBaseState
{
    float time;
    public override void EnterState(R_ElementClass element)
    {
        if (element.Dies == false) { element.SwitchState(null); }
        time = Random.Range(element.MinLifetime, element.MaxLifetime);
    }

    public override void UpdateState(R_ElementClass element)
    {
        if(time >= 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            element.SwitchState(element.dyingState);
        }
    }
}
