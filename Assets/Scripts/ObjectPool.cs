using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPool : MonoBehaviour
    {
        //public Transform heathBarParent;

        public Arrow arrow;
        //public HealthBar healthBar;

        public int arrowPoolSize = 50; 
        public int healthBarPoolSize = 20;

        private Queue<Arrow> _arrowPool;
        //private Queue<HealthBar> _healthBarPool;

        private void Awake()
        {
            _arrowPool = new Queue<Arrow>();
            for (int i = 0; i < arrowPoolSize; i++)
            {
                CreateNewArrow();
            }
            /*_healthBarPool = new Queue<HealthBar>();
            for (int i = 0; i < healthBarPoolSize; i++)
            {
                CreateNewHeathBar();
            }*/
        }

        /*private void CreateNewHeathBar()
        {
            var healthBarObj = Instantiate(healthBar);
            healthBarObj.transform.SetParent(heathBarParent);
            healthBarObj.gameObject.SetActive(false);
            _healthBarPool.Enqueue(healthBarObj);
        }*/

        void CreateNewArrow()
        {
            Arrow arrowObj = Instantiate(arrow);
            arrowObj.gameObject.SetActive(false);
            _arrowPool.Enqueue(arrowObj);
        }

        public Arrow GetObject()
        {

            if (_arrowPool.Count == 0)
            {
                CreateNewArrow();
            }
            return _arrowPool.Dequeue();

        }

        public void ReturnObject(Arrow arrow)
        {
            arrow.gameObject.SetActive(false);
            _arrowPool.Enqueue(arrow);
        }

    }
}