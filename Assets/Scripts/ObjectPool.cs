using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefab;
        public int poolSize = 50; 

        private Queue<Arrow> pool = new Queue<Arrow>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewArrow();
            }
        }

        void CreateNewArrow()
        {
            GameObject arrowObj = Instantiate(prefab);
            Arrow arrow = arrowObj.GetComponent<Arrow>();
            arrow.gameObject.SetActive(false);
            pool.Enqueue(arrow);
        }

        public Arrow GetObject()
        {

            if (pool.Count == 0)
            {
                CreateNewArrow();
            }
            return pool.Dequeue();

        }

        public void ReturnObject(Arrow arrow)
        {
            arrow.gameObject.SetActive(false);
            pool.Enqueue(arrow);
        }
    }
}