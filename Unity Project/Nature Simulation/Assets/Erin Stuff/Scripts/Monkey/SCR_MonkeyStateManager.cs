using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SCR_MonkeyStateManager : MonoBehaviour
{

    [SerializeField] private GameObject monkey;
    [SerializeField] private TwoBoneIKConstraint Arm;
    public float Speed;
    public Rigidbody Rb;
    public LayerMask layermask;
     public IKFootSolver righarm;
     public GameObject HeadAim, ArmAim,Mouth;
     public float hunger;
     public Renderer rend;
    public bool readytomate = false;

    private Animator Anim;
    private float Hunger;
  

 

    SCR_MonkeyBaseState CurrentState;
   
   public  SCR_MonkeyMateState MateState = new SCR_MonkeyMateState();
   public  SCR_MonkeySearchForFoodState SearchState = new SCR_MonkeySearchForFoodState();
   public  SCR_MonkeyAfraidState AfraidState = new SCR_MonkeyAfraidState();
   public  SCR_MonkeyEatState EatState = new SCR_MonkeyEatState();
    public SCR_MonkeyBaby BabyState = new SCR_MonkeyBaby();
    public SCR_MonkeyDoNothing NothingState = new SCR_MonkeyDoNothing();
    public SCR_MonkeyWanderState WanderState = new SCR_MonkeyWanderState();
 


    

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Color col = Random.ColorHSV(15/255f, 35/255f, 0.5f, 1f, 0f, 1f);
        rend.materials[1].color =col;
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach( Renderer ChildRend in rends)
        {
            ChildRend.material.color = col;
        }
       

        CurrentState = BabyState;

        CurrentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= 0.25f * Time.deltaTime;
        CurrentState.UpdateState(this);   
        if(hunger <0)
        {
            Die();
        }
    }

    public void SwitchState(SCR_MonkeyBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

    public void Die()
    {
        transform.parent = null;
        Destroy(this.gameObject);
    }

    public void Mate()
    {
        int i = Random.Range(0, 2);
        if(i<1)
        {
            GameObject Monkey = Instantiate(monkey, transform.position, Quaternion.identity);
        }
    }

 /*   public void ArmWeights(float NewWeight)
    {
        StartCoroutine(ChangeArmWeights(NewWeight));
    }

   private IEnumerator ChangeArmWeights(float NewArmWeights)
    {
   
        float OldWeight = Arm.weight;
        float T = 0;
        while(T < 1)
        {
           
            T += 1f * Time.deltaTime;
            Arm.weight = Mathf.Lerp(OldWeight, NewArmWeights, T);
            yield return null;

        }
        yield return null;
    }*/



    

    
}
