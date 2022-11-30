using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private float maxSpeed = 6f;

        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) navMeshAgent.enabled = false;

            UpdateAnimator();
        }


        public void StartMoveAction(Vector3 destination, float speedPercentage)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedPercentage);
        }

        public void MoveTo(Vector3 destination, float speedPercentage)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedPercentage);
            navMeshAgent.isStopped = false;
        }


        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            animator.SetFloat("forwardSpeed", speed);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
