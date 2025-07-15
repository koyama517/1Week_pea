using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    [SerializeField] private float speed = 1f;       
    [SerializeField] private float range = 0.5f;      

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}

