using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] float MinSpeed = 30f;
    [SerializeField] float MaxSpeed = 60f;

    float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = Random.RandomRange(MinSpeed, MaxSpeed);
        transform.Rotate(0f, Random.Range(0f, 360f), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, Speed * Time.deltaTime, 0f);
    }
}
