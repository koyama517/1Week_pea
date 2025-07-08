using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 moveSpeed;
    Vector2 HP;

    Rigidbody2D rb;

    public bool isStep;

    Vector2 stepPower;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (isStep) { rb.velocity = stepPower; }
    
    }
}