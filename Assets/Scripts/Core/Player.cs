using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script has the player's controls, and the behaviour of the player character
/// </summary>
public class Player : MonoSingleton<Player>
{
    [SerializeField]
    private int health = 3;

    /// <summary>
    /// The bullet the player shoots
    /// </summary>
    [SerializeField]
    private GameObject bullet;
    /// <summary>
    /// The time between shots
    /// </summary>
    [SerializeField]
    private float bulletInterval = 0.1f;
    private float bulletTimer = 0;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    /// <summary>
    /// The number of touchscreen inputs there were last frame, used for mobile input
    /// </summary>
    private int touchOnLastFrame = 0;
#endif

    /// <summary>
    /// The original position of the mouse or touchscreen input
    /// </summary>
    private Vector3 tapOrigin;
    /// <summary>
    /// The position of the player when the mouse/touch input began
    /// </summary>
    private Vector3 playerOrigin;

    /// <summary>
    /// The position the player should move to based on how far the mouse/finger is from its original position
    /// </summary>
    private Vector3 targetPlayerPosition;

    private SpriteRenderer sprite;
    private CircleCollider2D hitbox;

    /// <summary>
    /// Plays when it shoots a bullet
    /// </summary>
    [SerializeField]
    private AudioClip laser;
    /// <summary>
    /// Plays when a bullet hits it
    /// </summary>
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
        //Move the player if the mouse is down, otherwise, shoot
        if (Input.GetMouseButtonDown(0))
            SetNewOrigin(Input.mousePosition);
        if (Input.GetMouseButton(0))
            MovePlayer(Input.mousePosition);
        else
            Fire();
#endif
    }

    /// <summary>
    /// Sets the player and input's original positions when the mouse/finger first presses down
    /// </summary>
    /// <param name="position">The position of the input</param>
    private void SetNewOrigin(Vector3 position)
    {
        tapOrigin = Camera.main.ScreenToWorldPoint(position);
        GameManager.Instance.multiplier = 1.0f;
        playerOrigin = gameObject.transform.position;
    }

    /// <summary>
    /// Moves the player as long as the mouse/finger is held down
    /// </summary>
    /// <param name="position">Current input position</param>
    private void MovePlayer(Vector3 position)
    {
        Vector3 difference = tapOrigin - Camera.main.ScreenToWorldPoint(position);
        targetPlayerPosition = playerOrigin - difference;
        transform.position = targetPlayerPosition;
    }

    /// <summary>
    /// Shoots bullets whenever the player is stationary
    /// </summary>
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

    /// <summary>
    /// Lose one health when hit by a bullet
    /// The player is invincible for a short time after taking damage
    /// If there is no more health left, the player loses
    /// </summary>
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

    /// <summary>
    /// This handles the damage animation, and turns off the collider for the whole duration
    /// </summary>
    /// <returns></returns>
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
