using System.Collections.Generic;
using UnityEngine;

public class BulletAlongCurve : MonoBehaviour
{
    private List<Vector3> pathPoints;
    private float speed = 5f;
    private int currentSegmentIndex = 0;
    private float t = 0f;

    private bool reachedEnd = false;
    private float timeAfterEnd = 0f;
    private float lifeAfterEnd = 2f; // 軌道終端後の寿命（秒）

    public void SetPath(List<Vector3> points, float moveSpeed = 5f)
    {
        pathPoints = points;
        speed = moveSpeed;
        currentSegmentIndex = 0;
        t = 0f;
        reachedEnd = false;
        timeAfterEnd = 0f;
    }

    void Update()
    {
        if (pathPoints == null || pathPoints.Count < 2)
        {
            Destroy(gameObject);
            return;
        }

        if (!reachedEnd)
        {
            Vector3 start = pathPoints[currentSegmentIndex];
            Vector3 end = pathPoints[currentSegmentIndex + 1];
            t += (speed * Time.deltaTime) / Vector3.Distance(start, end);

            transform.position = Vector3.Lerp(start, end, t);
            Vector3 dir = (end - start).normalized;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            if (t >= 1f)
            {
                t = 0f;
                currentSegmentIndex++;
                if (currentSegmentIndex >= pathPoints.Count - 1)
                {
                    reachedEnd = true;
                }
            }
        }
        else
        {
            // 軌道終端後の挙動（たとえば前方に直進）
            transform.position += transform.up * speed * Time.deltaTime;

            // 寿命カウントダウン
            timeAfterEnd += Time.deltaTime;
            if (timeAfterEnd >= lifeAfterEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}
