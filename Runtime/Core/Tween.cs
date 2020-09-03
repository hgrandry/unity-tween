using System;
using HGrandry.Tweens;
using UnityEngine;

namespace HGrandry.Tweens
{
    public interface ITween
    {
        ITween SetEase(Ease ease);
        ITween SetDelay(float duration);
        ITween SetLoop(int count, TweenLoopType type);
        ITween OnUpdate(Action action);
        ITween OnComplete(Action action);
        ITween OnKill(Action action);
        ITween SetName(string name);
    }

    internal interface ITweenUpdate
    {
        string Name { get; }
        Component Owner { get; }
        void Update(float deltaTime);
        bool IsAlive { get; }
        void Dispose();
        void Kill(Component owner = null);
    }

    public enum TweenLoopType
    {
        None,
        Restart,
        Yoyo
    }
    
    internal class Tween : ITween, ITweenUpdate
    {
        internal class Delay
        {
            private float _duration;
            private float _time;

            public bool IsAlive { get; private set; } = true;
            public float OverFlow  { get; private set; }

            public void Init(float duration)
            {
                _duration = duration;

                _time = 0;
                IsAlive = true;
                OverFlow = 0;
            }

            public void Update(float deltaTime)
            {
                _time += deltaTime;
                OverFlow = _time - _duration;
                if (OverFlow >= 0)
                    IsAlive = false;
            }

            public void Dispose()
            {
                TweenPool.Delays.Recycle(this);
            }
        }
        
        public bool IsAlive { get; private set; } = true;

        public string Name { get; private set; }

        private ITweenState _state;
        private float _duration;

        private float _time;
        private Ease _ease = Ease.Linear;
        private Action _onUpdate;
        private Action _onComplete;
        private Action _onKill;
        private Delay _delay;
        private TweenLoopType? _loopType;
        private int _maxLoopCount;
        private int _currentLoopCount;
        private Component _owner;
        
        public Component Owner => _owner;

        public void Init(Component owner, ITweenState state, float duration)
        {
            _owner = owner;
            _state = state;
            _duration = duration;

            Name = null;
            IsAlive = true;
            _time = 0;
            _ease = Ease.Linear;
            _loopType = null;
            _maxLoopCount = 0;
            _currentLoopCount = 0;
            _delay = null;
            _onUpdate = null;
            _onComplete = null;
            _onKill = null;
        }
     
        public ITween SetEase(Ease ease)
        {
            _ease = ease;
            return this;
        }

        public ITween SetDelay(float duration)
        {
            _delay = TweenPool.Delays.Get(x => x.Init(duration));
            return this;
        }

        public ITween SetLoop(int count, TweenLoopType type)
        {
            _loopType = type;
            _maxLoopCount = count;
            return this;
        }

        public ITween OnComplete(Action action)
        {
            _onComplete = action;
            return this;
        }

        public ITween OnKill(Action action)
        {
            _onKill = action;
            return this;
        }

        public ITween SetName(string name)
        {
            Name = name;
            return this;
        }

        public ITween OnUpdate(Action action)
        {
            _onUpdate = action;
            return this;
        }

        public void Kill(Component owner = null)
        {
            if(!IsAlive)
                return;
            
            if(owner != null && owner != _owner)
                return;
            
            IsAlive = false;
            Try(_onKill);
        }

        void ITweenUpdate.Update(float deltaTime)
        {
            if (_owner == null)
            {
                Kill();
                return;
            }
            
            if (_delay != null)
            {
                _delay.Update(deltaTime);
                if(_delay.IsAlive)
                    return;

                float overFlow = _delay.OverFlow;
                _delay.Dispose();
                _delay = null;
                
                if(Math.Abs(overFlow) < float.Epsilon)
                    return;
                
                _time += overFlow;
            }
            else
            {
                _time += deltaTime;
            }

            if (_time < _duration)
            {
                float t = _time / _duration;
                float tEased = Easing.Interpolate(t, _ease);
                _state.Update(tEased);
                Try(_onUpdate);
            }
            else
            {
                OnTweenComplete();
            }
        }

        private void OnTweenComplete()
        {
            _state.Complete();
            Try(_onUpdate);
            Try(_onComplete);
         
            if (_loopType == null || _loopType == TweenLoopType.None || ++_currentLoopCount == _maxLoopCount)
            {
                IsAlive = false;
                return;
            }

            _time = 0;
                
            switch (_loopType)
            {
                case TweenLoopType.Yoyo:
                    _state.Inverse();
                    break;
            }
        }

        private static void Try(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        void ITweenUpdate.Dispose()
        {
            _delay?.Dispose();
            _state.Dispose();
            TweenPool.Tweens.Recycle(this);
        }
    }
}