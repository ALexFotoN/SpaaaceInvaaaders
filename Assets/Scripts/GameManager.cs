using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Text finalScoreText;
    
    [Header("Game Settings")]
    [SerializeField] private int scorePerEnemy = 10;
    
    private int _currentScore = 0;
    private bool _isGameActive = true;
    private PlayerController _player;
    private EnemyManager _enemyManager;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemyManager = FindObjectOfType<EnemyManager>();
        ResetGame();
    }
    
    private void Update()
    {
        // Перезапуск игры по кнопке R
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }
    
    public void AddScore(int points)
    {
        if (!_isGameActive) return;
        
        _currentScore += points;
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + _currentScore;
    }
    
    public void GameOver()
    {
        if (!_isGameActive) return;
        
        _isGameActive = false;
        gameOverPanel.SetActive(true);
    }
    
    public void Victory()
    {
        if (!_isGameActive) return;
        
        _isGameActive = false;
        finalScoreText.text = "Final Score: " + _currentScore;
        victoryPanel.SetActive(true);
    }

    private void ResetGame()
    {
        _currentScore = 0;
        UpdateScoreUI();
        
        _isGameActive = true;
        
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        
        if (_enemyManager)
        {
            _enemyManager.ResetEnemies();
        }
    }
    
    public bool IsGameActive()
    {
        return _isGameActive;
    }
}