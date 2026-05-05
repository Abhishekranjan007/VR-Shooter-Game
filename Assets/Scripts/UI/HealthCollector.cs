using System.Collections;
using UnityEngine;

public class HealthCollector : MonoBehaviour
{
    public Player player;


    [Header("Effects")]
    public GameObject coinVFX;
    public GameObject floatingTextPrefab;
    //public AudioClip collectSound;
    public Transform vrTarget; // usually XR camera or hand
    private bool isCollected = false;
    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }



    void Update()
    {
        // Idle coin animation (visual interest)
        transform.Rotate(0, 90f * Time.deltaTime, 0);

        float bob = Mathf.Sin(Time.time * 2f) * 0.05f;
        transform.localPosition = startLocalPos + new Vector3(0, bob, 0);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (!other.CompareTag("Player")) return;

        isCollected = true;

        // Disable collider immediately (prevents multiple triggers)
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        if (player == null)
            player = other.GetComponentInParent<Player>();

        if (player != null)
            player.UpdateHealth(-5f);

        Transform target = vrTarget != null ? vrTarget : Camera.main.transform;

        // Start VFX sequence
        if (coinVFX != null)
        {
            StartCoroutine(SpawnAndFollowVFX(transform.position, target));
        }

        if (floatingTextPrefab != null)
        {
            Vector3 spawnPos = target.position
                + target.forward * 1.9f   // in front of player
                + Vector3.up * 0.02f;      // slightly above

            GameObject textObj = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);

            FloatingText ft = textObj.GetComponent<FloatingText>();
            if (ft != null)
            {
                ft.SetText("+5", Color.yellow);
            }
        }

        // Destroy coin immediately as VFX is separate 
        Destroy(gameObject);
    }

    IEnumerator SpawnAndFollowVFX(Vector3 startPos, Transform target)
    {
        GameObject vfx = Instantiate(coinVFX, startPos, Quaternion.identity);

        float t = 0f;
        float duration = 0.25f;

        // Move VFX from coin player
        while (t < duration)
        {
            if (vfx == null) yield break;

            t += Time.deltaTime;
            float progress = t / duration;

            vfx.transform.position = Vector3.Lerp(startPos, target.position, progress);

            yield return null;
        }

        // Attach to player so it stays visible
        if (vfx != null)
        {
            vfx.transform.SetParent(target);

            // Slight forward + upward offset so it's visible in VR
            vfx.transform.localPosition = new Vector3(0f, 0.2f, 0.3f);
        }
    }


}
