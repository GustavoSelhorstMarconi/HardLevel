using System.Threading.Tasks;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public void HandleDestroyPlayer()
    {
        if (DataSaveControl.Instance != null)
        {
            int deathCounter = DataSaveControl.Instance.Load(DataSaveControl.DEATH_COUNT_KEY_NAME, 0);
            deathCounter++;
            
            DataSaveControl.Instance.Save(DataSaveControl.DEATH_COUNT_KEY_NAME, deathCounter);
            
            DeathUIControl.Instance.HandleUpdateUI(deathCounter);
        }
            
        Destroy(gameObject);
    }
}
