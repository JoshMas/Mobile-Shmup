using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [SerializeField]
    protected float health = 10;
    [SerializeField]
    protected GameObject[] bullets;
    [SerializeField]
    protected float score = 10;
    [SerializeField]
    protected float fireInterval;
    protected float timer;

    [SerializeField]
    protected AudioClip laser;
    [SerializeField]
    protected AudioClip explosion;
    protected AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateHealth(health);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        timer += Time.deltaTime;
        if(timer > fireInterval)
        {
            timer = 0;
            Fire();
        }
    }

    protected virtual void Fire()
    {
        Instantiate(bullets[0], gameObject.transform);
    }

    protected virtual void Move()
    {
    }

    public void LoseHealth(int damage)
    {
        health -= damage * GameManager.Instance.multiplier;
        audioSource.clip = explosion;
        audioSource.Play();
        GameManager.Instance.UpdateHealth(-damage);
        if(health <= 0)
        {
            GameManager.Instance.WinGame();
            gameObject.SetActive(false);
        }
    }
}
