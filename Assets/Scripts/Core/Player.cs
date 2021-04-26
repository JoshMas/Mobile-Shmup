using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletInterval = 0.1f;
    private float bulletTimer = 0;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    private int touchOnLastFrame = 0;
#endif

    private Vector3 tapOrigin;
    private Vector3 playerOrigin;

    private Vector3 targetPlayerPosition;

    private SpriteRenderer sprite;
    private CircleCollider2D hitbox;

    [SerializeField]
    private AudioClip laser;
    [SerializeField]
    private AudioClip explosion;
    private AudioSource audioSource;

    private void Start()
    {
        CreateInstance();
        sprite = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        Input.multiTouchEnabled = false;
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        //IOS AND ANDROID HERE
        if(Input.touchCount > touchOnLastFrame)
            SetNewOrigin(Input.GetTouch(0).position);
        if (Input.touchCount > 0)
            MovePlayer(Input.GetTouch(0).position);
        else
            Fire();
        touchOnLastFrame = Input.touchCount;
#else

        if (Input.GetMouseButtonDown(0))
            SetNewOrigin(Input.mousePosition);
        if (Input.GetMouseButton(0))
            MovePlayer(Input.mousePosition);
        else
            Fire();
#endif
    }

    private void SetNewOrigin(Vector3 position)
    {
        tapOrigin = Camera.main.ScreenToWorldPoint(position);
        GameManager.Instance.multiplier = 1.0f;
        playerOrigin = gameObject.transform.position;
    }

    private void MovePlayer(Vector3 position)
    {
        Vector3 difference = tapOrigin - Camera.main.ScreenToWorldPoint(position);
        targetPlayerPosition = playerOrigin - difference;
        transform.position = targetPlayerPosition;
    }

    private void Fire()
    {
        bulletTimer += Time.deltaTime;
        GameManager.Instance.multiplier += Time.deltaTime;
        if(bulletTimer > bulletInterval)
        {
            bulletTimer = 0;
            BulletPool.Instance.CreateBullet(bullet, transform.position, Quaternion.Euler(0, 0, Random.Range(-10.0f, 10.0f)));
            audioSource.clip = laser;
            audioSource.Play();
        }
    }

    public void LoseHealth()
    {
        health--;
        GameManager.Instance.ReduceLives();
        audioSource.clip = explosion;
        audioSource.Play();
        if (health < 0)
        {
            GameManager.Instance.LoseGame();
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(nameof(DamageAnim));
        }
    }

    private IEnumerator DamageAnim()
    {
        hitbox.enabled = false;
        for(int i = 0; i < 8; ++i)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(0.125f);
        }
        hitbox.enabled = true;
    }
}
