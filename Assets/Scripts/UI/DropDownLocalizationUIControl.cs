using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DropDownLocalizationUIControl : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown languageDropdown;

    private void Start()
    {
        languageDropdown.options.Clear();

        foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }
        
        languageDropdown.onValueChanged.AddListener(ChangeLanguage);
    }

    private void ChangeLanguage(int index)
    {
        LocalizationControl.Instance.SetLocale(index);
    }
}
