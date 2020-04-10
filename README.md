# Unity Tween

Lightweight tween library for Unity.

[Examples](https://github.com/hgrandry/unity-examples/tree/master/Assets/Tween)

```csharp
public class MyComponent : MonoBehaviour
{
  private void Start()
  {
      transform.TweenPosition(new Vector3(500, 0, 0), 1)
            .SetEase(Ease.CubicInOut)
            .OnComplete(() => Debug.Log("Tween completed!"));
  }
}
```
