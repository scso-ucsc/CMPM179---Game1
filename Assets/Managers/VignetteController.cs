using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteController : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    private Vignette vignette;

    private void Start()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
        }

        ResetVignette();
    }

    public void SetVignetteIntensity(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Clamp01(intensity); // Keep between 0 and 1
        }
    }

    // Intensify vignette over time until it reaches the target intensity
    public void FadeIn(float targetIntensity, float time, float slowdownFactor = 1)
    {
        StartCoroutine(IntensifyVignette(targetIntensity, time, slowdownFactor));
    }

    private IEnumerator IntensifyVignette(float targetIntensity, float time, float slowdownFactor = 1)
    {
        float startIntensity = vignette.intensity.value;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float newIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / time);
            SetVignetteIntensity(newIntensity);
            yield return null;
        }

        Time.timeScale = slowdownFactor;
    }

    // Fade out the vignette over time
    public void FadeOut(float time)
    {
        StartCoroutine(FadeOutVignette(time));
    }

    private IEnumerator FadeOutVignette(float time)
    {
        float startIntensity = vignette.intensity.value;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float newIntensity = Mathf.Lerp(startIntensity, 0, elapsedTime / time);
            SetVignetteIntensity(newIntensity);
            yield return null;
        }
    }

    // Reset the vignette intensity to 0

    public void ResetVignette()
    {
        SetVignetteIntensity(0);
    }
}
