using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private TextMeshProUGUI healthText;
    private float health = 0.0f;
    public float maxHealth = 100000.0f;
    public float multiplier = 1.0f;

    [SerializeField]
    private TextMeshProUGUI livesText;
    private int lives = 3;

    [SerializeField]
    private Button menuButton;
    private Vector3 buttonPos;

    // Start is called before the first frame update
    void Start()
    {
        CreateInstance();
        UpdateHealth(0.0f);
        buttonPos = menuButton.transform.position;
        menuButton.transform.position = new Vector3(Camera.main.pixelWidth * 2, Camera.main.pixelHeight * 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(float increment)
    {
        health += increment * multiplier;
        healthText.text = "Health: " + (int)health;

        float colorVal = Mathf.Clamp(health / maxHealth, 0, 1);
        healthText.color = new Color(1.0f, colorVal, colorVal);
    }

    public void ReduceLives()
    {
        lives--;
        livesText.text = "Lives Remaining: " + lives;
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        livesText.text = "You Died";
        menuButton.transform.position = buttonPos;
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        healthText.text = "You Win!";
        menuButton.transform.position = buttonPos;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
