using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

namespace Assets.Scripts
{
    public class AIMovement : MonoBehaviour
    {
        private Transform targetPos;
        private Character character;
        private NavMeshAgent agent;
        private float stopDistance;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component is missing on this GameObject.");
            }
        }
        void Update()
        {
            if (character == null || character.Target == null)
                return;

            targetPos = character.Target;
            float distanceToTarget = Vector3.Distance(transform.position, targetPos.position);
            MoveToTarget(distanceToTarget);
            RotateToTarget(targetPos);
        }

        private void RotateToTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            if (direction.magnitude > 0.01f) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        void MoveToTarget(float target)
        {
            if (target > stopDistance)
            {
                if (!agent.hasPath || agent.destination != targetPos.position)
                {
                    agent.SetDestination(targetPos.position);
                }
            }
            else 
            {
                if (agent.hasPath)
                {
                    agent.ResetPath();
                }
                character.Attack();
            }

        }
        public void AddParameters(Character characters)
        {
            character = characters.GetComponent<Character>();
            if (character == null)
            {
                Debug.LogError("Character component not found on the provided GameObject.");
                return;
            }

            agent.stoppingDistance = character.AttackRange;
            stopDistance = character.AttackRange;
        }

    }
}