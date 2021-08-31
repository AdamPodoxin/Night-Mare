using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSubtitles : MonoBehaviour
{
    [SerializeField] private Text subtitlesText;

    private bool useSubtitles = true;
    public bool UseSubtitles { get { return useSubtitles; } set { useSubtitles = value; } }

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowSubtitles(string text)
    {
        ShowSubtitles(text, 3f);
    }

    public void ShowSubtitles(string text, float duration)
    {
        if (!useSubtitles) return;

        subtitlesText.text = text;
        anim.Play("Subtitles_FadeIn");
        anim.SetFloat("subtitlesSpeed", 1f / duration);
    }
}
