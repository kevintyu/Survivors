using System.Collections.Generic;
using Survivors.Combat;
using UnityEngine;

namespace Survivors.Enemies
{
    [RequireComponent(typeof(Health))]
    public sealed class EnemyTarget : MonoBehaviour
    {
        private static readonly HashSet<EnemyTarget> ActiveTargets = new();
        private Health health;

        public static int Count => ActiveTargets.Count;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            ActiveTargets.Add(this);
        }

        private void OnDisable()
        {
            ActiveTargets.Remove(this);
        }

        public static EnemyTarget FindClosest(Vector2 position)
        {
            EnemyTarget closest = null;
            float closestSquaredDistance = float.PositiveInfinity;

            foreach (var candidate in ActiveTargets)
            {
                if (candidate == null || candidate.health == null || candidate.health.IsDead)
                {
                    continue;
                }

                float squaredDistance = ((Vector2)candidate.transform.position - position).sqrMagnitude;
                if (squaredDistance < closestSquaredDistance)
                {
                    closest = candidate;
                    closestSquaredDistance = squaredDistance;
                }
            }

            return closest;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRegistry()
        {
            ActiveTargets.Clear();
        }
    }
}
