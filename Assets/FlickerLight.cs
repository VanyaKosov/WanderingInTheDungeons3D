using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public float minRange = 12.0f;
    public float maxRange = 15.0f;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float minDelay = 0.6f;
    public float maxDelay = 1.0f;

    private float intensityChangeSpeed = 0f;
    private float rangeChangeSpeed;
    private Light torchLight;

    void Start()
    {
        torchLight = gameObject.GetComponent<Light>();
        StartCoroutine(GetFlickerValues());
    }

    void Update()
    {
        torchLight.intensity += intensityChangeSpeed * Time.deltaTime;
        torchLight.range += rangeChangeSpeed * Time.deltaTime;
    }

    private IEnumerator GetFlickerValues()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);

            float intensity = Random.Range(minIntensity, maxIntensity);
            intensityChangeSpeed = (intensity - torchLight.intensity) / delay;

            float intensityRatio = (intensity - minIntensity) / (maxIntensity - minIntensity);
            float range = intensityRatio * (maxRange - minRange) + minRange;
            rangeChangeSpeed = (range - torchLight.range) / delay;

            yield return new WaitForSeconds(delay);
        }
    }
}
