// Author MikeNspired. 

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace MikeNspired.UnityXRHandPoser
{
    public class ObjectSpawner : MonoBehaviour
    {
        public bool isActive = true;
        [SerializeField] private bool onlySpawnIfRoom = true;
        [SerializeField] private GameObject Prefab = null;
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private float spawnTimer = 5;

        private string name = string.Empty;
        public bool hitDetect;
        private float currentTimer = 0;

        private void Start()
        {
            name = Prefab.name;
        }

        private void FixedUpdate()
        {
            if (!isActive) return;

            if (!onlySpawnIfRoom)
            {
                TickTimerAndSpawn();
                return;
            }

            if (hitDetect)
                currentTimer = 0;
            else
                TickTimerAndSpawn();
        }

        private void TickTimerAndSpawn()
        {
            currentTimer += Time.deltaTime;
            if (!(currentTimer >= spawnTimer)) return;
            Spawn();
            currentTimer = 0;
        }

        private void Spawn()
        {
            GameObject _gameObject = Instantiate(Prefab, spawnPoint.position, spawnPoint.rotation);
            _gameObject.transform.SetParent(transform);
            _gameObject.name = name;
        }

        private void OnTriggerStay(Collider other)
        {
            XRGrabInteractable xRGrabInteractable = other.GetComponentInParent<XRGrabInteractable>();

            if(xRGrabInteractable)
            {
                if (other.GetComponentInParent<XRGrabInteractable>().name == name)
                    hitDetect = true;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            XRGrabInteractable xRGrabInteractable = other.GetComponentInParent<XRGrabInteractable>();

            if (xRGrabInteractable)
            {
                if (other.GetComponentInParent<XRGrabInteractable>().name == name)
                    hitDetect = false;
            }
        }
    }
}