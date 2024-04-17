using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerSinglePlayer : MonoBehaviour
{
    // ��������� Rigidbody2D, ����������� ��� ����������� ������������ �������.
    private Rigidbody2D rb;

    // ��������� BoxCollider2D, ������������ ��� �������� ������������ � ������� ���������.
    private BoxCollider2D coll;

    // ����, �����������, � ����� ����������� �������� ������� � ������ ������.
    bool facingRight = true;

    // ��������� ��� �������� ������������ � ���������� ��� �������.
    [Header("Collision Ground Settings")]
    [SerializeField] private LayerMask jumpableGround; // ����� ���� ��� �������� ��� �������.
    [SerializeField] private float groundAcceleration = 10f; // ��������� ��� ������������ �� �������� ��� �������.
    [SerializeField] private float groundDeceleration = 5f; // ���������� ��� ��������� �� �������� ��� �������.
    [SerializeField] private float groundjumpForce = 35f; // ���� ������ � �������� ��� �������.

    // ��������� ��� �������� ������������ � ������.
    [Header("Collision Sand Settings")]
    [SerializeField] private LayerMask sandGround; // ����� ���� ��� �����.
    [SerializeField] private float sandAcceleration = 5f; // ��������� ��� ������������ �� �����.
    [SerializeField] private float sandDeceleration = 10f; // ���������� ��� ��������� �� �����.
    [SerializeField] private float sandjumpForce = 30f; // ���� ������ � �����.

    // ��������� ��� ������ ���������� ������������.
    [Header("Other Settings")]
    [SerializeField] private float airAcceleration = 2f; // ��������� ��� �������� � �������.
    [SerializeField] private float maxSpeed = 10f; // ������������ �������� ������������.
    [SerializeField] private GroundChecker groundChecker; // ��������� ��� �������� ������������ � ����������.

    // ��������������� ���������� ��� �������� ����������� � �������� ������������.
    private Vector2 dirX;
    private bool isGrounded; // ����, �����������, ����� �� �������� �� �������� ��� �������.
    private bool isSand; // ����, �����������, ����� �� �������� �� �����.
    private float basicAcceleration; // ������� �������� ���������.
    private float basicDeceleration; // ������� ����������.
    private float jumpForce; // ���� ������.
    private bool jumpInProgress = false; // ����, �����������, ���������� �� � ������ ������ ������.

    // ����������� ���� ��� ��� ������� �����. ������������ ��� ������������� �����������.
    private void Awake()
    {
        // ��������� ���������� Rigidbody2D.
        rb = GetComponent<Rigidbody2D>();
        // ��������� ���������� GroundChecker.
        groundChecker = GetComponent<GroundChecker>();
    }

    // ����������� ������ ����.
    private void Update()
    {
            //��������� ���������� dirX � ����������� �� �������� Input.GetAxisRaw("Horizontal")
            dirX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            //����������, ��������� �� ����� �� ����� ��� �� �����
            isGrounded = groundChecker.IsGrounded();
            isSand = groundChecker.IsSand();

            if (isGrounded)
            {
                jumpInProgress = false;
                basicAcceleration = groundAcceleration;
                basicDeceleration = groundDeceleration;
                jumpForce = groundjumpForce;
            }

            if (isSand)
            {
                jumpInProgress = false;
                basicAcceleration = sandAcceleration;
                basicDeceleration = sandDeceleration;
                jumpForce = sandjumpForce;
            }

            //�������� ������ HandleMovement � HandleJumping ��� ��������� ������������ � �������
            HandleMovement();
            HandleJumping();

            //������������ ��������������� ��������� � ����������� �� ����������� ��������
            if (!facingRight && dirX.x > 0)
            {
                Flip();
            }
            else if (facingRight && dirX.x < 0)
            {
                Flip();
            }
    }

    //����� ���������� ��������� ���������
    private void HandleMovement()
    {
        // ���������� ��������� ����������� ��� ����, � ����� ������ �������� ������ ��������� ��������: �� ����� ��� � �������
        float acceleration = isGrounded ? basicAcceleration : airAcceleration;
        float deceleration = basicDeceleration;

        if (dirX.x != 0)
        {
            // ���������� ������� �������� �������� ����� ��� x � ����������� �� ����������� ��������
            float targetSpeed = dirX.x * maxSpeed;

            // � ����������� �� ������� � ������� ��������, ��������� ������������� �������� ��������� �� ������ ����� � ������� ������������
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, Time.deltaTime * acceleration), rb.velocity.y);
        }
        else if (isGrounded)
        {
            // ���� �������� ����� �� ����� � ����� �� �����, �� �������������� ��� �� ������� ��������
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, Time.deltaTime * deceleration), rb.velocity.y);
        }
    }

    //����� ���������� ������� ���������
    private void HandleJumping()
    {
        // ����������, ��������� �� ������� ������ "������" � ���� ��, ��� � ��������� �� ���,
        // ����� �������� ��������� �� ����� ��� � �����
        if (Input.GetButtonDown("Jump") && (isGrounded || isSand) && !jumpInProgress)
        {
            // ��������� ���� �����, ����� �������� ���������
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // ��������, ��� ������ ���� ������
            jumpInProgress = true;
        }
    }

    //����� ���������� ������� ���������
    private void Flip()
    {
            // ������ ����������� �������� ���������, ���� �� ������� ����������� ��������
            facingRight = !facingRight;

            // ������ ������� ���������, ����� �� �������� � ������ �������
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
    }
}