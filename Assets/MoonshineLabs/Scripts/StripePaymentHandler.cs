using System;
using Stripe;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StripePaymentHandler : MonoBehaviour
{
    // UI elements
    public TMP_InputField cardNumberField;
    public TMP_InputField expiryDateFieldMonth, expiryDateFieldYear;
    public TMP_InputField cvcField;
    public Button submitButton;
    public TMP_Text messageText; // Text element to display messages
    public Button messageButton; // Button to handle actions on message
    public GameObject paymentPanel; // The payment panel
    public GameObject messagePanel; // The message panel
    public Button retryButton;
    public GameObject closeWindowButton;
    // Server URLs
    public string createPaymentIntentURL = "http://localhost:3001/create-payment-intent";
    public string confirmPaymentURL = "http://localhost:3001/confirm-payment";
    public string createTokenURL = "http://localhost:3001/create-token";
    public string paymentIntentId;
    // Payment intent's client secret
    public string clientSecret;
    public Toggle saveCardInfoToggle;
    public static StripePaymentHandler Instance;
    public event Action<PaymentStatus> OnPaymentStatusChanged;
    public class PaymentStatus
    {
        public bool success;
        public string message;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        closeWindowButton.SetActive(false);
        retryButton.onClick.RemoveAllListeners();
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmit);
        retryButton.gameObject.SetActive(false); // Hide retry button initially
        submitButton.interactable = false;
        retryButton.onClick.AddListener(() => StartCoroutine(CreatePaymentIntent(100, "usd"))); // New retry action
        messagePanel.SetActive(false);
        // Create the payment intent when the script starts
        StartCoroutine(CreatePaymentIntent(100, "usd"));
        LoadCardInfo();  // assuming $10.00
    }

    private void OnSubmit()
    {
        retryButton.gameObject.SetActive(false); // Hide retry button
        retryButton.onClick.RemoveAllListeners(); // Remove previous retry actions
        retryButton.onClick.AddListener(OnSubmit); // Reset to default retry action
        StartCoroutine(CreateTokenAndConfirmPayment());
        if (saveCardInfoToggle.isOn)
        {
            SaveCardInfo(cardNumberField.text, expiryDateFieldMonth.text, expiryDateFieldYear.text, cvcField.text);
        } // Create a token and confirm the payment in one step
    }
    private IEnumerator CreatePaymentIntent(int amount, string currency)
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("currency", currency);

        using (UnityWebRequest www = UnityWebRequest.Post(createPaymentIntentURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                DisplayMessage("Failed to create payment. Please try again.", () => {});
                retryButton.gameObject.SetActive(true); // Show retry button
            }
            else
            {
                // Payment Intent created successfully
                PaymentIntentResponse response = JsonUtility.FromJson<PaymentIntentResponse>(www.downloadHandler.text);
                clientSecret = response.clientSecret;
                paymentIntentId = response.paymentIntentId;
                Debug.Log("Client Secret: " + clientSecret);
                Debug.Log("PaymentIntent ID: " + paymentIntentId);
                submitButton.interactable = true;
            }
        }
    }


    private IEnumerator CreateTokenAndConfirmPayment()
    {
        StripeConfiguration.ApiKey = "pk_test_51NbUEDJMUUkK0ZLws2NDCjQGyVKGC9ZWwDNSo2nJYME9UaVXCAYxjuVctkWPigu0oXAIfDDc0FaZmpxNEwNeghY400WCnM3f5g";

        var tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Number = cardNumberField.text,
                ExpMonth = expiryDateFieldMonth.text,
                ExpYear = expiryDateFieldYear.text,
                Cvc = cvcField.text,
            },
        };
        var tokenService = new TokenService();
        Token stripeToken = null;

        try
        {
            stripeToken = tokenService.Create(tokenOptions);
        }
        catch (StripeException e)
        {
            Debug.Log(e.Message);
            yield break;
        }

        // Step 2: Confirm the payment with the token
        WWWForm form = new WWWForm();
        form.AddField("token", stripeToken.Id); // Pass the token id to the server

        using (UnityWebRequest wwwConfirm = UnityWebRequest.Post(confirmPaymentURL, form))
        {
            yield return wwwConfirm.SendWebRequest();

            if (wwwConfirm.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(wwwConfirm.error);
                DisplayMessage("Failed to confirm payment. Please try again.", () =>
                {
                    messagePanel.SetActive(false);
                });
                retryButton.gameObject.SetActive(true); // Show retry button
                OnPaymentStatusChanged?.Invoke(new PaymentStatus { success = false, message = wwwConfirm.error });
            }
            else
            {
                Debug.Log("Payment confirmed successfully");
                DisplayMessage("Payment confirmed successfully", () => { closeWindowButton.SetActive(true); });
                OnPaymentStatusChanged?.Invoke(new PaymentStatus { success = true, message = "Payment confirmed successfully" });
            }
        }
    }
    private void SaveCardInfo(string cardNumber, string expiryMonth, string expiryYear, string cvc)
    {
        PlayerPrefs.SetString("CardNumberLast4", cardNumber.Substring(cardNumber.Length - 4));
        PlayerPrefs.SetString("ExpiryDate", expiryMonth + "/" + expiryYear);
        PlayerPrefs.SetString("CVC", "***");
        PlayerPrefs.Save();
    }

    private void LoadCardInfo()
    {
        if (PlayerPrefs.HasKey("CardNumberLast4"))
        {
            cardNumberField.text = "**** **** **** " + PlayerPrefs.GetString("CardNumberLast4");
            expiryDateFieldMonth.text = PlayerPrefs.GetString("ExpiryDate").Split('/')[0];
            expiryDateFieldYear.text = PlayerPrefs.GetString("ExpiryDate").Split('/')[1];
            cvcField.text = PlayerPrefs.GetString("CVC");
        }
    }
    private IEnumerator CreateCustomer(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        using (UnityWebRequest www = UnityWebRequest.Post("YOUR_SERVER_URL/create-customer", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string customerId = www.downloadHandler.text; // Assuming you return just the ID as plain text. Adjust as needed based on server response.
                // Store customerId for later use
            }
            else
            {
                Debug.Log(www.error);
                // Handle error
            }
        }
    }
    private IEnumerator ConfirmPaymentUsingSavedCard(string customerId, string paymentMethodId)
    {
        WWWForm form = new WWWForm();
        form.AddField("customerId", customerId);
        form.AddField("paymentMethodId", paymentMethodId);

        using (UnityWebRequest www = UnityWebRequest.Post("YOUR_SERVER_URL/confirm-payment", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Handle successful payment
                Debug.Log("Payment confirmed successfully");
            }
            else
            {
                Debug.Log(www.error);
                // Handle error
            }
        }
    }

    private IEnumerator SaveCard(string cardNumber, string expiryDate, string cvc, string cardHolderName, string customerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("cardNumber", cardNumber);
        form.AddField("expiryDate", expiryDate);
        form.AddField("cvc", cvc);
        form.AddField("cardHolderName", cardHolderName);
        form.AddField("customerId", customerId);

        using (UnityWebRequest www = UnityWebRequest.Post("YOUR_SERVER_URL/save-card", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string paymentMethodId = www.downloadHandler.text;  // Adjust as needed based on server response.
                // Store paymentMethodId for later use, or use it immediately for a transaction
            }
            else
            {
                Debug.Log(www.error);
                // Handle error
            }
        }
    }


[System.Serializable]
public class TokenResponse
{
    public string id;
}

    [System.Serializable]
    public class PaymentIntentResponse
    {
        public string clientSecret;
        public string paymentIntentId;
    }


    private void DisplayMessage(string message, UnityAction action)
    {
        messageText.text = message;
        messageButton.onClick.RemoveAllListeners();
        messageButton.onClick.AddListener(action);
        messageButton.onClick.AddListener(() => { messagePanel.SetActive(false); });
        messagePanel.SetActive(true);
    }
}