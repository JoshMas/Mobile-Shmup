using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : BulletCore
{
    private void Start()
    {
        transform.Rotate(0, 0, Random.Range(0, 2) == 0 ? 90.0f : -90.0f);
    }

    protected override void MoveBullet()
    {
        transform.Translate(Vector2.up * localSpeed * Time.deltaTime);
        Vector3 angleVector = Player.Instance.transform.InverseTransformPoint(transform.position);
        float targetRotation = Mathf.Rad2Deg * Mathf.Atan2(angleVector.y, angleVector.x) + 90.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(targetRotation, Vector3.forward), rotation * Time.deltaTime);
    }

    public override void ResetLifetime()
    {
        transform.Rotate(0, 0, Random.Range(0, 2) == 0 ? 90.0f : -90.0f);
        base.ResetLifetime();
    }
}
