using UnityEngine;
#pragma warning disable CS0649

namespace HGrandry.Tweens.Authoring
{
  public class AlphaTweenDefinition : TweenDefinition
  {
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] [Range(0, 1)] private float _target = 1;

    protected override ITween CreateTween(float duration)
    {
      return _canvasGroup.TweenAlpha(_target, duration);
    }

    public override void ApplyImmediate()
    {
      _canvasGroup.alpha = _target;
    }
  }
}