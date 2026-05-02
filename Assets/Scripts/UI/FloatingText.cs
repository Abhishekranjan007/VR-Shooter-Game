using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifeTime = 1.2f;

    private TextMeshPro textMesh;
    private Color startColor;
    private Transform cam;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startColor = textMesh.color;
        cam = Camera.main.transform;
    }

    void Update()
    {
        // Move up
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Face camera
        if (cam != null)
            transform.forward = cam.forward;

        // Fade out
        lifeTime -= Time.deltaTime;
        float alpha = lifeTime;

        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    public void SetText(string message, Color color)
    {
        textMesh.text = message;
        textMesh.color = color;
        startColor = color;
    }
}
