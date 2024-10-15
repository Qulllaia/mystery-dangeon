using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSettingChanges : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Dropdown _dropdownDifficulty;
    [SerializeField] private TMP_Dropdown _dropdownSaveFrequency;
    private SasedData savedData;

    void Awake(){
        savedData = GameObject.FindWithTag("DontDestroyOnLoad").GetComponent<SasedData>();
        _slider.value = savedData.volume;
        _dropdownDifficulty.value = savedData.difficulty;
        _dropdownSaveFrequency.value = savedData.frequency;
    }
    public void OnClick(){
        PlayerPrefs.SetFloat("Volume", _slider.value);
        PlayerPrefs.SetInt("Difficulty", _dropdownDifficulty.value);
        PlayerPrefs.SetInt("SaveFrequency", _dropdownSaveFrequency.value);

        savedData.ChangeOptionsData(_dropdownDifficulty.value, _dropdownSaveFrequency.value,_slider.value);
        PlayerPrefs.Save();
    }

}
