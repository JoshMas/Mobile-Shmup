using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This bullet's rotation and speed changes over time
/// </summary>
public class Spiralling : BulletCore
{

    [SerializeField]
    private float speedDifference = 10.0f;
    [SerializeField]
    private float rotationDifference = 45.0f;
    private float originalSpeed;
    private float originalRotation;

    private void Start()
    {
        originalSpeed = localSpeed;
        originalRotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CountDown();
        localSpeed += speedDifference * Time.deltaTime;
        rotation -= rotationDifference * Time.deltaTime;
    }

    public override void ResetLifetime()
    {
        base.ResetLifetime();
        localSpeed = originalSpeed;
        rotation = originalRotation;
    }
}
