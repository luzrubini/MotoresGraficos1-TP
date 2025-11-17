using UnityEngine;
using TMPro;
using System.Collections;

public class MirrorController : MonoBehaviour
{
    public static MirrorController Instance;

    [Header("Mirror UI")]
    public TMP_Text mirrorText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateMirror(string keyword, string truth)
    {
        if (mirrorText == null)
        {
            Debug.LogError("MirrorController → No hay TMP_Text asignado.");
            return;
        }

        mirrorText.text = keyword;

        StopAllCoroutines();
        StartCoroutine(FlashKeywords());

        Debug.Log($"[Mirror] Mostrando keyword: {keyword}");
    }

    private IEnumerator FlashKeywords()
    {
        while (true)
        {
            mirrorText.enabled = false;
            yield return new WaitForSeconds(0.15f);
            mirrorText.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
