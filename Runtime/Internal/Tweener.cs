using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HGrandry.Tweens
{
  internal class Tweener : MonoBehaviour
  {
    // static
    private static Tweener _instance;
    private static bool _initialized;

    private void Awake()
    {
      if (_instance != null)
      {
        Destroy(this);
      }

      _instance = this;
      _initialized = true;
    }

    private void OnDestroy()
    {
      if (_instance == this)
      {
        _instance = null;
        _initialized = false;
      }
    }

    private static void Initialize()
    {
      if (_initialized)
        return;

      var go = new GameObject("Tweener");
      DontDestroyOnLoad(go);
      _instance = go.AddComponent<Tweener>();
    }

    internal static ITween Create(Component owner, ITweenState state, float duration)
    {
      Initialize();
      Tween tween = TweenPool.Tweens.Get(x => x.Init(owner, state, duration));
      _instance.Add(tween);
      return tween;
    }

    // instance

    private readonly List<ITweenUpdate> _toUpdate = new List<ITweenUpdate>();
    private readonly List<ITweenUpdate> _toAdd = new List<ITweenUpdate>();
    private readonly List<ITweenUpdate> _toRemove = new List<ITweenUpdate>();

    private void Update()
    {
      float deltaTime = Time.deltaTime;

      if (_toAdd.Any())
      {
        _toUpdate.AddRange(_toAdd);
        _toAdd.Clear();
      }

      foreach (ITweenUpdate tween in _toUpdate)
      {
        if (!tween.IsAlive)
        {
          Remove(tween);
          continue;
        }

        tween.Update(deltaTime);

        if (!tween.IsAlive)
          Remove(tween);
      }

      if (!_toRemove.Any())
        return;

      foreach (ITweenUpdate tween in _toRemove)
      {
        _toUpdate.Remove(tween);
        tween.Dispose();
      }
      _toRemove.Clear();
    }

    private void Add(ITweenUpdate tween)
    {
      _toAdd.Add(tween);
    }

    private void Remove(ITweenUpdate tween)
    {
      _toRemove.Add(tween);
    }
  }
}