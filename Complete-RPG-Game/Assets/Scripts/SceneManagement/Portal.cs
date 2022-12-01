using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum PortalIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int sceneDestinationIndex = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private PortalIdentifier portalIdentifier;
        [SerializeField] private float fadeInTime = 1.5f;
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float waitTransitionTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(SceneTransitionRoutine());
            }
        }

        private IEnumerator SceneTransitionRoutine()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return StartCoroutine(fader.Fade(0f, 1f, fadeOutTime, Color.black));

            yield return SceneManager.LoadSceneAsync(sceneDestinationIndex);
            Portal otherPortal = GetOtherPortal();
            UpdateSpawnPlayer(otherPortal);
            yield return new WaitForSeconds(waitTransitionTime);

            yield return StartCoroutine(fader.Fade(1f, 0f, fadeInTime, Color.black));

            

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal != this && portal.portalIdentifier == portalIdentifier)
                {
                    return portal;
                }
            }
            return null;
        }

        private void UpdateSpawnPlayer(Portal portal)
        {
            if (portal == null) return;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.spawnPoint.rotation;
        }
    }
}
