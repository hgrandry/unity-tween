using UnityEngine;
#pragma warning disable CS0649
// ReSharper disable InconsistentNaming

namespace HGrandry.Tweens.Authoring
{
  public enum TweenStartMode
  {
    OnEnable,
    Manual
  }

  public class TweenParameters : MonoBehaviour
  {
    public float Duration = 1;
    public Ease Ease;
    public float Delay = 0;
    public TweenStartMode StartMode;
    public TweenLoopType Loop;
    public int LoopCount = -1;
  }
}