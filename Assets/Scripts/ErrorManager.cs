using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ErrorManager : MonoBehaviour
{
    public static ErrorManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    public Transform ErrorMessageCanvas;
    public Text ErrorMessage;
    public float MessageLength;
    float totalTime = 0;
    bool showMessage = false;

    void Update()
    {
        if(showMessage)
        {
            ErrorMessageCanvas.gameObject.SetActive(true);
            totalTime += Time.deltaTime;
            if(totalTime >= MessageLength)
            {
                showMessage = false;
                totalTime = 0;
            }
        }
        else
        {
            ErrorMessageCanvas.gameObject.SetActive(false);
        }
    }

    public void SetMessage(string message)
    {
        ErrorMessage.text = message;
    }

    public void ShowMessage()
    {
        showMessage = true;
    }

    public void SetMessageLength(float length)
    {
        MessageLength = length;
    }

    public void DefaultMessage()
    {
        SetMessage("Coming Soon");
        ShowMessage();
    }
}
