using UnityEngine.AI;

public abstract class SCR_MonkeyBaseState 
{


    public abstract void EnterState(SCR_MonkeyStateManager Monkey, NavMeshAgent agent);
                                    
    public abstract void UpdateState(SCR_MonkeyStateManager Monkey);
                                   
    public abstract void ExitState(SCR_MonkeyStateManager Monkey);





  


}
