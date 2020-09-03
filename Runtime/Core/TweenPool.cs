using UnityEngine;

namespace HGrandry.Tweens
{
    internal static class TweenPool
    {
        private const int StartupCount = 250;
    
        public static Pool<Tween> Tweens;
        public static Pool<Tween.Delay> Delays;
        public static Pool<FloatTweenState> Float;
        public static Pool<Vector2TweenState> V2;
        public static Pool<Vector3TweenState> V3;
        public static Pool<QuaternionTweenState> Quaternions;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            Tweens = new Pool<Tween>(StartupCount);
            Delays = new Pool<Tween.Delay>(StartupCount);
            Float = new Pool<FloatTweenState>(StartupCount);
            V2 = new Pool<Vector2TweenState>(StartupCount);
            V3 = new Pool<Vector3TweenState>(StartupCount);
            Quaternions = new Pool<QuaternionTweenState>(StartupCount);
        }
    }
}