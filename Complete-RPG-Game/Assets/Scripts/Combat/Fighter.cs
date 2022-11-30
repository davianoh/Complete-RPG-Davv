using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Health target;
        private Mover mover;
        private Animator animator;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;

        private float timeSinceLastAttack = 99f;
        
        void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target != null)
            {
                if(Vector3.Distance(transform.position, target.transform.position) < weaponRange)
                {
                    mover.Cancel();
                    AttackBehaviour();
                }
                else
                {
                    mover.MoveTo(target.transform.position);
                }
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack >= timeBetweenAttacks && !target.IsDead())
            {
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        public void Attack(GameObject combatTarget)
        {
            animator.ResetTrigger("stopAttack");
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.SetTrigger("stopAttack");
            target = null;
        }

        // Animation Event
        private void Hit()
        {
            if(target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
    }
}
