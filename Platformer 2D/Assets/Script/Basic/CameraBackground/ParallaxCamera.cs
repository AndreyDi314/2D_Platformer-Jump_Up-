using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �������� ����� ��� ���������� ���� ������� Update()  ������ ����, ���� � ���������.
[ExecuteInEditMode]

// ������� �������, ������� ����� ����������� ������ ���, ����� ������ ���������.
public delegate void ParallaxCameraDelegate(float deltaMovement);

// ����� ParallaxCamera, ����������� ������������ ���������.
public class ParallaxCamera : MonoBehaviour
{
    // ��������� ���� ��� ������� CameraTranslate, ������� ����� ���� ����������� � ������ ������.
    public ParallaxCameraDelegate onCameraTranslate;

    // ����������, � ������� ����������� ���������� ������� ������.
    private float oldPosition;

    void Start()
    {
        // � ������ ������������� �������� oldPosition ������ ������� ������� ������ �� �����������.
        oldPosition = transform.position.x;
    }

    void Update()
    {
        // ���� ������� ������ �� ����������� �� ����� ���������� �������,
        if (transform.position.x != oldPosition)
        {
            // �� ���������, �� ����� �� null ������� onCameraTranslate.
            if (onCameraTranslate != null)
            {
                // ���� ������� �� ����� null, �� �������� ��� � �������� ���������� �������� ����������� ������.
                float delta = oldPosition - transform.position.x;
                onCameraTranslate(delta);
            }

            // ����� ������ ��������� oldPosition ��� ���������� �����.
            oldPosition = transform.position.x;
        }
    }
}