using System;

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
        void Kill();
    }
}