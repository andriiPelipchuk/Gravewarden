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
        }
        void Update()
        {
            targetPos = character.Target;
            if (targetPos != null)
            {
                float target = Vector3.Distance(transform.position, targetPos.position);
                MoveToTarget(target);
                RotateToTarget(targetPos);
            }

        }

        private void RotateToTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            if (direction != Vector3.zero) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        void MoveToTarget(float target)
        {
            if (target > stopDistance)
            {
                agent.SetDestination(targetPos.position);
            }
            else 
            {
                agent.ResetPath();
                character.Attack();
            }

        }
        public void AddParameters(Character characters)
        {
            character = characters.GetComponent<Character>();

            agent.stoppingDistance = character.AttackRange;
            stopDistance = character.AttackRange;
        }
    }
}