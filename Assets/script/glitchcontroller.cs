using UnityEngine;

public class ScreenGlitchController : MonoBehaviour
{
    public Material glitchMat;
    public float glitchSpeed = 3f;

    void Awake()
    {
        if (glitchMat != null) glitchMat.SetFloat("_Intensity", 0f);
    }

    public void TriggerGlitch(float targetIntensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(GlitchRoutine(targetIntensity, duration));
    }

    System.Collections.IEnumerator GlitchRoutine(float target, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.PingPong(t * glitchSpeed, target);
            if (glitchMat != null) glitchMat.SetFloat("_Intensity", lerp);
            yield return null;
        }
        if (glitchMat != null) glitchMat.SetFloat("_Intensity", 0f);
    }
}
