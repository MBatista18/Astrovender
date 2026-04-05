using UnityEngine;

public class ArmManager : StateMachineBase
{
    public SegmentSM[] armSegments;
    AudioCall audioCall;
    public AudioCall GetAudioCall() { return audioCall; }

    public override void InstantiateComponents()
    {
        audioCall = GetComponent<AudioCall>();
        base.InstantiateComponents();
    }

    public float armDirection;

    public ArmStateJab stateJab;
    public ArmStateMove stateMove;
    public override StateBase InitialState()
    {
        return stateMove;
    }
    public ArmStateRetract stateRetract;
    public ArmStateSwipe stateSwipe;

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateJab = new ArmStateJab(this);
        stateMove = new ArmStateMove(this);
        stateRetract = new ArmStateRetract(this);
        stateSwipe = new ArmStateSwipe(this);
    }

    public bool startOnJab;

    public float xDistance;
    public float yDistance;

    [HideInInspector] public Vector3 startingPosition;

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        startingPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.right * xDistance);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * yDistance);
    }
}
