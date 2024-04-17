using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �����, ����������� ���������������� �������� ����.
public class MainMenu : MonoBehaviour
{
    // �����, ����������� ���� � ��������� ������ NetworkManager.
    public void PlayGame()
    {
        // �������� ������ NetworkManager.
        GameObject networkManagerObject = GameObject.Find("NetworkManager");

        if (networkManagerObject != null)
        {
            // ������� ������ NetworkManager.
            Destroy(networkManagerObject);
        }

        // ��������� ����� � ��������������� 3.
        SceneManager.LoadScene(3);
    }

    // ����� ���������� ��� ������� �� ������, �� ������� �������� ���� ������.
    public void QuitGame()
    {
        // ��������� ����.
        Application.Quit();
    }
}