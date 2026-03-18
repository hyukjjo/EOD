using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject InfinityStartbutton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameStartButton();
        }
    }

    public void GameStartButton()
    {
        //Debug.Log("Game Start");
        InfinityStartbutton.SetActive(false);
        GameManager.Instance.GameStart();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameStartButton();
    }
}
