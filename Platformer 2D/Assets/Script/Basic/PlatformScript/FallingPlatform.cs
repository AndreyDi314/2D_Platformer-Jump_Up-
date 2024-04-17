using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� FallingPlatform �������� �� ��������� ���������, ������� ������ ����� �������� � �������
public class FallingPlatform : MonoBehaviour
{
    // ������ �� Rigidbody2D ��� ���������� ������� ���������
    private Rigidbody2D rb;

    // ���������� ��� �������� ��������� ������� � �������� ���������
    private Vector2 initialPosition;
    private Quaternion initialRotation;

    // ���������� ��� ���������� ������������ ��������� � �������� �������
    private bool platformMovingBack;
    private Quaternion targetRotation;
    private float rotationSpeed = 90f;

    // ��������� �������������
    private void Start()
    {
        // �������� ������ �� Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // ��������� ��������� ������� � �������� ���������
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // ������������� ������� �������� �� 90 �������� ������ ������������ ����������
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 90));
    }

    // ���������� �������� � ������� ��������� � Update
    private void Update()
    {
        if (platformMovingBack)
        {
            MovePlatformBack();
        }
    }

    // ����� ��� ����������� ��������� � �������� �������
    private void MovePlatformBack()
    {
        // ���������� ��������� � ��������� �������
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, 20f * Time.deltaTime);

        // ������� ��������� ������� � ��������� �������
        transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);

        // ���������, ������ �� ���������� ��������� � ��������� �������
        if (Mathf.Approximately(transform.position.y, initialPosition.y))
        {
            // �������� ����, ���������� ��������� � ��������� ������� � ��������
            platformMovingBack = false;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }

    // ����� ��� ��������� ������������ � �����������
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ��������� ������������ � ������� � ��� �� ����������������
        if (collision.gameObject.CompareTag("Player") && !platformMovingBack)
        {
            // ��������� ����� DropPlatform ����� 0.5 ������
            Invoke("DropPlatform", 0.5f);
        }
    }

    // ������������� ���������� ��� ���������� ������� ���������
    private void FixedUpdate()
    {
        // ���� ��������� ��� �� ������������� � �� ������������ � �������� �������
        if (!rb.isKinematic && !platformMovingBack)
        {
            // ������� ��������� � ������� ��������
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // ����� DropPlatform �������� �� ������� ��������� ����� ������������ � �������
    private void DropPlatform()
    {
        // ��������� isKinematic � Rigidbody2D, ����� ��������� ����� ����������� �������
        rb.isKinematic = false;

        // ��������� ����� GetPlatformBack ����� 1 �������
        Invoke("GetPlatformBack", 1f);
    }

    // ����� GetPlatformBack ���������� ��������� � ��������� ������� � ��������
    private void GetPlatformBack()
    {
        // �������� �������� ���������
        rb.velocity = Vector2.zero;

        // �������� isKinematic � Rigidbody2D, ����� ��������� ��������� ����������� �������
        rb.isKinematic = true;

        // ������������� ���� platformMovingBack � true, ����� ������ ����������� ��������� � �������� �������
        platformMovingBack = true;

        // ������������� ������� �������� ��������� �� 180 �������� ������������ ����������
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 180));
    }
}