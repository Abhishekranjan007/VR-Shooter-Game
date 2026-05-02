using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject FloatingTextPrefab;

    private void Awake()
    {
        Instance = this;
    }

    //FLoating text on Headshot
    public void ShowFloatingText(Vector3 position, int bonus)
    {
        if (FloatingTextPrefab == null)
        {
            Debug.LogError("FloatingTextPrefab not assigned!");
            return;
        }

        GameObject ft_go = Instantiate(FloatingTextPrefab, position + Vector3.up * 0.5f, Quaternion.identity);
        
        FloatingText floatingText = ft_go.GetComponent<FloatingText>();

        if (floatingText != null)
        {
            floatingText.SetText("HEADSHOT +" + bonus, Color.red);
        }
    }
}
