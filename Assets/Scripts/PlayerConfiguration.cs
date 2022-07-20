using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConfiguration : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float scoreAddQuotient;
    private int _currentHealth;
    private int _newHealth;
    private float _currentScore;
    private int _newScore;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private SpawnController spawnController;

    private bool _stop;

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
        scoreText.text = Mathf.RoundToInt(_currentScore).ToString();
    }

    private void Update()
    {
        UpdateScore();
    }
}
