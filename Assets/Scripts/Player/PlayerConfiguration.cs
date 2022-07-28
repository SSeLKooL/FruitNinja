using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerConfiguration : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float scoreAddQuotient;
    private int _currentHealth;
    private int _newHealth;
    private float _currentScore;
    private int _newScore;
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameObject bestScoreObject;
    [SerializeField] private GameObject scoreObject;
    private TextMeshProUGUI _bestScoreText;
    private TextMeshProUGUI _scoreText;
    
    [SerializeField] private SpawnController spawnController;

    private string _best = "лучший: ";

    private bool _stop;

    private float _bestScore;

    private void GameOver()
    {
        _stop = true;
        spawnController.Stop();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        _newHealth = maxHealth;
        healthBar.CreateHealthBar(maxHealth);

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

        _bestScoreText.text = _best + _bestScore;
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
        _currentScore = math.lerp(_currentScore, _newScore, scoreAddQuotient);
        
        if (_newScore > _bestScore)
        {
            _bestScore = math.lerp(_bestScore, _newScore, scoreAddQuotient);
            PlayerPrefs.SetInt("RecordScore", _newScore);
            PlayerPrefs.Save();
        }
        
        _scoreText.text = Mathf.RoundToInt(_currentScore).ToString();
        _bestScoreText.text = _best + Mathf.RoundToInt(_bestScore);
        
    }

    private void Update()
    {
        UpdateScore();
    }
}
