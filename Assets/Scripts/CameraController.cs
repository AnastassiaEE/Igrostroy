using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector2 minMaxXY;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -minMaxXY.x, minMaxXY.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, -minMaxXY.y, minMaxXY.y);
 
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
