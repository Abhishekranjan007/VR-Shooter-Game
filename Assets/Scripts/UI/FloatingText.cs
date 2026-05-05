using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifeTime = 1.2f;

    private TextMeshPro textMesh;
    private Color startColor;
    private Transform cam;

    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startColor = textMesh.color;        

        timer = lifeTime;
    }

    void Start()
    {
        if (Camera.main != null)
            cam = Camera.main.transform;
    }

    void Update()
    {
        // Move up
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Face camera
        if (cam != null)
        {
            transform.LookAt(cam);
            transform.Rotate(0, 180f, 0);
            //transform.forward = cam.forward;
        }
            

        // Fade out
        timer -= Time.deltaTime;
        float alpha = timer / lifeTime;

        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer <= 0)
            Destroy(gameObject);
    }

    public void SetText(string message, Color color)
    {
        textMesh.text = message;
        textMesh.color = color;
        startColor = color;
    }
}
