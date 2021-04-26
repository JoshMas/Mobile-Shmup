using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyCore
{
    private int fireCount;
    [SerializeField]
    private int restPoint;
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
            if (timer > fireInterval)
            {
                timer = 0;
                Fire();
                ++fireCount;
            }
        }
        if(fireCount > restPoint)
        {
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

    protected override void Fire()
    {
        if (!resting)
            BulletPool.Instance.CreateBullet(bullets[Random.Range(0, bullets.Length)], transform.position, transform.rotation);
        audioSource.clip = laser;
        audioSource.Play();
    }

    protected override void Move()
    {
        if(!resting)
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, Player.Instance.transform.position.x, 0.5f * Time.deltaTime), transform.position.y);
    }
}
