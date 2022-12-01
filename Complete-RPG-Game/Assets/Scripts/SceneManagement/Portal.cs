using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneDestinationIndex = -1;

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
            yield return SceneManager.LoadSceneAsync(sceneDestinationIndex);
            Debug.Log("Loaded");
            Destroy(gameObject);
        }
    }
}
