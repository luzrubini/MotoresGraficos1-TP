using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    private int currentCamera = 1;

    private void Start()
    {
        SetCamera(1);
    }

    public void TriggerA()
    {
        if (currentCamera == 1) SetCamera(2);
        else if (currentCamera == 2) SetCamera(1);
    }

    public void TriggerB()
    {
        if (currentCamera == 2) SetCamera(3);
        else if (currentCamera == 3) SetCamera(2);
    }

    private void SetCamera(int index)
    {
        currentCamera = index;

        cam1.gameObject.SetActive(index == 1);
        cam2.gameObject.SetActive(index == 2);
        cam3.gameObject.SetActive(index == 3);
    }
}
