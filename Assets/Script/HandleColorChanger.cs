using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HandleColorChanger : MonoBehaviour
{
    public Handle handle;
    public Color normalColor = Color.white;
    public Color attackColor = Color.red;
    public Color readyToFireColor = Color.yellow; // ���ˏ����𖞂������Ƃ��̐F

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
            // ���ˏ����𖞂����Ă���ꍇ�͉��F��
        }
        else
        {
            sr.color = normalColor;
        }
    }
}
