using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LogicScript : MonoBehaviour
{
    public int playerScore;
    private int over = 0;
    public Text scoreText;
    public GameObject gameOverScreen;
    public AudioSource eric;
    public AudioSource beethoven;
    public AudioSource theme;

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
        eric.Play();
    }

    public void restartGame()
    {
        over = 0;
        playerScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        if (!theme.isPlaying)
        {
            theme.Play();
        }
    }

    public void gameOver()
    {
        
        theme.Stop();
        if (!beethoven.isPlaying && (over!=1))
        {
            beethoven.Play();
        }
        over = 1;
        gameOverScreen.SetActive(true);
    }
    
}