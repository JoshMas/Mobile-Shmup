using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : BulletCore
{

    private float originalSpeed;
    [SerializeField]
    private float slowRate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, 0, Random.Range(0, 2) == 0 ? 90.0f : -90.0f);
        originalSpeed = localSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CountDown();
        localSpeed -= slowRate * Time.deltaTime;
        if(localSpeed < 0)
        {
            localSpeed = originalSpeed;
            Vector3 angleVector = transform.InverseTransformPoint(Player.Instance.transform.position);
            float angle = Mathf.Rad2Deg * Mathf.Atan2(angleVector.x, angleVector.y);
            transform.Rotate(0, 0, -angle);
        }
    }

    public override void ResetLifetime()
    {
        transform.Rotate(0, 0, Random.Range(0, 2) == 0 ? 90.0f : -90.0f);
        base.ResetLifetime();
    }
}
