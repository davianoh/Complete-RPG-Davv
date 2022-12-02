using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeInSceneTime = 0.5f;

        private SavingSystem savingSystem;
        private Fader fader;
        private const string defaultSaveFile = "save";

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            fader = FindObjectOfType<Fader>();
        }

        private void Start()
        {
            StartCoroutine(LoadLastSceneSaved());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        private IEnumerator LoadLastSceneSaved()
        {
            yield return StartCoroutine(fader.Fade(1f, 1f, 0f, Color.black));
            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return StartCoroutine(fader.Fade(1f, 0f, fadeInSceneTime, Color.black));
        }
    }
}
