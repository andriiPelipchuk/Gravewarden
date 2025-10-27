using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CheckpointUI : MonoBehaviour
{
    public Canvas canvas;          
    public TextMeshProUGUI promptText;
    public float showHeight = 2f;
    public float fadeDuration = 0.2f;

    CanvasGroup cg;

    void Awake()
    {
        if (canvas == null) canvas = GetComponentInChildren<Canvas>(true);
        cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();

        // позиція трохи вище
        transform.localPosition = Vector3.up * showHeight;
        gameObject.SetActive(false);
        cg.alpha = 0f;
    }

    public void ShowPrompt(string text)
    {
        if (promptText != null) promptText.text = text;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeTo(1f));
    }

    public void HidePrompt()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndDisable());
    }

    public void PlayConfirm()
    {
        // короткий фідбек при збереженні
        StartCoroutine(ConfirmPulse());
    }

    IEnumerator ConfirmPulse()
    {
        float orig = cg.alpha;
        yield return FadeTo(1f, 0.08f);
        yield return FadeTo(0.6f, 0.12f);
    }

    IEnumerator FadeTo(float target, float duration = -1f)
    {
        if (duration < 0) duration = fadeDuration;
        float start = cg.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        cg.alpha = target;
    }

    IEnumerator FadeOutAndDisable()
    {
        yield return FadeTo(0f);
        gameObject.SetActive(false);
    }
}
