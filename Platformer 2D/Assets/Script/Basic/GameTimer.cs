using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ����� GameTimer �������� �� ������ ������� � ����.
public class GameTimer : MonoBehaviour
{
    // ����� ������ �������.
    private float startTime;
    // ����, �����������, ������� �� ������ �������.
    private bool isRunning = false;

    // ������ ���������� ��������, � ������� ������������ �����.
    public Text timerText;

    // ����� Start() ���������� ���� ��� � ������ ����.
    void Start()
    {
        // ��������� ������.
        startTime = Time.time;
        isRunning = true;
    }

    // ����� Update() ���������� ������ ����.
    void Update()
    {
        // ���� ������ �������, ��������� ��������� ����� � ���������� ��� �� ������.
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;

            // ����������� ����� � ���� "������:�������".
            string formattedTime = string.Format("{0:00}:{1:00}", Mathf.Floor(elapsedTime / 60), Mathf.RoundToInt(elapsedTime % 60));

            // ���������� ��������������� ����� �� ������.
            timerText.text = formattedTime;
        }
    }
}