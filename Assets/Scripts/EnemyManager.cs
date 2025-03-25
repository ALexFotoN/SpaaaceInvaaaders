using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Grid Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 11;
    [SerializeField] private float horizontalSpacing = 1f;
    [SerializeField] private float verticalSpacing = 0.7f;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveDownDistance = 0.5f;
    [SerializeField] private float timeBetweenSteps = 5f;
    [SerializeField] private float horizontalMoveDistance = 0.5f;
    
    [Header("Game Settings")]
    [SerializeField] private float defeatLineY = -4f;
    
    private readonly List<GameObject> _enemies = new List<GameObject>();
    private float _direction = 1f; // 1 = right, -1 = left
    private float _timeSinceLastStep = 0f;
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        SpawnEnemyGrid();
    }
    
    private void Update()
    {
        // Если игра не активна или нет врагов, пропускаем логику
        if (_gameManager && !_gameManager.IsGameActive() || _enemies.Count == 0)
            return;
            
        // Обновляем таймер движения
        _timeSinceLastStep += Time.deltaTime;
        
        // Пора делать следующий шаг?
        if (_timeSinceLastStep >= timeBetweenSteps)
        {
            MoveEnemies();
            _timeSinceLastStep = 0f;
        }
        
        // Проверяем условие поражения
        CheckDefeatCondition();
        
        // Проверяем условие победы
        CheckVictoryCondition();
    }
    
    private void SpawnEnemyGrid()
    {
        // Определяем начальную позицию для сетки врагов (центрирование)
        var startX = -(columns - 1) * horizontalSpacing / 2f;
        const float startY = 4f; // Начинаем сверху
        
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                var position = new Vector3(startX + col * horizontalSpacing, startY - row * verticalSpacing, 0f);
                var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                enemy.transform.parent = transform;
                _enemies.Add(enemy);
            }
        }
    }
    
    private void MoveEnemies()
    {
        // Проверяем, нужно ли менять направление
        var changeDirection = false;
        const float leftBound = -8f;
        const float rightBound = 8f;
        
        // Проверяем крайних врагов для смены направления
        foreach (var enemy in _enemies.Where(enemy => enemy))
        {
            if (_direction > 0 && enemy.transform.position.x >= rightBound - horizontalMoveDistance)
            {
                changeDirection = true;
                break;
            }

            if (!(_direction < 0) || !(enemy.transform.position.x <= leftBound + horizontalMoveDistance)) continue;
            changeDirection = true;
            break;
        }
        
        // Перемещаем всех врагов
        foreach (var enemy in _enemies.ToArray())
        {
            if (enemy)
            {
                if (changeDirection)
                {
                    // Двигаемся вниз при смене направления
                    enemy.transform.position += Vector3.down * moveDownDistance;
                }
                else
                {
                    // Двигаемся по горизонтали
                    enemy.transform.position += Vector3.right * (horizontalMoveDistance * _direction);
                }
            }
            else
            {
                // Удаляем null объекты (уничтоженных врагов)
                _enemies.Remove(enemy);
            }
        }
        
        // Меняем направление, если нужно
        if (changeDirection)
        {
            _direction *= -1;
        }
    }
    
    private void CheckDefeatCondition()
    {
        if (!_enemies.Any(enemy => enemy && enemy.transform.position.y <= defeatLineY)) return;
        _gameManager.GameOver();
    }
    
    private void CheckVictoryCondition()
    {
        if (_enemies.Count == 0)
        {
            _gameManager.Victory();
        }
    }
    
    public void ResetEnemies()
    {
        // Уничтожаем всех оставшихся врагов
        foreach (var enemy in _enemies.Where(enemy => enemy))
        {
            Destroy(enemy);
        }
        
        _enemies.Clear();
        _timeSinceLastStep = 0f;
        _direction = 1f;
        
        // Создаем новую сетку врагов
        SpawnEnemyGrid();
    }
}