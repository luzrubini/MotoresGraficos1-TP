using System.Collections;
using UnityEngine;
using TMPro;

public class FogController : MonoBehaviour
{
    [Header("Configuración de la neblina")]
    public Color fogColor = Color.gray;     
    public float minDensity = 0.2f;       
    public float maxDensity = 1f;         
    public float densityStep = 0.01f;          

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = minDensity;
    }

    public void IncreaseDensity()
    {
        RenderSettings.fogDensity += densityStep;
        RenderSettings.fogDensity = Mathf.Clamp(RenderSettings.fogDensity, minDensity, maxDensity);
        Debug.Log("Neblina aumentada a: " + RenderSettings.fogDensity);
    }

    public void DecreaseDensity()
    {
        RenderSettings.fogDensity -= densityStep;
        RenderSettings.fogDensity = Mathf.Clamp(RenderSettings.fogDensity, minDensity, maxDensity);
        Debug.Log("Neblina disminuida a: " + RenderSettings.fogDensity);
    }
    public void ResetFog()
    {
        RenderSettings.fogDensity = minDensity;
    }
    public void PulseFog()
    {
        StopAllCoroutines();
        StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        float start = RenderSettings.fogDensity;
        float target = Mathf.Clamp(start + 0.02f, minDensity, maxDensity);
        float t = 0f;
        while (t < 0.6f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(start, target, t / 0.6f);
            t += Time.deltaTime;
            yield return null;
        }
        // volver lentamente
        t = 0f;
        while (t < 1.2f)
        {
            RenderSettings.fogDensity = Mathf.Lerp(target, start, t / 1.2f);
            t += Time.deltaTime;
            yield return null;
        }
    }

}
