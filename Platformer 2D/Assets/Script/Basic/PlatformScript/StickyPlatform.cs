using UnityEngine;

// ����� StickyPlatform �������� �� ��������� ��������� ���������, ������� ��������� � ������
public class StickyPlatform : MonoBehaviour
{
    // ������ �� Rigidbody2D ������ � ���������
    private Rigidbody2D _playerRigidbody;
    private Rigidbody2D _platformRigidbody;

    // ���������� ��� ������������ ����, ��������� �� ����� �� ���������
    private bool _isPlayerOnPlatform;

    // ������ �� Animator ������
    private Animator _playerAnimator;

    // ����� Start ��� ������������� �����������
    private void Start()
    {
        // �������� ������ �� Rigidbody2D � Animator ������
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    // ����� FixedUpdate ��� ������������ ������ ������ � ����������
    private void FixedUpdate()
    {
        if (_isPlayerOnPlatform)
        {
            // ����������� ������ � ������� ���������
            _playerRigidbody.MovePosition(_platformRigidbody.position);

            // ���������, ���� �������� ������ ������ 0.1f, �������� �������� ������
            if (_playerRigidbody.velocity.magnitude > 0.1f)
            {
                _playerAnimator.SetBool("isMoving", true);
            }
            else
            {
                // ����� ��������� �������� ������
                _playerAnimator.SetBool("isMoving", false);
            }
        }
    }

    // ����� OnTriggerEnter2D ��� ������������ ������������ � ����������
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ������������ � ����������
        if (other.gameObject.CompareTag("Platform"))
        {
            // �������� ������ �� Rigidbody2D ���������
            _platformRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            // ���� ������ �� ����� null, �������� ���� _isPlayerOnPlatform
            if (_platformRigidbody != null)
            {
                _isPlayerOnPlatform = true;
            }
        }
    }

    // ����� OnTriggerExit2D ��� ������������ ������ �� ������������ � ����������
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // �������� ������ �� Rigidbody2D ��������� � ��������� ���� _isPlayerOnPlatform
            _platformRigidbody = null;
            _isPlayerOnPlatform = false;
        }
    }
}