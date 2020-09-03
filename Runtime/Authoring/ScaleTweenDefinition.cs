using UnityEngine;

namespace HGrandry.Tweens.Authoring
{
    public class ScaleTweenDefinition : TweenDefinition
    {
        [SerializeField] private Transform _transform;
        [SerializeField][Range(-1, 1)] private float _x = 1;
        [SerializeField][Range(-1, 1)] private float _y = 1;
        [SerializeField][Range(-1, 1)] private float _z = 1;
        
        protected override ITween CreateTween(float duration)
        {
            return _transform.TweenScale(new Vector3(_x, _y, _z), duration);
        }

        public override void ApplyImmediate()
        {
            _transform.localScale = new Vector3(_x, _y, _z);
        }

        protected override void KillTween(ITween tween)
        {
            _transform.KillTween(tween);
        }
    }
}