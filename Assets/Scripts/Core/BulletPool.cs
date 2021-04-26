using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoSingleton<BulletPool>
{

    List<BulletCore> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        CreateInstance();
        bulletPool = new List<BulletCore>();
    }

    public void CreateBullet(GameObject bullet, Vector3 position, Quaternion rotation)
    {
        BulletCore tempBullet = FindBullet(bullet.GetComponent<BulletCore>());
        if (tempBullet != null)
        {
            tempBullet.gameObject.SetActive(true);
            tempBullet.ResetLifetime();
            tempBullet.transform.position = position;
            tempBullet.transform.rotation = rotation;
        }
        else
        {
            GameObject newBullet = Instantiate(bullet, position, rotation, gameObject.transform);
            bulletPool.Add(newBullet.GetComponent<BulletCore>());
        }
    }

    private BulletCore FindBullet(BulletCore bulletToMatch)
    {
        foreach(BulletCore bullet in bulletPool)
        {
            if (bullet.id == bulletToMatch.id && !bullet.isActiveAndEnabled)
                return bullet;
        }
        return null;
    }
}
