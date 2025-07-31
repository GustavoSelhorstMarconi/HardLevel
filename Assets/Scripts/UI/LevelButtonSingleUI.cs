using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonSingleUI : MonoBehaviour
{
    [SerializeField]
    private Button levelButton;
    [SerializeField]
    private TextMeshProUGUI levelText;

    public void SetOnClick(int levelNumber)
    {
        string levelString = LocalizationControl.Instance.GetLocalizedText(LocalizationControl.LOCALIZATION_TABLE_NAME, LocalizationControl.LEVEL_KEY_NAME);
        
        levelText.text = levelString + (levelNumber + 1);
        
        levelButton.onClick.RemoveAllListeners();
        
        levelButton.onClick.AddListener(() =>
        {
            LevelControl.Instance.LoadLevelByNumber(levelNumber);
            MenuUIControl.Instance.CloseMenu();
        });
    }
}
