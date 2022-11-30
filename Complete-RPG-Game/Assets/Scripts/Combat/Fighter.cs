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
        [SerializeField] private float firstAttackDelay = 0.5f;
        [Range(0, 1)] [SerializeField] private float attackSpeedPercentage = 1f;

        private float timeSinceLastAttack;
        
        void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            timeSinceLastAttack = timeBetweenAttacks - firstAttackDelay;
        }

        void Update()
        {
            //timeSinceLastAttack += Time.deltaTime;

            if(target != null)
            {
                if(Vector3.Distance(transform.position, target.transform.position) < weaponRange)
                {
                    mover.Cancel();
                    AttackBehaviour();
                }
                else
                {
                    animator.ResetTrigger("attack");
                    mover.MoveTo(target.transform.position, attackSpeedPercentage);
                    timeSinceLastAttack = timeBetweenAttacks - firstAttackDelay;
                }
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= timeBetweenAttacks && !target.IsDead())
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
