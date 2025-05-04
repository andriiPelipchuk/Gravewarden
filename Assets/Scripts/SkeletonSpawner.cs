using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkeletonSpawner : MonoBehaviour
    {
        public GameObject prefab;
        public Transform[] spawners;

        public LayerMask blockLayer;
        public float detectionRadius = 5f; 
        public float repositionRadius = 10f; 
        public int maxRepositionAttempts = 10;

        private bool skeletonAlive = false;
        private Queue<GameObject> pool;
        public float checkRadius = 0.6f;         
        public int maxOffsetTries = 5;

        private void Awake()
        {
            pool = new Queue<GameObject>();
            for (int i = 0; i < spawners.Length; i++)
            {
                CreateNewSkeleton();
            }
        }

        private Vector3[] offsetDirections = new Vector3[]
        {
        Vector3.forward, Vector3.back, Vector3.left, Vector3.right,
        Vector3.forward + Vector3.left, Vector3.forward + Vector3.right,
        Vector3.back + Vector3.left, Vector3.back + Vector3.right
        };

        private Vector3 FindFreeSpawnPosition(Vector3 startPos)
        {
            if (IsPositionClear(startPos))
                return startPos;

            foreach (Vector3 dir in offsetDirections)
            {
                for (int i = 1; i <= maxOffsetTries; i++)
                {
                    Vector3 offsetPos = startPos + dir.normalized * i * checkRadius * 1.5f;
                    if (IsPositionClear(offsetPos))
                        return offsetPos;
                }
            }

            return Vector3.zero;
        }

        private bool IsPositionClear(Vector3 pos)
        {
            return !Physics.CheckSphere(pos, checkRadius, blockLayer);
        }

        private void OnDrawGizmosSelected()
        {
            if (spawners == null) return;

            Gizmos.color = UnityEngine.Color.green;
            foreach (Transform point in spawners)
            {
                Gizmos.DrawWireSphere(point.position, checkRadius);
            }
        }
        private void CreateNewSkeleton()
        {
            GameObject skeleton = Instantiate(prefab);
            skeleton.gameObject.SetActive(false);
            pool.Enqueue(skeleton);
        }

        public void SpawnSkeleton()
        {
            if (skeletonAlive)
                return;
            foreach (Transform spawnPoint in spawners) 
            {
                GameObject skeleton = GetFromPool();
                skeleton.transform.position = FindFreeSpawnPosition(spawnPoint.position);
                skeleton.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                skeleton.SetActive(true);
            }
            skeletonAlive = true;
        }

        private GameObject GetFromPool()
        {
            return pool.Dequeue();
        }
        public void ReturnToPool(GameObject skeleton)
        {
            skeleton.SetActive(false);
            pool.Enqueue(skeleton);
        }
    }
}