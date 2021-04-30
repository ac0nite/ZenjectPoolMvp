using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class Spawner : ITickable
    {
        private GameSettings _settings;
        private SignalBus _signalBus;
        //private ViewObject.Factory _factoryViewObject;
        private PoolManager _pool;
        
        private string _nextSpawnName = null;
        //private int _count = 0;

        public Spawner(GameSettings settings, SignalBus signalBus, PoolManager pool)
        {
            _settings = settings;
            _signalBus = signalBus;
            //_factoryViewObject = factory;
            _pool = pool;
            
            _signalBus.Subscribe<ChangeTypeViewObjectSignal>(NewViewObject);
            _signalBus.Subscribe<ViewObjectInCenterSignal>(NextSpawn);
        }
        
        public void Tick()
        {
            // if (!_poolСreatedViewObject.IsEmpty())
            // {
            //     var name = _poolСreatedViewObject.Dequeue();
            //     var obj = _settings.ObjectDetails.Find(x => x.Name == name);
            //     _factoryViewObject.Create(obj.Mesh);
            // }
        }

        private void NewViewObject(ChangeTypeViewObjectSignal _newObject)
        {
            if (_nextSpawnName == null)
            {
                _nextSpawnName = _newObject.Name;
                NextSpawn();
            }
            else
            {
                _nextSpawnName = _newObject.Name;   
            }
        }

        private void NextSpawn()
        {
            var detail = _settings.ObjectDetails.Find(x => x.Name == _nextSpawnName);
            _pool.GetObject("ViewObject").GetComponent<ViewObject>().Init(detail);
        }
    }
}