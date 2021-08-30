using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSubtitles : MonoBehaviour
{
    [SerializeField] private Text subtitlesText;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private IEnumerator ShowSubtitlesCoroutine(string text, float duration)
    {
        subtitlesText.text = text;
        anim.Play("Subtitles_FadeIn");

        yield return new WaitForSeconds(duration + 0.25f);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Subtitles_On"))
            anim.Play("Subtitles_FadeOut");
    }

    public void ShowSubtitles(string text)
    {
        if (!gameObject.activeInHierarchy) return;

        StopCoroutine(ShowSubtitlesCoroutine(text, 3f));
        StartCoroutine(ShowSubtitlesCoroutine(text, 3f));
    }

    public void ShowSubtitles(string text, float duration)
    {
        if (!gameObject.activeInHierarchy) return;

        StopCoroutine(ShowSubtitlesCoroutine(text, duration));
        StartCoroutine(ShowSubtitlesCoroutine(text, duration));
    }
}
