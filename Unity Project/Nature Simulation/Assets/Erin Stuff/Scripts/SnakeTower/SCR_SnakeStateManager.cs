using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SCR_SnakeStateManager : MonoBehaviour
{

    [SerializeField] private GameObject Snake;
    public LayerMask layermask;
    public GameObject HeadAim;
    public GameObject Target;
    public GameObject SnakeSeed;
    public float hunger;
    private float Hunger;

    SCR_SnakeBaseState CurrentState;

    public SCR_SnakeWaitState WaitState = new SCR_SnakeWaitState();
    public SCR_SnakeEatState EatState = new SCR_SnakeEatState();
    public SCR_SnakeConsumeState ConsumeState = new SCR_SnakeConsumeState();

    void Awake()
    {

        CurrentState = WaitState;

        CurrentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= 0.25f * Time.deltaTime;
        CurrentState.UpdateState(this);
        if (hunger < 0)
        {
            Die();
        }
    }

    public void SwitchState(SCR_SnakeBaseState state)
    {
        CurrentState = state;
        state.EnterState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Monkey"))
        {
            Target = other.gameObject;
            CurrentState = EatState;
            CurrentState.EnterState(this);
        }
    }

    public void Reproduce()
    {
        for (int i = Random.Range(0, 10); i < 10; i++)
        {
            GameObject Seed = Instantiate(SnakeSeed, HeadAim.transform.position, Quaternion.identity);

            Seed.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-50, 50), Random.Range(0, 40), Random.Range(-50, 50)), ForceMode.VelocityChange);
        }
        
    }


    private void Die()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Monkey"))
        {
            Target = null;
            
        }
    }

 





}
