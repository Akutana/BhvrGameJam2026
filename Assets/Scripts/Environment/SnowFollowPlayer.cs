using UnityEngine;

public class SnowFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 0f;  //This is how much the block of particles lags behind
                                    //player's movement (higher number = more lag)
    
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
