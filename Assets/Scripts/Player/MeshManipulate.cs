using UnityEngine;

public class MeshManipulate : MonoBehaviour
{
    private MeshRenderer[] meshes;

    void Awake()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();
    }

    public void Show()
    {
        foreach(var mesh in meshes)
        {
            mesh.enabled = true;
        }
    }

    public void Hide()
    {
        foreach(var mesh in meshes)
        {
            mesh.enabled = false;
        }
    }
}
