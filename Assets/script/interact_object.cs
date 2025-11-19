using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [Header("Diálogo Base")]
    [TextArea(3, 5)] public string introText;
    [TextArea(3, 5)] public string option1Text;
    [TextArea(3, 5)] public string option2Text;
    [TextArea(3, 5)] public string msgOrgullo;
    [TextArea(3, 5)] public string msgHumildad;
    [TextArea(3, 5)] public string msgIguales;
    [TextArea(3, 5)] public string msgFinalOrgullo;
    [TextArea(3, 5)] public string msgFinalHumildad;

    [Header("Memory Puzzle")]
    [TextArea(2, 4)] public string falseMemory;
    [TextArea(2, 4)] public string trueMemory;
    public string keyword;
    public bool revealedTruth = false;

    [Header("Identificación")]
    public string objectName;
    public bool isVirgilio = false;

    [HideInInspector] public bool alreadyInteracted = false;

    public void Interact()
    {
        if (isVirgilio)
        {
            DialogueManager.Instance.StartInteraction(this);
            return;
        }

        if (alreadyInteracted && !revealedTruth && DialogueManager.Instance.virgilioAfterAllObjects)
        {
            RevealTruth();
            DialogueManager.Instance.ShowTemporaryMessage(trueMemory, 4f);
            return;
        }

        if (alreadyInteracted && !revealedTruth)
        {
            DialogueManager.Instance.ShowTemporaryMessage(falseMemory, 4f);
            return;
        }

        DialogueManager.Instance.StartInteraction(this);
        alreadyInteracted = true;
    }


    public void RevealTruth()
    {
        if (revealedTruth) return;
        revealedTruth = true;

        MemoryPuzzleManager.Instance.RegisterTruth(objectName, keyword, trueMemory, this);

        MirrorController.Instance.UpdateMirror(keyword, trueMemory);
    }
}
