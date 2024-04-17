using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� CameraController �������� �� ������������ ������ �� ���������� � ����.
public class CameraController : MonoBehaviour
{
    // ��������� Transform ���� (���������), �� ������� ����� ��������� ������.
    Transform target;

    // ������, ���������� �� ����������� ������������ ������.
    Vector3 velocity = Vector3.zero;

    // �������� ��� ���������� ������������ ������������ ������.
    [Range(0, 1)]
    public float smoothTime;

    // ������ ������ �� �������.
    Vector3 positionOffset = new Vector3(0f, 5f, 10.19f);

    // ��������� ��� ����������� ������������ ������ �� ��� X � Y.
    [Header("Axis Limitation")]
    public Vector2 xLimit;
    public Vector2 yLimit;

    // ��������� GameObject ������.
    private GameObject player;

    // ������� Awake ���������� � ����� ������ ������ ���������. ������������ ��� ������������� ����������.
    private void Awake()
    {
        // ������� ������ �� ���� � ��������� ��� ��������� Transform.
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    // ������� FixedUpdate ���������� � ����������� ������ ����� ����������. ������������ ��� ������ � ������� � ������������� ������.
    private void FixedUpdate()
    {
        // ���� ������-���� ����������, �� ����������� ������.
        if (target != null)
        {
            // ��������� ������� ������� ������.
            Vector3 targetPosition = ClampTargetPosition(target.position + positionOffset);

            // ���������� ������������ ������ � ������� ������� SmoothDamp.
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    // ������� ClampTargetPosition ������������ ������������ ������ � �������� �������� ������ �� ��� X � Y.
    private Vector3 ClampTargetPosition(Vector3 targetPosition)
    {
        // ���������� ����� ������� ������ � �������������.
        return new Vector3(Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y), Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y), -10);
    }
}
