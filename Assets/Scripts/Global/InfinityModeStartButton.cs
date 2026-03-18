using UnityEngine;

public class InfinityModeStartButton : MonoBehaviour
{
    [SerializeField] private GameObject startbutton;
    [SerializeField] private WallController wallController;
    [SerializeField] private GateButton gateButton;
    [SerializeField] private GameObject turret;
    [SerializeField] private TurretSpawner turretSpawner;
    [SerializeField] private BetterySpawner betterySpawner;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    GameStartButton();
        //}
    }

    public void GameStartButton()
    {
        //Debug.Log("Game Start");
        startbutton.SetActive(false);
        wallController.SetInfinityMode();
        gateButton.ForceOpen();
        turret.SetActive(true);
        turretSpawner.enabled = false;
        betterySpawner.SetInfinityMode();

        InteractableMagazineHolder[] holders = FindObjectsOfType<InteractableMagazineHolder>(true);
        foreach (var holder in holders)
        {
            holder.InfinityAmmo = true;
            holder.StartCount = 0;
        }

        GameManager.Instance.IsInfiniteMode = true;
        GameManager.Instance.GameStart();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameStartButton();
    }
}