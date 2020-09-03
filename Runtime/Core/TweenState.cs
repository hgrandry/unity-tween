using System;
using UnityEngine;

namespace HGrandry.Tweens
{
    internal interface ITweenState
    {
        void Update(float t);
        void Complete();
        void Inverse();
        void Dispose();
    }
    
    internal abstract class TweenState<T> : ITweenState
    {
        protected T StartValue;
        protected T TargetValue;
        protected Action<T> Set;

        internal void Init(T start, T target, Action<T> set)
        {
            StartValue = start;
            TargetValue = target;
            Set = set;
        }

        public abstract void Update(float t);
        public abstract void Dispose();

        public void Complete()
        {
            Set(TargetValue);
        }

        public void Inverse()
        {
            T start = StartValue;
            StartValue = TargetValue;
            TargetValue = start;
            InverseDelta();
        }

        protected abstract void InverseDelta();
    }

    internal class FloatTweenState : TweenState<float>
    {
        private float _deltaValue;

        public void Init(float start, Action<float> set, float target)
        {
            base.Init(start, target, set);
            _deltaValue = target - start;
        }

        public override void Update(float t)
        {
            Set(StartValue + _deltaValue * t);
        }

        protected override void InverseDelta()
        {
            _deltaValue = -_deltaValue;
        }

        public override void Dispose()
        {
            TweenPool.Float.Recycle(this);
        }
    }
    
    internal class Vector2TweenState : TweenState<Vector2>
    {
        private Vector2 _deltaValue;

        public void Init(Vector2 start, Action<Vector2> set, Vector2 target)
        {
            base.Init(start, target, set);
            _deltaValue = target - start;
        }

        public override void Update(float t)
        {
            Set(StartValue + _deltaValue * t);
        }

        protected override void InverseDelta()
        {
            _deltaValue = -_deltaValue;
        }
        
        public override void Dispose()
        {
            TweenPool.V2.Recycle(this);
        }
    }

    internal class Vector3TweenState : TweenState<Vector3>
    {
        private Vector3 _deltaValue;

        public void Init(Vector3 start, Action<Vector3> set, Vector3 target)
        {
            base.Init(start, target, set);
            _deltaValue = target - start;
        }

        public override void Update(float t)
        {
            Set(StartValue + _deltaValue * t);
        }

        protected override void InverseDelta()
        {
            _deltaValue = -_deltaValue;
        }
        
        public override void Dispose()
        {
            TweenPool.V3.Recycle(this);
        }
    }
    
    internal class QuaternionTweenState : TweenState<Quaternion>
    {
        public void Init(Quaternion start, Action<Quaternion> set, Quaternion target)
        {
            base.Init(start, target, set);
        }

        public override void Update(float t)
        {
            Set(Quaternion.Lerp(StartValue, TargetValue, t));
        }

        protected override void InverseDelta()
        {
        }
        
        public override void Dispose()
        {
            TweenPool.Quaternions.Recycle(this);
        }
    }
}