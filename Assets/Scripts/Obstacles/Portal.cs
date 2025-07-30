using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Transform portalDestiny;
    [SerializeField]
    private float offsetDistance;
    [SerializeField]
    private float timeEffect;
    [SerializeField]
    private Vector3 scaleEffect;

    private float currentTimeEffect = 0f;
    private Vector3 baseScale;
    private bool growing = true;

    private void Awake()
    {
        baseScale = portalDestiny.localScale;
    }

    private void Update()
    {
        HandleEffect();
    }

    private void HandleEffect()
    {
        currentTimeEffect = growing ? currentTimeEffect + Time.deltaTime : currentTimeEffect - Time.deltaTime;
        
        float effectValue = currentTimeEffect / timeEffect;
        
        transform.localScale = Vector3.Lerp(baseScale, scaleEffect, effectValue);

        if (currentTimeEffect >= timeEffect)
            growing = false;
        else if (currentTimeEffect < 0f)
            growing = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerControl>(out _))
        {
            Vector3 destinyPosition = portalDestiny.position;
            other.transform.position = new Vector2(destinyPosition.x, destinyPosition.y) + (other.GetComponent<Rigidbody2D>().linearVelocity.normalized * offsetDistance);
        }
    }
}
