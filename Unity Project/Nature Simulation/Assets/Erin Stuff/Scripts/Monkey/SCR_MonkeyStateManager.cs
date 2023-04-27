using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;

public class SCR_MonkeyStateManager : MonoBehaviour
{

    public TwoBoneIKConstraint LeftArm, RightArm;
    private Animator Anim;
    private float Hunger;
    private NavMeshAgent Agent;

 

    SCR_MonkeyBaseState CurrentState;
   
   public  SCR_MonkeyMateState MateState = new SCR_MonkeyMateState();
   public  SCR_MonkeySearchForFoodState SearchState = new SCR_MonkeySearchForFoodState();
   public  SCR_MonkeyAfraidState AfraidState = new SCR_MonkeyAfraidState();


    

    void Awake()
    {
        
        Agent = gameObject.GetComponent<NavMeshAgent>();

        CurrentState = SearchState;

        CurrentState.EnterState(this, Agent);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.UpdateState(this);

        Debug.Log(CurrentState);
    }

    public void SwitchState(SCR_MonkeyBaseState state)
    {
        CurrentState = state;
        state.EnterState(this, Agent);
    }
}
