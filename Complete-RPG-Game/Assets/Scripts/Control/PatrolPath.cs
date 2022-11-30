using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float wayPointGizmosRadius = 0.2f;

        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(GetWayPointPosition(i), wayPointGizmosRadius);

                int j = GetNextIndex(i);
                Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));
            }
        }

        public Vector3 GetWayPointPosition(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if(i == transform.childCount - 1)
            {
                return 0;
            }
            return i + 1;
        }
    }
}
