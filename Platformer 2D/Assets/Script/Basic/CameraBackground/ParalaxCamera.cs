using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� ParallaxCamera �������� �� �������� ������� ���������� � ������. ����������� � ������������ ����� (ExecuteInEditMode).
[ExecuteInEditMode]

public class ParalaxCamera : MonoBehaviour
{
    // ��������� ParallaxCamera, � �������� ���������� ������ ������.
    public ParallaxCamera parallaxCamera;

    // ������ ���� ParallaxLayer.
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    // ������� Start ���������� � ����� ������ ������ �������.
    void Start()
    {
        // ���� ������ ParallaxCamera �� ��������� � ������, �� ����������� ��� � ������� ������.
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // ���� ������ ParallaxCamera ������, �� ������������� �� ������� CameraTranslate.
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // �������������� ���� ParallaxLayer.
        SetLayers();
    }

    // ������� SetLayers �������������� ���� ParallaxLayer.
    void SetLayers()
    {
        // ������� ������ ����.
        parallaxLayers.Clear();

        // ���������� �� ���� �������� �������� �������� �������.
        for (int i = 0; i < transform.childCount; i++)
        {
            // �������� ��������� ParallaxLayer � �������� ��������� �������.
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // ���� ��������� ParallaxLayer ����������, �� ��������� ��� � ������ ����.
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    // ������� Move ���������� ���� ParallaxLayer.
    void Move(float delta)
    {
        // ���������� ��� ���� �� ������ ����.
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}