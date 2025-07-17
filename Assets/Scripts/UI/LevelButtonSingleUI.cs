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
        levelText.text = "Level "  + levelNumber;
        
        levelButton.onClick.AddListener(() =>
        {
            LevelControl.Instance.LoadLevelByNumber(levelNumber);
            MenuUIControl.Instance.CloseMenu();
        });
    }
}
