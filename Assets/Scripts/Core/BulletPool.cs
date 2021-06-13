using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The object pool for the bullets
/// </summary>
public class BulletPool : MonoSingleton<BulletPool>
{

    List<BulletCore> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        CreateInstance();
        bulletPool = new List<BulletCore>();
    }

    /// <summary>
    /// Gets the required bullet from the object pool if it exists
    /// Otherwise, it instantiates a new bullet and adds it to the pool
    /// </summary>
    /// <param name="bullet">The type of bullet needed</param>
    /// <param name="position">The position to spawn it at</param>
    /// <param name="rotation">The rotation to spawn it at</param>
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

    /// <summary>
    /// Finds a bullet in the pool
    /// </summary>
    /// <param name="bulletToMatch">The type of bullet needed</param>
    /// <returns>The first matching bullet, or null if it doesn't exist</returns>
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
