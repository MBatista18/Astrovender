using UnityEngine;
using System.Collections;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] Transform[] transformPositions;
    public Transform GetTransformAt(int index) { return transformPositions[index]; }
    public int GetPositionsCount() { return transformPositions.Length; }

    [SerializeField] GalleryTarget[] galleryTargets;

    bool summoned = false;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < galleryTargets.Length; i++)
        {
            galleryTargets[i].SetGalleryManager(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) { CallBirds(); }
    }

    public void CallBirds()
    {
        if (summoned) { return; }
        audioSource.Play();

        StartCoroutine(SummonBirds());
    }

    IEnumerator SummonBirds()
    {
        summoned = true;

        for (int i = 0; i < galleryTargets.Length; i++)
        {
            if (galleryTargets[i].Equals(null)) { continue; }

            galleryTargets[i].BeginFlying();
            yield return new WaitForSeconds(.25f);
        }
    }
}
