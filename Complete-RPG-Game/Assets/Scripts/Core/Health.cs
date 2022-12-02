using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float health = 100f;

        private bool isDead = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0f);
            Debug.Log(health);
            if(health == 0)
            {
                Dead();
            }
        }

        private void Dead()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            TakeDamage(0f);
        }
    }
}

