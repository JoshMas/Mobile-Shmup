using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The main script that manages the overall game
/// Includes UI and win/lose states
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    /// <summary>
    /// This value is unused
    /// </summary>
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private TextMeshProUGUI healthText;
    private float health = 0.0f;
    public float maxHealth = 100000.0f;
    /// <summary>
    /// This value increases the longer the player stands still
    /// The higher thsi value is, the more damage their bullets deal
    /// </summary>
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

    /// <summary>
    /// Changes the value of the boss' health text
    /// The lower it is, the more red it is
    /// </summary>
    /// <param name="increment">The amount the health is adjusted by</param>
    public void UpdateHealth(float increment)
    {
        health += increment * multiplier;
        healthText.text = "Health: " + (int)health;

        float colorVal = Mathf.Clamp(health / maxHealth, 0, 1);
        healthText.color = new Color(1.0f, colorVal, colorVal);
    }

    /// <summary>
    /// Reduces the value of the player's health text
    /// </summary>
    public void ReduceLives()
    {
        lives--;
        livesText.text = "Lives Remaining: " + lives;
    }

    /// <summary>
    /// Stops the game, gives the player a loss, and reveals the button to go to the main menu
    /// </summary>
    public void LoseGame()
    {
        Time.timeScale = 0;
        livesText.text = "You Died";
        menuButton.transform.position = buttonPos;
    }

    /// <summary>
    /// Stops the game, gives the player a win, and reveals the button to go to the main menu
    /// </summary>
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
