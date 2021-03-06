using UnityEngine;

namespace HGrandry.Tweens.Authoring
{
    public class AnchorPositionTweenDefinition : TweenDefinition
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _target = Vector2.zero;
        
        protected override ITween CreateTween(float duration)
        {
            return _rectTransform.TweenAnchorPos(_target, duration);
        }

        public override void ApplyImmediate()
        {
            _rectTransform.anchoredPosition = _target;
        }

        protected override void KillTween(ITween tween)
        {
            _rectTransform.KillTween(tween);
        }
    }
}