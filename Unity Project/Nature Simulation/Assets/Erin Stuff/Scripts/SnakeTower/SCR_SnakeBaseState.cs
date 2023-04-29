public abstract class SCR_SnakeBaseState
{
    public abstract void EnterState(SCR_SnakeStateManager Snake);

    public abstract void UpdateState(SCR_SnakeStateManager Snake);

    public abstract void ExitState(SCR_SnakeStateManager Snake);

}
