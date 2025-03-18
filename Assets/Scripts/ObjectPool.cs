using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPool : MonoBehaviour
    {
        public Arrow prefab;
        public int poolSize = 50; 

        private Queue<Arrow> pool;

        private void Awake()
        {
            pool = new Queue<Arrow>();
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewArrow();
            }
        }

        void CreateNewArrow()
        {
            Arrow arrowObj = Instantiate(prefab);
            arrowObj.gameObject.SetActive(false);
            pool.Enqueue(arrowObj);
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