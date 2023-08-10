using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleToast : MonoBehaviour
{
    public GameObject toastHolder;
    public RectTransform toastPanel;
    public Image panelBG;
    public TextMeshProUGUI toastMessage;
    private Coroutine runningToast;
    private bool isToastRunning;
    public void startToast(string Message, bool Sentiment, float time2show)
    {
        if (!isToastRunning)
        {
            runningToast = StartCoroutine(runToast(Message, Sentiment,time2show));
        }

        {
            hideToast = time2show + 5;
            Vector3 toastPos = toastPanel.anchoredPosition;
            toastPos.y = -35;
            toastPanel.anchoredPosition = toastPos;
            StopCoroutine(runningToast);
            runningToast = StartCoroutine(runToast(Message, Sentiment,time2show));
        }
    }
    float hideToast = 0;
    private void Update()
    {
        hideToast = hideToast - Time.deltaTime;
        if (hideToast < 0)
        {
            Vector3 toastPos = toastPanel.anchoredPosition;
            toastPos.y = -35;
            toastPanel.anchoredPosition = toastPos;
        }
    }

    private void OnEnable()
    {
        Vector3 toastPos = toastPanel.anchoredPosition;
        toastPos.y = -35;
        toastPanel.anchoredPosition = toastPos;
        //startToast("This is a Test", true, 20);
    }

    IEnumerator runToast(string Message, bool Sentiment,float time2show)
    {
        toastMessage.text = Message;
        if (Sentiment)
        {
            panelBG.color = new Color(0, 1, 0, .33f);
        }
        else
        {
            panelBG.color = new Color(1, 0, 0, .33f);
        }
        toastHolder.SetActive(true);
        while (toastPanel.anchoredPosition.y < 35)
        {
            yield return new WaitForEndOfFrame();
            Vector3 toastPos = toastPanel.anchoredPosition;
            toastPos.y++;
            toastPanel.anchoredPosition = toastPos;
        }

        yield return new WaitForSeconds(time2show);
        
        while (toastPanel.anchoredPosition.y > -35)
        {
            yield return new WaitForEndOfFrame();
            Vector3 toastPos = toastPanel.anchoredPosition;
            toastPos.y--;
            toastPanel.anchoredPosition = toastPos;
        }
        toastHolder.SetActive(false);
    }
}
