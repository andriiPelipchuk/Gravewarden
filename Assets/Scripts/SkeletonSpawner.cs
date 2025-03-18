using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SkeletonSpawner : MonoBehaviour
    {
        public GameObject prefab;
        public Transform[] spawners;

        private bool skeletonAlive = false;
        private Queue<GameObject> pool;

        private void Awake()
        {
            pool = new Queue<GameObject>();
            for (int i = 0; i < spawners.Length; i++)
            {
                CreateNewSkeleton();
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
            /*if(skeletonAlive)
                return;*/
            foreach (Transform spawnPoint in spawners) 
            {
                GameObject helper = GetFromPool();
                helper.transform.position = spawnPoint.position; 
                helper.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
            //skeletonAlive = true;
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