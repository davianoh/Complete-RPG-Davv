using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Core
{
    public class Cinematics : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
