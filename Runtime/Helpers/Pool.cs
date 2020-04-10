using System;
using System.Collections.Generic;
using System.Linq;

namespace HGrandry.Tweens
{
    internal class Pool<T> where T : new()
    {
        private readonly Stack<T> _stack = new Stack<T>();
        private readonly int _maxCount;

        public Pool(int startCount = 0, int maxCount = int.MaxValue)
        {
            _maxCount = maxCount;

            for (int i = 0; i < startCount; i++)
            {
                _stack.Push(new T());
            }
        }

        public T Get(Action<T> init)
        {
            T instance = _stack.Any()
                ? _stack.Pop()
                : new T();

            init(instance);
            
            return instance;
        }

        public void Recycle(T instance)
        {
            if (_stack.Count < _maxCount)
            {
                _stack.Push(instance);
            }
        }
    }
}