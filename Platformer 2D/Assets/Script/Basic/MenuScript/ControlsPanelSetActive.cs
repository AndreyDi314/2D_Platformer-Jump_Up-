using UnityEngine;
using UnityEngine.UI;

// —крипт отвечает за показ и скрытие панели с настройками управлени€.
public class ControlsPanelSetActive : MonoBehaviour
{
    // ѕанель, которую нужно показывать и скрывать.
    public GameObject panelObject;

    // ѕеременна€, котора€ хранит текущее состо€ние панели: активна она или нет.
    private bool isActive = true;

    // Ётот метод мен€ет состо€ние панели на противоположное.
    public void ControlsPanelActive()
    {
        // ћен€ем значение переменной isActive на противоположное.
        isActive = !isActive;

        // ”станавливаем видимость панели в зависимости от значени€ isActive.
        panelObject.SetActive(isActive);
    }
}