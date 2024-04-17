using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� GroundChecker �������� �� ��������, ����� �� ����� �� ����� � �� ����� ���� ����������� �� �����.
public class GroundChecker : MonoBehaviour
{
    // ��������� ������.
    [SerializeField] private BoxCollider2D playerCollider;

    // ����� ���� �����, �� ������� ����� �������.
    [SerializeField] private LayerMask jumpableGround;

    // ����� ���� �����.
    [SerializeField] private LayerMask sandGround;

    // �������, ������� ���������, ����� �� ����� �� �����.
    public bool IsGrounded()
    {
        // ��������� ��������� ����� ������, ����� ��������� �������� ��������.
        float extraHeight = 0.1f;

        // ���������, ���� �� � �������� ��������� �����.
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, jumpableGround);

        // ���� ������ ��� ���������, ������ ����� ����� �� �����.
        return raycastHit.collider != null;
    }

    // �������, ������� ���������, ����� �� ����� �� �����.
    public bool IsSand()
    {
        // ��������� ��������� ����� ������, ����� ��������� �������� ��������.
        float extraHeight = 0.1f;

        // ���������, ���� �� � �������� ��������� �����.
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, sandGround);

        // ���� ������ ��� ���������, ������ ����� ����� �� �����.
        return raycastHit.collider != null;
    }

    // ������� OnCollisionEnter ����������, ����� ��������� ������� �������� �������� ������� ����������.
    private void OnCollisionEnter(Collision collision)
    {
        // ���� ������, � ������� ���������� �����, ����� ��� "���������", �� �� �������� ��� ������� ��������� ����.
        if (collision.gameObject.CompareTag("Platform"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // ���� ���� �������, �� ����� ��� ��� "��������������".
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    // ������� OnCollisionExit ����������, ����� ��������� ������� �������� �������� ������� ����������.
    private void OnCollisionExit(Collision collision)
    {
        // ���� ������, � ������� ���������� �����, ����� ��� "���������", �� �� �������� ��� ������� ��������� ����.
        if (collision.gameObject.CompareTag("Platform"))
        {
            // ���� ���� �������, �� ���������� ��� ��� �� "������������".
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
