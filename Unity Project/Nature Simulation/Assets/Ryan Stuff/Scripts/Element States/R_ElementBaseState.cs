using UnityEngine;

[System.Serializable]
public abstract class R_ElementBaseState
{
    public abstract void EnterState(R_ElementClass element);

    public abstract void UpdateState(R_ElementClass element);
}
