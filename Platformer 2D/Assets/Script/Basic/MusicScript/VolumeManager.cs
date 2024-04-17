using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// ����� VolumeManager �������� �� ���������� ���������� ����� � ����.
public class VolumeManager : MonoBehaviour
{
    // �������� ��������� ��������� � �������
    public string volumeParametr = "MasterVolume";

    // ������ �� ����� ������
    public AudioMixer mixer;

    // �������� ��� ����������� ���������
    public Slider slider;

    // ��������� ���� ��� ����������� �������� �������� ���������
    public Text sliderValueText;

    // ��������������� ���� ��� �������� �������� �������� ���������
    private float _volumeValue;

    // ��������� ��� �������������� �������� ��������� � ��������
    private const float _multiplier = 20f;


    private void Awake()
    {
        // ������������� �� ������� ��������� �������� ��������
        slider.onValueChanged.AddListener(HandleSliderValueChanged);

    }

    private void HandleSliderValueChanged(float value)
    {
        // ����������� �������� �������� � �������� � ���������� ��� � ������
        _volumeValue = Mathf.Log10(value) * _multiplier;
        mixer.SetFloat(volumeParametr, _volumeValue);

        // ��������� ��������� ���� � ������� ��������� ���������
        sliderValueText.text = value.ToString("F1");
    }

    void Start()
    {
        Debug.Log("VolumeManager: Start called.");
        // �������� ������� �������� ��������� �� PlayerPrefs
        _volumeValue = PlayerPrefs.GetFloat(volumeParametr, Mathf.Log10(slider.value) * _multiplier);
        // ������������� �������� �������� � ������������ � ������� ��������� ���������
        slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
        // ��������� ��������� ���� � ������� ��������� ���������
        sliderValueText.text = slider.value.ToString("F1");
    }

    private void OnDisable()
    {
        // ��������� ������� �������� ��������� � PlayerPrefs ��� ���������� �������
        PlayerPrefs.SetFloat(volumeParametr, _volumeValue);
    }
}