using UnityEngine;

public class FlexibleSword : MonoBehaviour
{
    public GameObject segmentPrefab;
    public int segmentCount = 10;
    public float segmentSpacing = 0.4f;

    void Start()
    {
        Rigidbody2D prevBody = GetComponent<Rigidbody2D>();

        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Vector2.right * segmentSpacing * (i + 1);
            GameObject segment = Instantiate(segmentPrefab, pos, Quaternion.identity);
            Rigidbody2D rb = segment.GetComponent<Rigidbody2D>();
            rb.mass = 5f;
            rb.gravityScale = 0;

            // ジョイントで接続
            HingeJoint2D joint = segment.AddComponent<HingeJoint2D>();
            joint.connectedBody = prevBody;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector2(-segmentSpacing / 2f, 0);
            joint.connectedAnchor = new Vector2(segmentSpacing / 2f, 0);
            joint.useLimits = true;
            JointAngleLimits2D limits = new JointAngleLimits2D { min = -15, max = 15 };
            joint.limits = limits;

            // オプション：直線補正
            DistanceJoint2D dist = segment.AddComponent<DistanceJoint2D>();
            dist.connectedBody = prevBody;
            dist.autoConfigureDistance = false;
            dist.distance = segmentSpacing;
            dist.maxDistanceOnly = true;

            prevBody = rb;
        }
    }
}