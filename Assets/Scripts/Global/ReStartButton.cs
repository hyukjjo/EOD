using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ReStartButton : MonoBehaviour
{
    private XRInteractionManager _interactionManager;
    private XRDirectInteractor[] _directInteractors;

    private void Start()
    {
        _interactionManager = FindObjectOfType<XRInteractionManager>();
        _directInteractors = FindObjectsOfType<XRDirectInteractor>();
        GameManager.Instance.GameClear += GameReStartButton;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GameReStartButton();
        //}
    }

    public void GameReStartButton()
    {
        //Debug.Log("Game Start");
        Singleton<GameManager> a = FindObjectOfType<Singleton<GameManager>>();
        Singleton<SoundManager> b = FindObjectOfType<Singleton<SoundManager>>();
        Singleton<ObjectPoolManager> c = FindObjectOfType<Singleton<ObjectPoolManager>>();
        Singleton<StageMonsterHolder> d = FindObjectOfType<Singleton<StageMonsterHolder>>();
        Singleton<MonsterSpawner> e = FindObjectOfType<Singleton<MonsterSpawner>>();

        Destroy(a.gameObject);
        Destroy(b.gameObject);
        Destroy(c.gameObject);
        Destroy(d.gameObject);
        Destroy(e.gameObject);

        if (_directInteractors.Length > 0)
        {
            foreach (var _directInteractor in _directInteractors)
            {
                XRBaseInteractable tem = _directInteractor.selectTarget;

                if (tem != null)
                    _interactionManager.SelectExit(_directInteractor, _directInteractor.selectTarget);
            }
        }

        GC.Collect();

        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //SceneManager.LoadScene("Game", LoadSceneMode.Single);
        //Destroy(gameObject);
        StartCoroutine(LoadAsyncScene());
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameReStartButton();
    }

    private IEnumerator LoadAsyncScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }
}
