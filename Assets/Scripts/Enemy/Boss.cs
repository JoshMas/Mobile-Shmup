using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This inherits from the core enemy class
/// It has multiple bullet patterns, and shifts between firing and resting
/// </summary>
public class Boss : EnemyCore
{
    /// <summary>
    /// This value is for counting bullets fired
    /// </summary>
    private int fireCount;
    /// <summary>
    /// How many bullets it fires before resting
    /// </summary>
    [SerializeField]
    private int restPoint;
    /// <summary>
    /// Whether it's resting or not
    /// </summary>
    private bool resting;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateHealth(health);
        GameManager.Instance.maxHealth = health;
        audioSource = GetComponent<AudioSource>();
        resting = false;
    }

    void Update()
    {
        Move();
        timer += Time.deltaTime;
        if (!resting)
        {
            //Run the normal firing loop if not resting, counting the bullets as you go
            if (timer > fireInterval)
            {
                timer = 0;
                Fire();
                ++fireCount;
            }
        }
        if(fireCount > restPoint)
        {
            //Do nothing until the rest period is over
            resting = true;
            timer += Time.deltaTime;
            if(timer > fireInterval * restPoint)
            {
                resting = false;
                fireCount = 0;
                timer = 0;
            }
        }
    }

    /// <summary>
    /// Fire a random bullet
    /// </summary>
    protected override void Fire()
    {
        if (!resting)
            BulletPool.Instance.CreateBullet(bullets[Random.Range(0, bullets.Length)], transform.position, transform.rotation);
        audioSource.clip = laser;
        audioSource.Play();
    }

    /// <summary>
    /// If not resting, move in front of the player
    /// This probably made the game too easy, oh well
    /// </summary>
    protected override void Move()
    {
        if(!resting)
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, Player.Instance.transform.position.x, 0.5f * Time.deltaTime), transform.position.y);
    }
}
