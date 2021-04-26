using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitting : BulletCore
{

    [SerializeField]
    private GameObject subBullet;
    [SerializeField]
    private int burstCount;
    [SerializeField]
    private float interval;
    private float originalInterval;

    private void Start()
    {
        originalInterval = interval;
    }

    protected override void MoveBullet()
    {
        transform.Translate(Vector2.up * localSpeed * Time.deltaTime);
        if(timer > interval)
        {
            interval += originalInterval;
            for(float i = 0; i < burstCount; ++i)
            {
                float angle = i / burstCount;
                BulletPool.Instance.CreateBullet(subBullet, transform.position, Quaternion.AngleAxis(angle * 360, Vector3.forward));
            }
        }
    }

    public override void ResetLifetime()
    {
        interval = originalInterval;
        base.ResetLifetime();
    }
}
