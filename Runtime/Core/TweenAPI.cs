using System;
using UnityEngine;

namespace HGrandry.Tweens
{
    public static class TweenAPI
    {
        public static ITween Delay(this Component owner, float delay, Action action)
        {
            FloatTweenState state = TweenPool.Float.Get(s => s.Init(0, _ => {}, 0));
            return Tweener.Create(owner, state, 0).SetDelay(delay).OnComplete(action);
        }
        
        public static ITween Tween(this Component owner, Func<float> get, Action<float> set, float target, float duration)
        {
            FloatTweenState state = TweenPool.Float.Get(s => s.Init(get(), set, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static ITween Tween(this Component owner, Func<Vector2> get, Action<Vector2> set, Vector2 target, float duration)
        {
            Vector2TweenState state = TweenPool.V2.Get(s => s.Init(get(), set, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static ITween Tween(this Component owner, Func<Vector3> get, Action<Vector3> set, Vector3 target, float duration)
        {
            Vector3TweenState state = TweenPool.V3.Get(s => s.Init(get(), set, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static void KillTweens(this Component owner)
        {
            Tweener.KillTweens(owner);
        }
        
        public static void KillTween(this Component owner, ITween tween)
        {
            if (tween is ITweenUpdate t && owner == t.Owner)
                t.Kill();
        }
        
        // CanvasGroup
        
        public static ITween TweenAlpha(this CanvasGroup owner, float target, float duration)
        {
            FloatTweenState state = TweenPool.Float.Get(s => s.Init(owner.alpha, x => owner.alpha = x, target));
            return Tweener.Create(owner, state, duration);
        }
        
        // Transform
        
        public static ITween TweenPosition(this Transform owner, Vector3 target, float duration)
        {
            Vector3TweenState state = TweenPool.V3.Get(s => s.Init(owner.position, v => owner.position = v, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static ITween TweenLocalPosition(this Transform owner, Vector3 target, float duration)
        {
            Vector3TweenState state = TweenPool.V3.Get(s => s.Init(owner.localPosition, v => owner.localPosition = v, target));
            return Tweener.Create(owner, state, duration);
        }

        public static ITween TweenRotation(this Transform owner, Quaternion target, float duration)
        {
            QuaternionTweenState state = TweenPool.Quaternions.Get(s => s.Init(owner.rotation, v => owner.rotation = v, target));
            return Tweener.Create(owner, state, duration);
        }

        public static ITween TweenRotation(this Transform owner, Vector3 target, float duration)
        {
            return TweenRotation(owner, Quaternion.Euler(target), duration);
        }

        public static ITween TweenScale(this Transform owner, Vector3 target, float duration)
        {
            Vector3TweenState state = TweenPool.V3.Get(s => s.Init(owner.localScale, v => owner.localScale = v, target));
            return Tweener.Create(owner, state, duration);
        }
        
        // RectTransform
        
        public static ITween TweenAnchorPos(this RectTransform owner, Vector2 target, float duration)
        {
            Vector2TweenState state = TweenPool.V2.Get(s => s.Init(owner.anchoredPosition, v => owner.anchoredPosition = v, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static ITween TweenAnchorMin(this RectTransform owner, Vector2 target, float duration)
        {
            Vector2TweenState state = TweenPool.V2.Get(s => s.Init(owner.anchorMin, v => owner.anchorMin = v, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static ITween TweenAnchorMax(this RectTransform owner, Vector2 target, float duration)
        {
            Vector2TweenState state = TweenPool.V2.Get(s => s.Init(owner.anchorMax, v => owner.anchorMax = v, target));
            return Tweener.Create(owner, state, duration);
        }
        
        public static ITween TweenPivot(this RectTransform owner, Vector2 target, float duration)
        {
            Vector2TweenState state = TweenPool.V2.Get(s => s.Init(owner.pivot, v => owner.pivot = v, target));
            return Tweener.Create(owner, state, duration);
        }
    }
}