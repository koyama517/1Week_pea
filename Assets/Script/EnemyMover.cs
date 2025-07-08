using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private float moveSpeed;

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Sword")
        {
            Destroy(gameObject);
        }
    }

}
