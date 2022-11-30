using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspiciousDuration = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float dwellDuration = 2f;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Mover mover;

        private Vector3 guardPosition;
        private float suspiciousTimer = 99f;
        private float timeSinceArrivedAtWayPoint = 99f;
        private float wayPointDistanceTolerance = 0.5f; //Offset by patrolPath y position
        private int currentWayPointIndex = 0;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if(InAttackRange() && !player.GetComponent<Health>().IsDead())
            {
                suspiciousTimer = 0f;
                AttackBehaviour();
            }
            else if(suspiciousTimer <= suspiciousDuration)
            {
                suspiciousTimer += Time.deltaTime;
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        #region PATROL BEHAVIOUR
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWayPoint = 0f;
                    IterateWayPointIndex();
                }
                nextPosition = GetCurrentWayPoint();
            }

            if(timeSinceArrivedAtWayPoint >= dwellDuration)
            {
                mover.StartMoveAction(nextPosition);
            }
        }

        private void IterateWayPointIndex()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWayPoint()) <= wayPointDistanceTolerance;
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPointPosition(currentWayPointIndex);
        }
        #endregion

        #region SUSPICION BEHAVIOUR
        private void SuspicionBehaviour()
        {
            transform.GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        #endregion

        #region ATTACK BEHAVIOUR
        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }
        #endregion



        private bool InAttackRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) <= chaseDistance;
        }

        #region UNITY EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        #endregion
    }
}
