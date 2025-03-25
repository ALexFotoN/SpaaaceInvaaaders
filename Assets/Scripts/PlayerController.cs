using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float screenBoundary = 8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    
    private float _nextFireTime = 0f;
    
    private void Start()
    {
        // Создаем точку стрельбы, если она не указана
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }
    
    private void Update()
    {
        MovePlayer();
        HandleShooting();
    }
    
    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var movement = new Vector3(horizontalInput, 0f, 0f);
        transform.position += movement * (moveSpeed * Time.deltaTime);
        
        // Ограничение движения по горизонтали
        var position = transform.position;
        position.x = Mathf.Clamp(position.x, -screenBoundary, screenBoundary);
        transform.position = position;
    }
    
    private void HandleShooting()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !(Time.time >= _nextFireTime)) return;
        Shoot();
        _nextFireTime = Time.time + fireRate;
    }
    
    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}