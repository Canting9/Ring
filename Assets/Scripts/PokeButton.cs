using UnityEngine;
using UnityEngine.Events;

public class PokeButton : MonoBehaviour
{
    public UnityEvent onPoke;
    public Transform rightIndexTip;
    public float pokeDistance = 0.02f;

    private float cooldown = 0f;
    private bool wasInside = false;

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        if (rightIndexTip == null) return;

        float dist = Vector3.Distance(
            transform.position, rightIndexTip.position);
        bool isInside = dist < pokeDistance;

        // 手指刚进入范围的那一瞬间触发
        if (isInside && !wasInside)
        {
            cooldown = 0.5f;
            if (onPoke != null) onPoke.Invoke();
        }
        wasInside = isInside;
    }
}
