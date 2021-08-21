using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification instance;

    public Text notificationText;
    public Animator notificationAnim;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator SwitchNotificaion(string text)
    {
        notificationAnim.Play("Switch");
        yield return new WaitForSeconds(0.16f);
        DisplayNotification(text);
    }

    public void DisplayNotification(string text)
    {
        if (notificationAnim.GetCurrentAnimatorStateInfo(0).IsName("Notify"))
        {
            StartCoroutine(SwitchNotificaion(text));
        }
        else
        {
            notificationText.text = text;
            notificationAnim.Play("Notify");
        }
    }
}
