﻿
using System;
using UnityEngine;

    public class SingletoneGameObject<T> : MonoBehaviour where T: SingletoneGameObject<T>
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                if (_instance == null)
                {
                    var holderObject = new GameObject($"Singleton_{typeof(T)}");
                    _instance = holderObject.AddComponent<T>();
                    DontDestroyOnLoad(holderObject);
                }

                return _instance;
            }
        }

        public static T TryInstance
        {
            get { return _instance != null ? _instance : null; }
        }

        private static T _instance = null;
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = (T)this;
                DontDestroyOnLoad(this.gameObject);   
            }
        }
    }
