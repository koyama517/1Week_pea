using UnityEngine;
using System.Collections.Generic;

public class BulletAlongCurve : MonoBehaviour
{
    public float followDuration = 0.3f;     // �O�����Ȃ��鎞��
    public float moveSpeed = 10f;           // ���i���x
    public float lifeTime = 3f;             // �ő吶������

    private List<Vector3> path = new List<Vector3>();
    private float timer = 0f;
    private int currentIndex = 0;
    private bool isFollowingPath = true;
    private Vector3 finalDirection;

    public void SetPath(List<Vector3> curvePoints)
    {
        path = new List<Vector3>(curvePoints);
        transform.position = path[0];

        if (path.Count >= 2)
        {
            // �����̌����i�Z�O�����g�̌����j
            Vector3 dir = path[1] - path[0];
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, dir));
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isFollowingPath && path.Count > 1)
        {
            float t = timer / followDuration;
            int segmentCount = path.Count - 1;

            if (t >= 1f)
            {
                // �O���I�� �� ���i�ֈڍs
                isFollowingPath = false;
                Vector3 lastDir = (path[segmentCount] - path[segmentCount - 1]).normalized;
                finalDirection = lastDir;
                return;
            }

            // �J�[�u����`��Ԃł��ǂ�
            float totalT = t * segmentCount;
            int index = Mathf.FloorToInt(totalT);
            float localT = totalT - index;

            if (index < segmentCount)
            {
                Vector3 p0 = path[index];
                Vector3 p1 = path[index + 1];
                Vector3 pos = Vector3.Lerp(p0, p1, localT);
                transform.position = pos;

                Vector3 dir = (p1 - p0).normalized;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, dir));
            }
        }
        else
        {
            // ���i�i�Ō�̊p�x�j
            transform.position += finalDirection * moveSpeed * Time.deltaTime;
        }

        // ��莞�Ԍ�ɍ폜
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �G�ɓ���������폜�i�^�O�Ȃǂŏ����ǉ��j
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
