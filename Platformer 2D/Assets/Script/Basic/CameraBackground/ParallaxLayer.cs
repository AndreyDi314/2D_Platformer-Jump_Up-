using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������, ����������� ��� ���������� ������� � ��������� � ������ ��������� �������.
[ExecuteInEditMode]

// ����� ParallaxLayer, ����������� ������������ ���������.
public class ParallaxLayer : MonoBehaviour
{
    // �������� ��� ������� ������������ ������������� ����������.
    public float parallaxFactor;

    // ��������� ������� ������� � ������������ � ������������ ���������.
    public void Move(float delta)
    {
        // ������� ����� ������� �� ������ ������� ��������� ������� �������.
        Vector3 newPos = transform.localPosition;

        // ������������� ������� ������� � ����������� �� �������� ��������� parallaxFactor. ��� ������ �������� ���������, ��� ������ �������� ����� �� ������.
        newPos.x -= delta * parallaxFactor;

        // ��������� ��������� �������� �������.
        transform.localPosition = newPos;
    }
}