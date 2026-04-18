using UnityEngine;

public class SnowFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2f;
    
    void LateUpdate()
    {
        Vector3 target = player.position;

        transform.position = Vector3.Lerp(
            transform.position,
            target,
            Time.deltaTime * followSpeed
        );
    }
}
