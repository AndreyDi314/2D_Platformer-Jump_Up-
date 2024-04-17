using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �����, ����������� ���������������� ����� � ����.
public class PauseMenu : MonoBehaviour
{
    // ����� � ������.
    public GameObject pauseGameMenu;

    // �����, �����������, ������ �� ������� Esc.
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �������� ��� ��������� ����� � ����������� �� �������� ���������.
            pauseGameMenu.SetActive(!pauseGameMenu.activeSelf);
        }
    }

    // �����, ���������� ����� �������� ������.
    private void OnEnable()
    {
        // ��������� �������, ������� ����� ���������� ����� �������� ������
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // �����, ���������� ��� ���������� �������
    private void OnDisable()
    {
        // ������� �������, ����� �� ������ �� ���������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ����� ���������� ������ ���, ����� ����������� ����� �����.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called");
        // ���������, �������� �� ����������� ����� ������� ����.
        if (scene.buildIndex == 0)
        {
            // ������� ��� ������� �� ��������� ���� "Destroy".
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Destroy");
            // ���������� �� ���� ��������� ��������.
            foreach (GameObject obj in objectsToDestroy)
            {
                Debug.Log($"Destroying object: {obj.name}");
                // ���������� ������ ������.
                Destroy(obj);
            }

            // ��������� ��������, ������� ��������� ������� ���� � ��������� � 1 �������.
            StartCoroutine(LoadMenuAfterDelay(1.0f));
        }
    }

    // ��������, ����������� ������� ���� ����� ��������� ��������
    IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadMenu();
    }

    // �����, ����������� ������� ����.
    public void LoadMenu()
    {
        // ��������� ����� Menu.
        SceneManager.LoadScene(0);
    }
}