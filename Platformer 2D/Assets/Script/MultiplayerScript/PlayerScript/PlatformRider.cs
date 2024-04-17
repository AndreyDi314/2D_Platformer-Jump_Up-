using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformRider : MonoBehaviour
{
    // ��������� ������� ���� isRidingPlatform, ������� ���������, ����� �� �������� �� ��������� ��� ���.
    private bool isRidingPlatform = false;

    // ��������� ���� platformPositionOffset, ������� ������ �������� ��������� ������������ ������� ���������.
    private Vector3 platformPositionOffset;

    // ��������� ���� currentPlatformCollider, ������� ������ ������� ��������� ���������, �� ������� ����� ��������.
    private Collider2D currentPlatformCollider;

    // ����� OnTriggerEnter2D() ����������, ����� �������� ������ � ���������� ���� ������� ����������.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ���������, � ������� ���������� ��������, ����� ��� "Platform", �� ������������� �������� isRidingPlatform � true.
        if (collision.CompareTag("Platform"))
        {
            isRidingPlatform = true;
            platformPositionOffset = transform.position - collision.transform.position;
            currentPlatformCollider = collision;
        }
    }

    // ����� OnTriggerExit2D() ����������, ����� �������� ������� �� ���������� ���� ������� ����������.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ���� ���������, � ������� ���������� ��������, ����� ��� "Platform", �� ������������� �������� isRidingPlatform � false.
        if (collision.CompareTag("Platform"))
        {
            isRidingPlatform = false;
            currentPlatformCollider = null;
        }
    }

    // ����� FixedUpdate() ���������� � ������������� ������� � ������������ ��� ������ � ������ ��������.
    private void FixedUpdate()
    {
        // ���� �������� isRidingPlatform ����� true � currentPlatformCollider �� ����� null, �� ���������� ��������� ������ � ����������.
        if (isRidingPlatform && currentPlatformCollider != null)
        {
            transform.position = currentPlatformCollider.transform.position + platformPositionOffset;
        }
    }
}