using UnityEngine;

public class ShootOpenDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        GetComponent<Animator>().Play("SODoorOpen");
        GetComponent<AudioSource>().Play();
    }
}
