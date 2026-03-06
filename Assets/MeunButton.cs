using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public GameObject menu_button;
    public UnityEvent onPress;        // 长按成功触发
    public UnityEvent onRelease;      // 长按成功后松开触发
    public float holdTime = 1f;       // 长按时间 (秒)

    GameObject presser;
    bool isPressed = false;
    bool isHolding = false;

    Renderer buttonRenderer;
    Color originalColor;
    Coroutine holdRoutine;

    void Start()
    {
        buttonRenderer = menu_button.GetComponent<Renderer>();
        originalColor = buttonRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果已经有人在按，就忽略
        if (isHolding || isPressed) return;

        presser = other.gameObject;
        buttonRenderer.material.color = Color.red;

        // 开始长按计时
        isHolding = true;
        holdRoutine = StartCoroutine(HoldToPress());
    }

    private void OnTriggerExit(Collider other)
    {
        // 退出的不是当前按压者则忽略
        if (other.gameObject != presser) return;

        // 停止长按计时
        if (holdRoutine != null)
        {
            StopCoroutine(holdRoutine);
            holdRoutine = null;
        }

        // 如果还没按成功 → 取消
        if (!isPressed)
        {
            buttonRenderer.material.color = originalColor;
            isHolding = false;
            presser = null;
            return;
        }

        // 如果已经按成功 → 触发 release
        buttonRenderer.material.color = originalColor;
        onRelease.Invoke();
        isPressed = false;
        isHolding = false;
        presser = null;
    }

    private IEnumerator HoldToPress()
    {
        float timer = 0f;

        while (timer < holdTime)
        {
            timer += Time.deltaTime;

            // 如果手已经离开，中断
            if (!isHolding) yield break;

            yield return null;
        }

        // 长按成功
        isPressed = true;
        onPress.Invoke();
    }
}
