using System;
using UnityEngine;

public class LevelContainerUIControl : MonoBehaviour
{
    [SerializeField]
    private Transform levelTemplate;

    private int levelAmount;
    
    private void Start()
    {
        levelTemplate.gameObject.SetActive(false);
        levelAmount = LevelControl.Instance.GetLevelAmount();

        UpdateLevelsUI();
    }
    
    private void UpdateLevelsUI()
    {
        foreach (Transform child in transform)
        {
            if (child == levelTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levelAmount; i++)
        {
            Transform levelTransform = Instantiate(levelTemplate, transform);
            levelTransform.gameObject.SetActive(true);
            
            levelTransform.GetComponent<LevelButtonSingleUI>().SetOnClick(i);
        }
    }
}
