using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : Interactable
{
    [TextArea(5, 10)]
    public string infoText;

    [Space]

    public Transform infoBoxObject;
    public Text infoBoxText;

    public Transform infoBoxI;

    [Space]

    public string readMessage = "Read";

    protected bool isReading = false;

    protected Transform camTransform;

    protected Vector3 _lookPos;
    protected Vector3 _rotation;

    protected void Start()
    {
        UpdateText();
        camTransform = Camera.main.transform;

        UpdateBox();
    }

    protected void Update()
    {
        if (isReading)
        {
            _lookPos = camTransform.position - infoBoxObject.position;
            _lookPos.y = 0;

            _rotation = Quaternion.LookRotation(_lookPos).eulerAngles;
            infoBoxObject.eulerAngles = _rotation + Vector3.up * 180f;
        }
        else if (infoBoxI.gameObject.activeInHierarchy)
        {
            _lookPos = camTransform.position - infoBoxI.position;
            _lookPos.y = 0;

            _rotation = Quaternion.LookRotation(_lookPos).eulerAngles;
            infoBoxI.eulerAngles = _rotation + Vector3.up * 180f;
        }
    }

    protected void UpdateBox()
    {
        infoBoxObject.gameObject.SetActive(isReading);
        interactText = isReading ? "Close" : readMessage;
    }

    public override void Interact(PlayerInteraction playerInteraction)
    {
        infoBoxI.gameObject.SetActive(false);

        isReading = !isReading;
        UpdateBox();
    }

    public void UpdateText()
    {
        infoBoxText.text = infoText;
    }

    public void UpdateText(string newText)
    {
        infoText = newText;
        UpdateText();
    }
}
