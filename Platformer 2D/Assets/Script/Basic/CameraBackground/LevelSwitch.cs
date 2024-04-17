using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� LevelSwitch �������� �� ���������/���������� ������� � ����������� �� ����, ������ �� � ���� �����.
public class LevelSwitch : MonoBehaviour
{
    // ��������� GameObject ��������� �������.
    public GameObject activeFrame;

    // ������� OnTriggerEnter2D ����������, ����� ������ � ����������� ������ � �������.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, �������� �� ������, ��������� � �������, �������.
        if (other.CompareTag("Player"))
        {
            // �������� �������� ������.
            activeFrame.SetActive(true);
        }
    }

    // ������� OnTriggerExit2D ����������, ����� ������ � ����������� �������� �������.
    private void OnTriggerExit2D(Collider2D other)
    {
        // ���������, �������� �� ������, ���������� �������, �������.
        if (other.CompareTag("Player"))
        {
            // ��������� �������� ������.
            activeFrame.SetActive(false);
        }
    }
}