using UnityEngine;

public class Handle : MonoBehaviour
{
    public Transform player;       // �v���C���[��Transform
    public float maxDistance = 3f; // �v���C���[����̍ő勗������
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
