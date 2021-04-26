using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCore : MonoBehaviour
{
    public string id = "basic_bullet_0";
    [SerializeField]
    protected float localSpeed;
    [SerializeField]
    protected Vector2 worldSpeed;
    [SerializeField]
    protected float rotation;
    [SerializeField]
    protected float lifetime;
    [SerializeField]
    protected bool isAllied;

    protected float timer;

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        CountDown();
    }

    protected virtual void CountDown()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void MoveBullet()
    {
        transform.Translate(Vector2.up * localSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        transform.Translate(worldSpeed * Time.deltaTime, Space.World);
    }

    public virtual void ResetLifetime()
    {
        timer = 0;
    }

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
