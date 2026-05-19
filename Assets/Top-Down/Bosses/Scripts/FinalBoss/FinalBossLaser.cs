using UnityEngine;
using System.Collections;

public class FinalBossLaser : MonoBehaviour, IShieldResponse
{
    bool canRotate = true;
    [SerializeField] float rotationSpeed = 5f;

    LineRenderer laserLineOfSight;

    private void Start()
    {
        laserLineOfSight = GetComponent<LineRenderer>();

        laserLineOfSight.SetPosition(0, transform.position);
        laserLineOfSight.SetPosition(1, transform.position);

        StartCoroutine(RotationCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
        {
            Vector3 relativePosition = AssetCall.instance.playerSM.transform.position - transform.position;

            float angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            return;
        }

        Vector3 hitTarget = transform.position;

        RaycastHit2D a = Physics2D.Raycast(transform.position, transform.right, 20f, LayerMask.GetMask("Destructible", "Walls"));

        if (a) { hitTarget = a.point; }

        RaycastHit2D b = Physics2D.Raycast(transform.position, transform.right, 20f, LayerMask.GetMask("Player"));

        if (b) 
        {
            hitTarget = b.point;
            PlayerHealth.ModifyOxygenLevel(-20, false, b.point, this);
        }

        laserLineOfSight.SetPosition(1, hitTarget);
    }

    public void OnShieldAttack() { }

    IEnumerator RotationCheck()
    {
        yield return new WaitForSeconds(2f);

        GetComponent<AudioSource>().Play();

        canRotate = false;

        Destroy(gameObject, .5f);
    }
}
