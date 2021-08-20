using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    private Vector3 _initialEulerAngles;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        if (lightSwitch != null) _initialEulerAngles = lightSwitch.eulerAngles;
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
        if (lightSwitch != null) lightSwitch.eulerAngles = new Vector3((isOn ? onAngle : offAngle), _initialEulerAngles.y, _initialEulerAngles.z)/*Vector3.right * (isOn ? onAngle : offAngle)*/;
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

    public override void Interact(PlayerInteraction playerInteraction)
    {
        isOn = !isOn;
        DoChecks();

        source.Play();
    }
}
