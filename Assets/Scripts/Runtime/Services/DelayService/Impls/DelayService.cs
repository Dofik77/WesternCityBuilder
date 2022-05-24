using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Runtime.Services.DelayService.Impls
{
    public class DelayService : IDelayService, ITickable
    {
        private List<DelayObject> _delays;

        public DelayService()
        {
            _delays = new List<DelayObject>();
        }
        
        public void Do(float delay, Action action)
        {
            _delays.Add(new DelayObject(delay, action));
        }

        public void Tick()
        {
            for (int i = 0; i < _delays.Count; i++)
                if (_delays[i].Handle())
                    _delays.Remove(_delays[i]);
        }
    }
    
    public class DelayObject
    {
        public float Delay;
        public Action Action;
        
        public DelayObject (float delay, Action action)
        {
            Delay = delay;
            Action = action;
        }
            
        public bool Handle()
        {
            Delay -= Time.deltaTime;
            if (Delay <= 0)
            {
                Action.Invoke();
                return true;
            }
            return false;
        }
    }
}