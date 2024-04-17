using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Класс VolumeManager отвечает за управление громкостью звука в игре.
public class VolumeManager : MonoBehaviour
{
    // Название параметра громкости в микшере
    public string volumeParametr = "MasterVolume";

    // Ссылка на аудио микшер
    public AudioMixer mixer;

    // Ползунок для регулировки громкости
    public Slider slider;

    // Текстовое поле для отображения текущего значения громкости
    public Text sliderValueText;

    // Вспомогательное поле для хранения текущего значения громкости
    private float _volumeValue;

    // Константа для преобразования значения громкости в децибелы
    private const float _multiplier = 20f;


    private void Awake()
    {
        // Подписываемся на событие изменения значения ползунка
        slider.onValueChanged.AddListener(HandleSliderValueChanged);

    }

    private void HandleSliderValueChanged(float value)
    {
        // Преобразуем значение ползунка в децибелы и записываем его в микшер
        _volumeValue = Mathf.Log10(value) * _multiplier;
        mixer.SetFloat(volumeParametr, _volumeValue);

        // Обновляем текстовое поле с текущим значением громкости
        sliderValueText.text = value.ToString("F1");
    }

    void Start()
    {
        Debug.Log("VolumeManager: Start called.");
        // Получаем текущее значение громкости из PlayerPrefs
        _volumeValue = PlayerPrefs.GetFloat(volumeParametr, Mathf.Log10(slider.value) * _multiplier);
        // Устанавливаем значение ползунка в соответствии с текущим значением громкости
        slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
        // Обновляем текстовое поле с текущим значением громкости
        sliderValueText.text = slider.value.ToString("F1");
    }

    private void OnDisable()
    {
        // Сохраняем текущее значение громкости в PlayerPrefs при отключении скрипта
        PlayerPrefs.SetFloat(volumeParametr, _volumeValue);
    }
}