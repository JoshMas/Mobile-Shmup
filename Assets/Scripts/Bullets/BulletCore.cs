using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basic script for the bullets
/// </summary>
public class BulletCore : MonoBehaviour
{
    /// <summary>
    /// Used to differentiate bullet types for the bullet pool
    /// </summary>
    public string id = "basic_bullet_0";
    [SerializeField]
    protected float localSpeed;
    [SerializeField]
    protected Vector2 worldSpeed;
    [SerializeField]
    protected float rotation;
    [SerializeField]
    protected float lifetime;
    /// <summary>
    /// Used to see if the bullet damages the boss, or the player
    /// If isAllied is true, it damages the boss
    /// </summary>
    [SerializeField]
    protected bool isAllied;

    protected float timer;

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CountDown();
    }

    /// <summary>
    /// Increments the timer
    /// If it surpasses the bullet's lifetime, remove the bullet
    /// </summary>
    protected virtual void CountDown()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Move and rotate the bullet based on its movement and rotation values
    /// </summary>
    protected virtual void MoveBullet()
    {
        transform.Translate(Vector2.up * localSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        transform.Translate(worldSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Sets the timer back to zero
    /// </summary>
    public virtual void ResetLifetime()
    {
        timer = 0;
    }

    /// <summary>
    /// Checks if it's collided with something it should be damaging
    /// If so, deal damage to it and deactivate
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAllied)
        {
            collision.gameObject.GetComponent<Player>().LoseHealth();
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Enemy") && isAllied)
        {
            EnemyCore enemyScript = collision.gameObject.GetComponent<EnemyCore>();
            if(enemyScript != null)
            {
                enemyScript.LoseHealth(10);
            }
            gameObject.SetActive(false);
        }
    }
}
