using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemValue;
    public TMP_Text itemDescription;
    public TMP_Text splToken;
    public Image itemImage;
    public Button retryButton;
    public string itemId, platformId, token;
    public SolanaPayQR solanaPayQRCode;
    public GetAndCreateItems getAndCreateItems;
    public GameObject solanaPlayPopup, itemShopPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Coroutine paymentInProgress;
    private bool isInProgress;
    public void InitiateTransaction()
    {
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() => InitiateTransaction());
        if (isInProgress)
        {
            isInProgress = false;
            StopCoroutine(paymentInProgress);
            paymentInProgress = StartCoroutine(getPaymentString());
        }
        else
        {
            paymentInProgress = StartCoroutine(getPaymentString());
        }
    }

    IEnumerator getPaymentString()
    {
        string paymentString = "";
        isInProgress = true;
        yield return StartCoroutine(getAndCreateItems.solanaPayInterface.GetPurchaseLink(platformId, itemId, 1, "Test9999", token, (payment) =>
        {
            paymentString = payment;//.Replace("http", "https");
        }));
        solanaPlayPopup.SetActive(true);
        yield return new WaitForEndOfFrame();
        solanaPayQRCode.input = paymentString; 
        solanaPayQRCode.create_Code();
        //itemShopPopup.SetActive(false);
        isInProgress = false;
    }
}
