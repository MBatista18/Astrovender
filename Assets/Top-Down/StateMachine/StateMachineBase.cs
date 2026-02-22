using UnityEngine;

public class StateMachineBase : MonoBehaviour
{
    StateBase currentState;
    public StateBase GetCurrentState() { return currentState; }
    public void ChangeState(StateBase state)
    {
        if (!canChangeStates || this == null) { return; }

        currentState.thisEnd();
        currentState = state;
        currentState.thisStart();
    }

    bool canChangeStates;
    public bool GetCanChangeStates() { return canChangeStates; }
    public void SetCanChangeStates(bool a) { canChangeStates = a; }

    public virtual void InstantiateComponents() { }
    public virtual void InstantiateStates() { }
    public virtual void InstantiateValues() { SetCanChangeStates(true); }

    //public virtual void StartFunctions() { }
    public virtual void OnEnableFunctions() { }
    public virtual void OnDisableFunctions() { }
    public virtual StateBase InitialState() { return null; }
    public virtual StateBase DeathState() { this.SetCanChangeStates(false); return null; }

    private void Awake()
    {
        InstantiateComponents();
        InstantiateStates();
        InstantiateValues();
    }

    private void Start()
    {
        if (InitialState() != null) { currentState = InitialState(); Debug.Log("True"); }
        else { Debug.Log("false"); }

        currentState?.thisStart();
    }

    private void Update()
    {
        UpdateFunctions();
        currentState?.thisUpdate();
    }

    public virtual void UpdateFunctions() { }

    private void FixedUpdate()
    {
        currentState?.thisFixedUpdate();
    }
    private void LateUpdate()
    {
        currentState?.thisLateUpdate();
    }

    private void OnEnable()
    {
        OnEnableFunctions();
    }

    private void OnDisable()
    {
        OnDisableFunctions();
    }
}
