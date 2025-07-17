using System.Threading.Tasks;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public void HandleDestroyPlayer()
    {
        Destroy(gameObject);
    }
}
