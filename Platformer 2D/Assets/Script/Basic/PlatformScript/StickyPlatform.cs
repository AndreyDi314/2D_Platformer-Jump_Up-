using UnityEngine;

// Класс StickyPlatform отвечает за поведение клеящейся платформы, которая прилипает к игроку
public class StickyPlatform : MonoBehaviour
{
    // Ссылки на Rigidbody2D игрока и платформы
    private Rigidbody2D _playerRigidbody;
    private Rigidbody2D _platformRigidbody;

    // Переменная для отслеживания того, находится ли игрок на платформе
    private bool _isPlayerOnPlatform;

    // Ссылка на Animator игрока
    private Animator _playerAnimator;

    // Метод Start для инициализации компонентов
    private void Start()
    {
        // Получаем ссылки на Rigidbody2D и Animator игрока
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    // Метод FixedUpdate для передвижения игрока вместе с платформой
    private void FixedUpdate()
    {
        if (_isPlayerOnPlatform)
        {
            // Передвигаем игрока в позицию платформы
            _playerRigidbody.MovePosition(_platformRigidbody.position);

            // Проверяем, если скорость игрока больше 0.1f, включаем анимацию ходьбы
            if (_playerRigidbody.velocity.magnitude > 0.1f)
            {
                _playerAnimator.SetBool("isMoving", true);
            }
            else
            {
                // Иначе выключаем анимацию ходьбы
                _playerAnimator.SetBool("isMoving", false);
            }
        }
    }

    // Метод OnTriggerEnter2D для отслеживания столкновений с платформой
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если столкновение с платформой
        if (other.gameObject.CompareTag("Platform"))
        {
            // Получаем ссылку на Rigidbody2D платформы
            _platformRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            // Если ссылка не равна null, включаем флаг _isPlayerOnPlatform
            if (_platformRigidbody != null)
            {
                _isPlayerOnPlatform = true;
            }
        }
    }

    // Метод OnTriggerExit2D для отслеживания выхода из столкновения с платформой
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // Обнуляем ссылку на Rigidbody2D платформы и выключаем флаг _isPlayerOnPlatform
            _platformRigidbody = null;
            _isPlayerOnPlatform = false;
        }
    }
}