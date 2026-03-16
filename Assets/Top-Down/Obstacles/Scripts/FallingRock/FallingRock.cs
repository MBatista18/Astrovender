using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] GameObject collider;
    [SerializeField] GameObject particle;


    private void Awake()
    {
        collider.SetActive(false);
    }

    float timer = 0f;

    private void Update()
    {
        if (timer > 1)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > .92f)
            {
                collider.SetActive(true);
            }
        }
    }
}
