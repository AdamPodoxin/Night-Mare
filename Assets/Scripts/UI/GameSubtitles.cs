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

    public void ShowSubtitles(string text)
    {
        if (!gameObject.activeInHierarchy) return;
        ShowSubtitles(text, 3f);
    }

    public void ShowSubtitles(string text, float duration)
    {
        if (!gameObject.activeInHierarchy) return;

        subtitlesText.text = text;
        anim.Play("Subtitles_FadeIn");
        anim.SetFloat("subtitlesSpeed", 1f / duration);
    }
}
