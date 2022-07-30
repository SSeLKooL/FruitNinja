using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerConfiguration : MonoBehaviour
{
    [SerializeField] private int startHealth;
    [SerializeField] private float scoreAddQuotient;
    [SerializeField] private float minScoreSpeed;

    private float _currentScoreSpeed;
    private int _currentHealth;
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

    private bool _stop;

    private float _bestScore;

    public bool CheckGameStatus()
    {
        return _stop;
    }

    private void GameOver()
    {
        _stop = true;
        spawnController.Stop();
        touchTrail.emitting = false;
        
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
            resultBestScoreText.text = Best + _bestScore;
        }

        gameOverScreen.ShowGameOverScreen();
    }

    private void Start()
    {
        _currentHealth = startHealth;
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
        if (!_stop)
        {
            _newScore += points;
        }
    }

    public void HitPlayer()
    {
        if (!_stop)
        {
            _newHealth--;
            
            if (_newHealth < _currentHealth)
            {
                _currentHealth--;
                healthBar.RemoveHeart();
            }

            if (_newHealth == 0)
            {
                GameOver();
            }
        }
    }

    private void UpdateScore()
    {
        _currentScoreSpeed = Math.Max((_newScore - _currentScore) * scoreAddQuotient, minScoreSpeed);

        if (_currentScore < _newScore)
        {
            _currentScore += _currentScoreSpeed * Time.deltaTime;
        }
        
        if (_newScore > _bestScore)
        {
            _bestScore = _currentScore;
        }
        
        _scoreText.text = Mathf.RoundToInt(_currentScore).ToString();
        _bestScoreText.text = Best + Mathf.RoundToInt(_bestScore);
        
    }

    private void Update()
    {
        UpdateScore();
    }
}
