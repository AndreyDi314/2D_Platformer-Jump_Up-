using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����� ��� ����, ����� ������, �� ������� �� �����, �� ����������� ��� �������� ����� �����.
public class DontDestroy : MonoBehaviour
{
    // ���� ����� ���������� ����� ����� �������� �������.
    void Awake()
    {
        // ������� ��� ������� �� �������� DontDestroy � �����.
        DontDestroy[] dontDestroyObjects = FindObjectsOfType<DontDestroy>();

        // ���� ����� �������� ������ ������, ������, ���� ������ ��� ����� �� ������ �������, � �� ����� ��� �������, ����� �������� ������������.
        if (dontDestroyObjects.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // ���� �� ����� �������� ���, ������, ��� ������ ������ � ����� ��������, � �� ������ ��������� ��� ��� �������� ����� ����.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}