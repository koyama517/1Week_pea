using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Handle handle;
    public Transform[] segments;

    public float segmentLength = 0.5f;
    public float followSpeed = 15f;
    public float rotationSpeed = 10f;
    public float returnSpeed = 3f;
    public float maxBend = 20f;
    public float inputSensitivity = 10f;

    private float[] currentBends;
    private float[] targetBends;
    private float bendDirection = 0f;

    private Vector3 prevHandlePos;

    void Start()
    {
        currentBends = new float[segments.Length];
        targetBends = new float[segments.Length];
        prevHandlePos = handle.transform.position;
    }

    void Update()
    {
        if (handle == null || segments.Length == 0) return;

        // ハンドルのY移動量を取得
        Vector3 currentHandlePos = handle.transform.position;
        float deltaY = currentHandlePos.y - prevHandlePos.y;
        prevHandlePos = currentHandlePos;

        // しなり方向（上に動けば先端が垂れる）
        bendDirection = Mathf.Lerp(bendDirection, Mathf.Clamp(deltaY * inputSensitivity, -1f, 1f), Time.deltaTime * 10f);

        // セグメントごとにしなり角を設定
        for (int i = 0; i < segments.Length; i++)
        {
            float t = (float)i / (segments.Length - 1); // 根元→先端
            targetBends[i] = maxBend * t * -bendDirection; // 符号反転で自然なしなりに
        }

        // セグメント更新
        for (int i = 0; i < segments.Length; i++)
        {
            if (i == 0)
            {
                segments[i].position = handle.transform.position;
                segments[i].rotation = handle.transform.rotation;
                currentBends[i] = 0f;
            }
            else
            {
                Transform prev = segments[i - 1];

                // しなり角を滑らかに追従
                currentBends[i] = Mathf.Lerp(currentBends[i], targetBends[i], Time.deltaTime * returnSpeed);

                float baseAngle = handle.transform.eulerAngles.z;
                float totalAngle = baseAngle + currentBends[i];

                Quaternion targetRot = Quaternion.Euler(0, 0, totalAngle);
                segments[i].rotation = Quaternion.Slerp(segments[i].rotation, targetRot, Time.deltaTime * rotationSpeed);

                Vector3 targetPos = prev.position + prev.right * segmentLength;
                segments[i].position = Vector3.Lerp(segments[i].position, targetPos, Time.deltaTime * followSpeed);
            }
        }
    }
}
