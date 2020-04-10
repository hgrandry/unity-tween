using System;
using UnityEngine;
#pragma warning disable CS0649

namespace HGrandry.Tweens.Authoring
{
  public abstract class TweenDefinition : MonoBehaviour
  {
    [SerializeField] private TweenParameters _tweenParams;

    private ITween _tween;

    protected abstract ITween CreateTween(float duration);
    public abstract void ApplyImmediate();

    private void OnEnable()
    {
      if (_tweenParams.StartMode == TweenStartMode.OnEnable)
      {
        Run();
      }
    }

    private void OnDisable()
    {
      Stop();
    }

    public void Run(Action onComplete = null)
    {
      Stop();

      _tween = CreateTween(_tweenParams.Duration)
          .SetEase(_tweenParams.Ease);

      if (_tweenParams.Delay > 0)
        _tween.SetDelay(_tweenParams.Delay);

      if (onComplete != null)
        _tween.OnComplete(onComplete);

      if (_tweenParams.Loop != TweenLoopType.None)
        _tween.SetLoop(_tweenParams.LoopCount, _tweenParams.Loop);
    }

    public void Stop()
    {
      if (_tween == null)
        return;

      _tween.Kill();
      _tween = null;
    }
  }
}