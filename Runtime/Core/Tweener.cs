using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HGrandry.Tweens
{
    internal class Tweener : MonoBehaviour
    {
        // static
        
        private static Tweener _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                _toAdd.AddRange(_instance._toAdd);
                _toUpdate.AddRange(_instance._toUpdate);
                _toRemove.AddRange(_instance._toRemove);
                
                _instance._toAdd.Clear();
                _instance._toUpdate.Clear();
                _instance._toRemove.Clear();
                
                Destroy(_instance);
            }
            
            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        private static void Initialize()
        {
            if(_instance != null)
                return;
            
            var go = new GameObject("Tweener");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<Tweener>();
        }

        internal static ITween Create(Component owner, ITweenState state, float duration)
        {
            Initialize();
            Tween tween = TweenPool.Tweens.Get(x => x.Init(owner, state, duration));
            _instance.Add(tween);
            return tween;
        }
        
        // instance
        
        private readonly List<ITweenUpdate> _toUpdate = new List<ITweenUpdate>();
        private readonly List<ITweenUpdate> _toAdd = new List<ITweenUpdate>();
        private readonly List<ITweenUpdate> _toRemove = new List<ITweenUpdate>();
        
        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (_toAdd.Any())
            {
                _toUpdate.AddRange(_toAdd);
                _toAdd.Clear();
            }
            
            foreach (ITweenUpdate tween in _toUpdate)
            {
                if (!tween.IsAlive)
                {
                    Remove(tween);
                    continue;
                }
                
                tween.Update(deltaTime);
                
                if (!tween.IsAlive)
                    Remove(tween);
            }
            
            if(!_toRemove.Any())
                return;
            
            foreach (ITweenUpdate tween in _toRemove)
            {
                _toUpdate.Remove(tween);
                tween.Dispose();
            }
            _toRemove.Clear();
        }

        private void Add(ITweenUpdate tween)
        {
            _toAdd.Add(tween);
        }

        private void Remove(ITweenUpdate tween)
        {
            _toRemove.Add(tween);
        }

        internal static void KillTweens(Component owner)
        {
            foreach (ITweenUpdate tween in _instance._toAdd)
            {
                if (tween.Owner == owner)
                {
                    tween.Kill();
                }
            }
            
            foreach (ITweenUpdate tween in _instance._toUpdate)
            {
                if (tween.Owner == owner)
                {
                    tween.Kill();
                }
            }
        }
    }
}