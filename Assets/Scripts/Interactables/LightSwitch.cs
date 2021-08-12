using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    public bool isOn = false;
    public Light[] lights;

    [Space]
    [Header("Light Switch")]

    public Transform lightSwitch;

    public float onAngle;
    public float offAngle;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        DoChecks();
    }

    private void SetLights()
    {
        foreach (Light light in lights)
        {
            light.enabled = isOn;
        }
    }

    private void SetAngle()
    {
        if (lightSwitch != null) lightSwitch.eulerAngles = Vector3.right * (isOn ? onAngle : offAngle);
    }

    private void SetText()
    {
        interactText = "Turn " + (isOn ? "off" : "on");
    }

    private void DoChecks()
    {
        SetLights();
        SetAngle();
        SetText();
    }

    public override void Interact()
    {
        isOn = !isOn;
        DoChecks();

        source.Play();
    }
}
