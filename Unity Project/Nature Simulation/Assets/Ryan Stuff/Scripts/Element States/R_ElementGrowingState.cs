using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_ElementGrowingState : R_ElementBaseState
{
    public override void EnterState(R_ElementClass element)
    {
        element.transform.localScale = Vector3.zero;
    }
    public override void UpdateState(R_ElementClass element)
    {
        if (element.transform.localScale.x <= element.scale.x)
        {
            var t = 0f;
            t += element.GrowSpeed * Time.deltaTime;
            element.transform.localScale += new Vector3(t, t, t);
        }
        else
        {
            element.Grown = true;
            element.SwitchState(element.livingState);
        }
    }
}
