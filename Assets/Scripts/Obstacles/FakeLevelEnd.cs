using System;
using UnityEngine;

public class FakeLevelEnd : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectsGoing;
    [SerializeField]
    private GameObject[] objectsBack;

    private void Start()
    {
        HandleObjectsEnable(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerControl>(out _))
        {
            HandleObjectsEnable(false);
            gameObject.SetActive(false);
        }
    }

    private void HandleObjectsEnable(bool enable)
    {
        foreach (GameObject objectGoing in objectsGoing)
        {
            objectGoing.SetActive(enable);
        }

        foreach (GameObject objectBack in objectsBack)
        {
            objectBack.SetActive(!enable);
        }
    }
}
