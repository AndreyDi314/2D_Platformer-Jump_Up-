using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����, ����������� ��������������� ����� ���� ��� ������� ��������� ����.
public class SoundEffect : MonoBehaviour
{
    // �������������� ��� ��������������� �����.
    private AudioSource audioSource;

    // ���� ���������� �����.
    private bool isPlaying = false;

    // �����, ����������� ��� ������ ���������.
    private void Start()
    {
        // �������������� ��������������.
        audioSource = GetComponent<AudioSource>();

        // ������������� ��������������� �����.
        audioSource.Stop();
    }

    // �����, ����������� ������ ����.
    private void Update()
    {
        // ������� ��� ���� �� ����� �� ���� "Saw"
        foreach (GameObject sawObject in GameObject.FindGameObjectsWithTag("Saw"))
        {
            // ���� ���� ����� � ���� �� ������, �� ��������� ����.
            if (IsVisible(sawObject) && !isPlaying)
            {
                audioSource.Play();
                isPlaying = true;

                // ������� �� ����� foreach, ����� �� ��������� ���� ��������� ��� ������.
                return;
            }
        }

        // ���� ���� �������, � �� ���� ���� �� �����, ������������� ����.
        if (isPlaying && !IsAnySawVisible())
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    // �����, �����������, ����� �� ���� � ���� ������ ������.
    bool IsVisible(GameObject obj)
    {
        // ����� WorldToViewportPoint() ����������� ������� ������� �� ������� ��������� � ���������� ������.

        // ���� ���������� � ������� ������������ ������, ���������� true.
        Vector3 viewPos = Camera.main.WorldToViewportPoint(obj.transform.position);
        return viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
    }

    // �����, �����������, ����� �� ����� ���� �� �����.
    bool IsAnySawVisible()
    {
        // ������� ��� ���� �� ����� �� ���� "Saw"
        foreach (GameObject sawObject in GameObject.FindGameObjectsWithTag("Saw"))
        {
            // ���� ���� �����, ���������� true.
            if (IsVisible(sawObject))
            {
                return true;
            }
        }

        // ���� �� ���� ���� �� �����, ���������� false.
        return false;
    }
}