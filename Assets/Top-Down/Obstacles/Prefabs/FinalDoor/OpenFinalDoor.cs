using UnityEngine;

public class OpenFinalDoor : MonoBehaviour
{
    bool openFinalDoor = false;
    public void OpenDoor() { openFinalDoor = true; }
    bool doOnce = false;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!openFinalDoor) { return; }
        if (doOnce) { return; }

        doOnce = true;

        animator.Play("OpenFinalDoor");
    }
}
