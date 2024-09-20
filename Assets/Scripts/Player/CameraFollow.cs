using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.3f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        var pos = transform.position;
        var targetPos = target.position + offset;
        
        transform.position = Vector3.SmoothDamp(new Vector3(pos.x, pos.y, -10), new Vector3(targetPos.x, targetPos.y, -10), ref velocity, smoothTime);
    }
    
}