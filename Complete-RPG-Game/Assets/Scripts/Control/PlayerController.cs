using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float runSpeedPercentage = 1f;

        private Mover mover;
        private Fighter fighter;
        private Health health;

        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InteractCombat()) return;
            if (InteractMovement()) return;
        }

        private bool InteractCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target != null && !target.GetComponent<Health>().IsDead())
                {
                    if (Input.GetMouseButton(0))
                    {
                        fighter.Attack(target.gameObject);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InteractMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point, runSpeedPercentage);
                }
                return true;
            }
            return false;
        }


        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
