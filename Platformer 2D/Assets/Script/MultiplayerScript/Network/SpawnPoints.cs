using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    // ��������� ���� spawnPoints, ������� ����� ������� ��� ���������� SpawnPoints � �����.
    private SpawnPoints[] spawnPoints;

    private void Awake()
    {
        // ���� ��� ���������� SpawnPoints � �����.
        spawnPoints = FindObjectsOfType<SpawnPoints>();

        // ���� ���������� ����������� SpawnPoints ������ 1, �� ������� ������� ���������.
        if (spawnPoints.Length > 1)
        {
            Destroy(gameObject);
        }

        // ��������� ������� ��������� � ������ ����� ������� ����.
        DontDestroyOnLoad(gameObject);
    }
}