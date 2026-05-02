using UnityEngine;

public class FollowCameraNoRotation : MonoBehaviour
{
    public Transform cameraTransform;

    void LateUpdate()
    {
        transform.position = cameraTransform.position;

        // keep collider upright
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }
}
