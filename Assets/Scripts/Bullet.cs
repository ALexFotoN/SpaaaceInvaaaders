using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    
    private void Start()
    {
        // Уничтожить пулю через lifetime секунд
        Destroy(gameObject, lifetime);
    }
    
    private void Update()
    {
        // Движение пули вверх
        transform.Translate(Vector2.up * (speed * Time.deltaTime));
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка столкновения с врагом
        if (!other.CompareTag("Enemy")) return;
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}