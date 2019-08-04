using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameSession : MonoBehaviour
{
    // config params
    [SerializeField] float[] gameSpeed;
    [SerializeField] int scorePerBlockDestroyed = 83;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] bool isAutoPlayEnabled;
    [SerializeField] List<GameObject> healthSprites;
    [SerializeField] TextMeshProUGUI speedText;

    // state variables
    [SerializeField] int currentScore = 0;
    [SerializeField] int currentHealth = 0;
    [SerializeField] int currentSpeed = 0;

    private void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if(gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        scoreText.text = currentScore.ToString();
        currentHealth = healthSprites.Count;
        currentSpeed = 0;
        Time.timeScale = gameSpeed[currentSpeed];
    }

    public void ChangeSpeed()
    {
        if((currentSpeed + 1) < gameSpeed.Length)
        {
            currentSpeed++;
        }
        else
        {
            currentSpeed = 0;

        }
        speedText.text = (currentSpeed + 1).ToString() + "x";
        Time.timeScale = gameSpeed[currentSpeed];
    }

    public void LoseOneLife()
    {
        if (!isAutoPlayEnabled)
        {
            healthSprites[currentHealth - 1].SetActive(false);
            currentHealth--;
            if (currentHealth == 0)
            {
                FindObjectOfType<SceneLoader>().LoadGameOverScreen();
            }
            else
            {
                ResetPaddleAndBall();
            }
        }
        else
        {
            ResetPaddleAndBall();
        }
    }

    public void GainOneLife()
    {
        if(currentHealth < healthSprites.Count)
        {
            healthSprites[currentHealth].SetActive(true);
            currentHealth++;
        }
    }

    private void ResetPaddleAndBall()
    {
        FindObjectOfType<Paddle>().ResetPosition();
        FindObjectOfType<Ball>().ResetPosition();
    }

    internal bool IsAutoPlayEnabled()
    {
        return isAutoPlayEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        /*Time.timeScale = gameSpeed;*/
    }

    public void AddToScore(int blockHitPoints)
    {
        currentScore += (scorePerBlockDestroyed * blockHitPoints);
        scoreText.text = currentScore.ToString();
    }

    public void DestroyItself()
    {
        Destroy(gameObject);
    }
}
