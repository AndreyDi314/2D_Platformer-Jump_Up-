using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{
    // ���������� ���������� ��� Animator, SpriteRenderer, Player � Rigidbody2D �����������
    private Animator anim;
    private SpriteRenderer sprite;
    private Player player;
    private Rigidbody2D rb;

    // ����������� ������������ ��� ��������� ������������
    private enum MovementState { idle, running, jumping, falling }

    // ���������� ��� �������� �������� ��������� ������������
    private MovementState currentState;

    // ���������� ��� ����������� �������� ����� ��� X
    public Vector2 dirX;

    // �����, ���������� ����� ������ ���������� �����
    private void Start()
    {
        // ��������� ����������� Animator, SpriteRenderer, Player � Rigidbody2D
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // �����, ���������� ������ ����
    private void Update()
    {
        // ��������, ����� �� ������� ������ �������� (authority)
        if (hasAuthority)
        {
            // ���������� ����������� �������� ����� ��� X �� ������ ����� �������������� ���
            dirX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            // ����������� �������� ��������� ������������
            if (dirX.x > 0 || dirX.x < 0)
            {
                ChangeState(MovementState.running);
            }
            else
            {
                ChangeState(MovementState.idle);
            }

            // ����������� �������� ��������� ������ ��� �������
            if (rb.velocity.y > 0.1f)
            {
                ChangeState(MovementState.jumping);
            }
            else if (rb.velocity.y < -0.1f)
            {
                ChangeState(MovementState.falling);
            }
        }
    }

    // ����� ��� ��������� �������� ��������� ������������
    private void ChangeState(MovementState newState)
    {
        // ��������, ���������� �� ��������� ������������
        if (currentState != newState)
        {
            // ���������� �������� ��������� ������������
            currentState = newState;

            // ��������� �������� ��������� ������������ � Animator ����� ������������� ��������
            anim.SetInteger("state", (int)currentState);
        }
    }
}
