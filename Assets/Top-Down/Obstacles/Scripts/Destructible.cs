using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] GameObject particle;

    //Destroys the game object when the function is called upon
    public void CallDestroy()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
