using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton_New : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameStartButton();
        }
    }

    public void GameStartButton()
    {
        GameManager.Instance.GameStart();
        transform.parent.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameStartButton();
        }
    }
}
