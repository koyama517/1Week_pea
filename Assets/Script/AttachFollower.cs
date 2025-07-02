using UnityEngine;

public class AttachFollower : MonoBehaviour
{
    public GameObject followerObject; // �Ǐ]����I�u�W�F�N�g
    public float offsetX = 1f;        // �E�����̃I�t�Z�b�g����

    void Start()
    {
        // follower ���E�ɔz�u
        Vector3 rightPos = transform.position + new Vector3(offsetX, 0f, 0f);
        followerObject.transform.position = rightPos;

        // �W���C���g�ǉ��iDistanceJoint2D�̏ꍇ�j
        DistanceJoint2D joint = followerObject.AddComponent<DistanceJoint2D>();
        joint.connectedBody = GetComponent<Rigidbody2D>();
        joint.autoConfigureDistance = false;
        joint.distance = offsetX;
        joint.enableCollision = false; // �Փ˂𖳌��ɂ������ꍇ
    }
}
