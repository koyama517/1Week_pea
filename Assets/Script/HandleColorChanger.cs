using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HandleColorChanger : MonoBehaviour
{
    public Handle handle;
    public Color normalColor = Color.white;
    public Color attackColor = Color.red;
    public Color readyToFireColor = Color.yellow; // 発射条件を満たしたときの色

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (handle == null) return;

        if (handle.isAttacking)
        {
            // 発射条件を満たしている場合は黄色に
        }
        else
        {
            sr.color = normalColor;
        }
    }
}
