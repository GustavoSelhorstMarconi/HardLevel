using System;
using UnityEngine;

public class LevelContainerUIControl : MonoBehaviour
{
    [SerializeField]
    private int levelAmount;
    [SerializeField]
    private Transform levelTemplate;

    private void Start()
    {
        levelTemplate.gameObject.SetActive(false);

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
