using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Vector2 flickerDelayBounds = new Vector2(0.1f, 0.75f);

    private Light myLight;

    private void Start()
    {
        myLight = GetComponent<Light>();
        StartCoroutine(LightFlickerCoroutine());
    }

    private IEnumerator LightFlickerCoroutine()
    {
        myLight.intensity = 1f;
        yield return new WaitForSeconds(Random.Range(flickerDelayBounds.x, flickerDelayBounds.y) / 2f);
        myLight.intensity = 0f;
        yield return new WaitForSeconds(Random.Range(flickerDelayBounds.x, flickerDelayBounds.y) / 2f);
        StartCoroutine(LightFlickerCoroutine());
    }
}
