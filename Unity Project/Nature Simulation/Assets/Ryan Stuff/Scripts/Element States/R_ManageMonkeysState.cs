using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_ManageMonkeysState : R_ElementBaseState
{
    public override void EnterState(R_ElementClass element)
    {

    }

    public override void UpdateState(R_ElementClass element)
    {
        if(element.GetComponent<R_MonkeyHouse>().monkeys.Count == 0)
        {
            element.SwitchState(element.dyingState);
        }
    }
}
