using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private float moveSpeed;
    private Rigidbody2D rb;

    public float knockbackDistance = 2f;
    public float knockbackDuration = 0.2f;

    private bool isKnockback = false;
    private Vector2 knockbackStartPos;
    private Vector2 knockbackEndPos;
    private float knockbackTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    void Update()
    {
        if (isKnockback)
        {
            knockbackTimer += Time.deltaTime;
            float t = knockbackTimer / knockbackDuration;
            transform.position = Vector2.Lerp(knockbackStartPos, knockbackEndPos, t);

            if (t >= 1f)
            {
                isKnockback = false;
            }

            return;
        }

        // 通常移動
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Sword") && !isKnockback)
        {
            // ノックバック開始位置と終了位置を設定（右方向に一定距離）
            knockbackStartPos = transform.position;
            knockbackEndPos = knockbackStartPos + Vector2.right * knockbackDistance;
            knockbackTimer = 0f;
            isKnockback = true;
        }
    }
}
