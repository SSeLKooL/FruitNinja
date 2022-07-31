using System;
using TMPro;
using UnityEngine;

public class PlayerConfiguration : MonoBehaviour
{
    [SerializeField] private int startHealth;
    [SerializeField] private float scoreAddQuotient;
    [SerializeField] private float minScoreSpeed;

    [SerializeField] private float freezeTime;
    [SerializeField] private float freezeQuotient;

    [SerializeField] private GameObject freezeScreen;

    private float _currentFreezeTime;
    private bool _isFreeze;

    private float _currentScoreSpeed;
    private int _newHealth;
    private float _currentScore;
    private int _newScore;
    
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameObject bestScoreObject;
    [SerializeField] private GameObject scoreObject;
    
    private TextMeshProUGUI _bestScoreText;
    private TextMeshProUGUI _scoreText;
    
    [SerializeField] private TextMeshProUGUI resultScoreText;
    [SerializeField] private TextMeshProUGUI resultBestScoreText;

    [SerializeField] private TrailRenderer touchTrail;
    
    [SerializeField] private SpawnController spawnController;

    [SerializeField] private GameOverScreen gameOverScreen;

    private const string Best = "лучший: ";

    public bool stop;

    private float _bestScore;

    public void FreezeTime()
    {
        Time.timeScale = 1 - freezeQuotient;
        freezeScreen.SetActive(true);
        _currentFreezeTime = 0;
        _isFreeze = true;
    }

    private void GameOver()
    {
        stop = true;
        spawnController.Stop();
        touchTrail.emitting = false;
        StopFreeze();
        
        if (_newScore > _bestScore)
        {
            PlayerPrefs.SetInt("RecordScore", _newScore);
            PlayerPrefs.Save();
            resultScoreText.text = _newScore.ToString();
            resultBestScoreText.text = Best + _newScore;
        }
        else
        {
            resultScoreText.text = _newScore.ToString();
            resultBestScoreText.text = Best + Mathf.RoundToInt(_bestScore);
        }

        gameOverScreen.ShowGameOverScreen();
    }

    private void Start()
    {
        _newHealth = startHealth;
        healthBar.CreateHealthBar(startHealth);

        _bestScoreText = bestScoreObject.GetComponent<TextMeshProUGUI>();
        _scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
        
        if (PlayerPrefs.HasKey("RecordScore"))
        {
            _bestScore = PlayerPrefs.GetInt("RecordScore");
        }
        else
        {
            _bestScore = 0;
        }

        _bestScoreText.text = Best + _bestScore;
    }

    public void AddScorePoints(int points)
    {
        if (!stop)
        {
            _newScore += points;
        }
    }

    public void HitPlayer()
    {
        if (!stop)
        {
            _newHealth--;

            healthBar.RemoveHeart();

            if (_newHealth == 0)
            {
                GameOver();
            }
        }
    }

    public void HealPlayer()
    {
        if (!stop)
        {
            _newHealth++;

            healthBar.AddHealth(1);
        }
    }

    private void UpdateScore()
    {
        if (_currentScore < _newScore)
        {
            _currentScoreSpeed = Math.Max((_newScore - _currentScore) * scoreAddQuotient, minScoreSpeed);
            _currentScore += _currentScoreSpeed * Time.deltaTime;

            if (_currentScore > _newScore)
            {
                _currentScore = _newScore;
            }
        }

        if (_newScore > _bestScore)
        {
            _bestScore = _currentScore;
        }
        
        _scoreText.text = Mathf.RoundToInt(_currentScore).ToString();
        _bestScoreText.text = Best + Mathf.RoundToInt(_bestScore);
        
    }

    private void StopFreeze()
    {
        _isFreeze = false;
        freezeScreen.SetActive(false);
        Time.timeScale = 1;
    }
    
    private void CheckFreezeTime()
    {
        if (_isFreeze)
        {
            _currentFreezeTime += Time.deltaTime / freezeQuotient;

            if (_currentFreezeTime > freezeTime)
            {
                StopFreeze();
            }
        }
    }

    private void Update()
    {
        UpdateScore();
        CheckFreezeTime();
    }
}
