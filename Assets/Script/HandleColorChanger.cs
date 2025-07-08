using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HandleColorChanger : MonoBehaviour
{
    public Handle handle;
    public Color normalColor = Color.white;
    public Color attackColor = Color.red;
    public Color readyToFireColor = Color.yellow; // ”­ËğŒ‚ğ–‚½‚µ‚½‚Æ‚«‚ÌF

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
            // ”­ËğŒ‚ğ–‚½‚µ‚Ä‚¢‚éê‡‚Í‰©F‚É
        }
        else
        {
            sr.color = normalColor;
        }
    }
}
