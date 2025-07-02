using UnityEngine;

public class Handle : MonoBehaviour
{
    public Transform player;       // プレイヤーのTransform
    public float maxDistance = 3f; // プレイヤーからの最大距離制限
    bool stepTriger;


    void FixedUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = mouseWorldPos - player.position;
        float distance = direction.magnitude;

        if (distance > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        transform.position = player.position + direction;
    }

}
