using UnityEngine;

public class SnowFollowPlayer : MonoBehaviour
{
    public Transform player;
    
    void LateUpdate()
    {
        transform.position = player.position;
    }
}
