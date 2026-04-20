using UnityEngine;
using System.Collections;

public class FallingRockManager : MonoBehaviour
{
    [SerializeField] GameObject fallingRock;
    BoxCollider2D coll;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        coll = GetComponent<BoxCollider2D>();
    }

    float MAXtimer = 1f;
    float timer;

    bool hit;

    [SerializeField] bool checkForBomb = true;
    

    private void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime; return; }

        if (checkForBomb && Physics2D.BoxCast(transform.position, coll.size, 0f, Vector2.zero, 0f, LayerMask.GetMask("Explosion")))
        {
            hit = true;
        }

        if (hit)
        {

            hit = false;

            timer = MAXtimer;
            StartCoroutine(RockSlide());
            AssetCall.instance.cameraEffectors.SetCameraShake(2);
        }
    }

    public void CallRockslide() {  hit = true; }

    IEnumerator RockSlide()
    {
        audioSource.Play();

        yield return new WaitForSeconds(1f);
        // have some way of shaking the camera

        float radiusX = (coll.size.x - 1) / 2f;
        float radiusY = (coll.size.y - 1) / 2f;

        Instantiate(fallingRock, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);
        Instantiate(fallingRock, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);
        Instantiate(fallingRock, transform.position + new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0), Quaternion.identity);
    }
}
