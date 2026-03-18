using UnityEngine;

public class MaintenanceSkipButton : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            MaintenanceSkip();
        }    
    }

    public void MaintenanceSkip()
    {
        GameManager.Instance.SkipMaintenence();
    }

    private void OnCollisionEnter(Collision collision)
    {
        MaintenanceSkip();
    }
}
