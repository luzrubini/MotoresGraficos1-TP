using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text introTextUI;
    public TMP_Text option1TextUI;
    public TMP_Text option2TextUI;

    [Header("Moral System")]
    public int orgullo = 0;
    public int humildad = 0;

    [Header("Puzzle Tracking")]
    private bool virgilioUnlocked = false;
    public bool virgilioAfterAllObjects = false;

    private InteractionObject currentObject;
    private bool isChoosing = false;
    private FogController fogController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        dialoguePanel.SetActive(false);

        fogController = FindObjectOfType<FogController>();
    }

    void Update()
    {
        if (isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseOption(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseOption(2);
        }
    }

    public void StartInteraction(InteractionObject obj)
    {
        currentObject = obj;

        // Si la verdad ya se reveló
        if (obj.revealedTruth)
        {
            ShowTemporaryMessage(obj.trueMemory, 3.5f);
            MirrorController.Instance.UpdateMirror(obj.keyword, obj.trueMemory);
            return;
        }

        if (obj.isVirgilio)
        {
            StartCoroutine(VirgilioDialogue());
            return;
        }

        StartCoroutine(InteractionFlow(obj));
    }

    private IEnumerator InteractionFlow(InteractionObject obj)
    {
        dialoguePanel.SetActive(true);
        introTextUI.text = obj.introText;
        introTextUI.gameObject.SetActive(true);
        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        yield return WaitForSecondsOrSpace(5f);

        if (orgullo > humildad)
            introTextUI.text = obj.msgOrgullo;
        else if (humildad > orgullo)
            introTextUI.text = obj.msgHumildad;
        else
            introTextUI.text = obj.msgIguales;

        yield return WaitForSecondsOrSpace(5f);

        introTextUI.gameObject.SetActive(false);

        option1TextUI.text = "1 - " + obj.option1Text;
        option2TextUI.text = "2 - " + obj.option2Text;
        option1TextUI.gameObject.SetActive(true);
        option2TextUI.gameObject.SetActive(true);

        isChoosing = true;
        Time.timeScale = 0;
    }

    private IEnumerator WaitForSecondsOrSpace(float seconds)
    {
        float timer = 0f;
        while (timer < seconds)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                break;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    void ChooseOption(int choice)
    {
        isChoosing = false;
        Time.timeScale = 1;

        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        string finalMessage = "";

        if (choice == 1)
        {
            orgullo++;
            finalMessage = currentObject.msgFinalOrgullo;

            MirrorState[] mirrors = FindObjectsOfType<MirrorState>();
            foreach (MirrorState m in mirrors) m.AddCrack();

            fogController?.IncreaseDensity();
        }
        else if (choice == 2)
        {
            humildad++;
            finalMessage = currentObject.msgFinalHumildad;

            fogController?.DecreaseDensity();
        }

        // Si se interactuó con todos los objetos y hablamos con Virgilio
        if (MemoryPuzzleManager.Instance.AllObjectsInteracted() && virgilioAfterAllObjects)
        {
            currentObject.RevealTruth();
        }

        StartCoroutine(ShowFinalMessage(finalMessage, 5f));
        MemoryPuzzleManager.Instance.RegisterObject(currentObject.objectName);
    }

    private IEnumerator ShowFinalMessage(string msg, float tiempo)
    {
        dialoguePanel.SetActive(true);
        introTextUI.text = msg;
        introTextUI.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(tiempo);

        introTextUI.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    public void ShowTemporaryMessage(string msg, float tiempo = 2f)
    {
        StartCoroutine(TemporaryMessageCoroutine(msg, tiempo));
    }

    private IEnumerator TemporaryMessageCoroutine(string msg, float tiempo)
    {
        dialoguePanel.SetActive(true);
        introTextUI.text = msg;
        introTextUI.gameObject.SetActive(true);
        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(tiempo);

        introTextUI.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    private IEnumerator VirgilioDialogue()
    {
        dialoguePanel.SetActive(true);
        introTextUI.gameObject.SetActive(true);
        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        string msg = "";

        int progress = 0;
        if (MemoryPuzzleManager.Instance.libroFound) progress++;
        if (MemoryPuzzleManager.Instance.cartasFound) progress++;
        if (MemoryPuzzleManager.Instance.crucifijoFound) progress++;

        if (progress == 0)
            msg = "Tres fragmentos duermen bajo el polvo, Gabriel.\nEmpieza por aquello que guarda palabras no dichas.";
        else if (progress == 1)
            msg = "El camino se abre un poco más.\nA veces la fe pesa más que el hierro, y las cartas mienten menos que las bocas.";
        else if (progress == 2)
            msg = "Solo queda una verdad por mirar de frente.\n¿Podrás sostenerla sin romperte?";

        if (MemoryPuzzleManager.Instance.AllObjectsInteracted())
        {
            virgilioAfterAllObjects = true;
            msg += "\nSientes que la verdad está al alcance de tus ojos...";
        }

        introTextUI.text = msg;
        yield return WaitForSecondsOrSpace(8f);

        introTextUI.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }
}
