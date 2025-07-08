using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Transform handle;           // ���̕�
    public Transform[] segments;       // �Z�O�����g�Q�i�E�����ɕ��ԁj
    public float segmentLength = 0.5f; // �Z�O�����g�Ԃ̋���
    public float followSpeed = 15f;    // ���Ȃ�̑����i�傫���قǍd���j

    void Update()
    {
        // �擪�Z�O�����g�͕��ɌŒ�
        segments[0].position = handle.position;

        for (int i = 1; i < segments.Length; i++)
        {
            // �ڕW�ʒu = �O�̃Z�O�����g�̉E������ segmentLength ���ꂽ�ꏊ
            Vector3 targetPos = segments[i - 1].position + Vector3.right * segmentLength;

            // ���݈ʒu���Ȃ߂炩�ɒǏ]������
            segments[i].position = Vector3.Lerp(segments[i].position, targetPos, Time.deltaTime * followSpeed);
        }
    }
}
