using UnityEngine;

public class AttachFollower : MonoBehaviour
{
    public GameObject followerObject; // 追従するオブジェクト
    public float offsetX = 1f;        // 右方向のオフセット距離

    void Start()
    {
        // follower を右に配置
        Vector3 rightPos = transform.position + new Vector3(offsetX, 0f, 0f);
        followerObject.transform.position = rightPos;

        // ジョイント追加（DistanceJoint2Dの場合）
        DistanceJoint2D joint = followerObject.AddComponent<DistanceJoint2D>();
        joint.connectedBody = GetComponent<Rigidbody2D>();
        joint.autoConfigureDistance = false;
        joint.distance = offsetX;
        joint.enableCollision = false; // 衝突を無効にしたい場合
    }
}
