using UnityEngine;

/// <summary>
/// The base class for enemies
/// While there is only one enemy (the boss), there were plans to have waves of enemies instead
/// </summary>
public class EnemyCore : MonoBehaviour
{
    [SerializeField]
    protected float health = 10;
    /// <summary>
    /// The different types of bullets it fires
    /// The base class only uses the first one
    /// </summary>
    [SerializeField]
    protected GameObject[] bullets;
    /// <summary>
    /// The score earned by eliminating this enemy
    /// This value went unused
    /// </summary>
    [SerializeField]
    protected float score = 10;
    /// <summary>
    /// The amount of time between bullets
    /// </summary>
    [SerializeField]
    protected float fireInterval;
    protected float timer;

    /// <summary>
    /// Play this sound when shooting a bullet
    /// </summary>
    [SerializeField]
    protected AudioClip laser;
    /// <summary>
    /// Play this sound when hit
    /// </summary>
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

    /// <summary>
    /// Shoot the first bullet in the bullet array
    /// </summary>
    protected virtual void Fire()
    {
        Instantiate(bullets[0], gameObject.transform);
    }

    /// <summary>
    /// The enemy's movement pattern
    /// This is empty for teh basic enemy
    /// </summary>
    protected virtual void Move()
    {
    }

    /// <summary>
    /// Reduces this enemy's health
    /// If it's reduced to zero, set the game's state to victory
    /// (Because there's only one enemy)
    /// </summary>
    /// <param name="damage">How much health to deduct</param>
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
