using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionUIControl : MonoBehaviour
{
    public static LevelTransitionUIControl Instance { get; private set; }
    
    [SerializeField]
    private Image imageTransitionLevel;
    [SerializeField]
    private float transitionDuration;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
    }

    public void StartFadeIn(Action onFadeComplete = null)
    {
        StartCoroutine(Fade(0f, 1f, onFadeComplete));
    }
    
    public void StartFadeOut(Action onFaceComplete = null)
    {
        StartCoroutine(Fade(1f, 0f, onFaceComplete));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, Action onFadeComplete)
    {
        float elapsed = 0f;
        Color color = imageTransitionLevel.color;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            
            imageTransitionLevel.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
        imageTransitionLevel.color = new Color(color.r, color.g, color.b, endAlpha);
        onFadeComplete?.Invoke();
    }
}
