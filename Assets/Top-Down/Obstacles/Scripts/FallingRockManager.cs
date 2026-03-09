using UnityEngine;
using System.Collections;

public class FallingRockManager : MonoBehaviour
{
    [SerializeField] GameObject fallingRock;
    BoxCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    float MAXtimer = 1f;
    float timer;

    private void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime; return; }

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, coll.size, 0f, Vector2.zero, 0f, LayerMask.GetMask("Explosion"));

        if (hit)
        {
            timer = MAXtimer;
            StartCoroutine(RockSlide());
            AssetCall.instance.cameraEffectors.SetCameraShake(2);
        }
    }

    IEnumerator RockSlide()
    {
        yield return new WaitForSeconds(1f);
        // have some way of shaking the camera

        float radiusX = (coll.size.x - 1) / 2f;
        float radiusY = (coll.size.y - 1) / 2f;

        Instantiate(fallingRock, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);
        Instantiate(fallingRock, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);
        Instantiate(fallingRock, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);
    }
}
