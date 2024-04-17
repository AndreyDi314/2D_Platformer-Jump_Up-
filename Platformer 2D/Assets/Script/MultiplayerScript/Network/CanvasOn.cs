using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� CanvasOn �������� �� ��������� ������� � ������ ����.
public class CanvasOn : MonoBehaviour
{
    // ������ �������, ������� ����� ������������.
    public GameObject Canvas;

    // ����� Start() ���������� ���� ��� � ������ ����.
    private void Start()
    {
        // ���������� ������.
        Canvas.SetActive(true);
    }
}