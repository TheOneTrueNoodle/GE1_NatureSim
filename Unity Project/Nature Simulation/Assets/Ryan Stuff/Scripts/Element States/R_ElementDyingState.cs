using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_ElementDyingState : R_ElementBaseState
{
    public override void EnterState(R_ElementClass element)
    {

    }
    public override void UpdateState(R_ElementClass element)
    {
        if (element.transform.localScale.x > 0)
        {
            var t = 0f;
            t += element.GrowSpeed * Time.deltaTime;
            element.transform.localScale -= new Vector3(t, t, t);
        }
        else
        {
            int newElement = Random.Range(1, 100);

            if (newElement < element.ChanceForNewElement)
            {
                element.SpawnNewElement(element.transform.position);    
            }
            else
            {
                element.destroy();
            }
        }
    }
}
