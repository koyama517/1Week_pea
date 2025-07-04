using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Transform handle;           // 剣の柄
    public Transform[] segments;       // セグメント群（右方向に並ぶ）
    public float segmentLength = 0.5f; // セグメント間の距離
    public float followSpeed = 15f;    // しなりの速さ（大きいほど硬い）

    void Update()
    {
        // 先頭セグメントは柄に固定
        segments[0].position = handle.position;

        for (int i = 1; i < segments.Length; i++)
        {
            // 目標位置 = 前のセグメントの右方向に segmentLength 離れた場所
            Vector3 targetPos = segments[i - 1].position + Vector3.right * segmentLength;

            // 現在位置をなめらかに追従させる
            segments[i].position = Vector3.Lerp(segments[i].position, targetPos, Time.deltaTime * followSpeed);
        }
    }
}
