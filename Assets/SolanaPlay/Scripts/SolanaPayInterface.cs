using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using Mono.Cecil.Cil;
using mPlayer;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using Newtonsoft.Json.Converters;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using WebP;


public class SolanaPayInterface : MonoBehaviour
{
    
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OnReady(string text);

    [DllImport("__Internal")]
    private static extern void SendReactMessage(string textFunction, string textMessage);

    [DllImport("__Internal")]
    private static extern void SendFinishMessage(string text);
#endif

    public ExtensionMethods _extension;

    public string m_hostname = "/";
    public string m_api = "api/solanaplay";
    public string m_platform = "moonshinelabs";
    public GameObject popup;
// Delegate that defines the method signature for our event
    public delegate void TransactionStatusHandler(PaymentStatusResponse statusResponse);

// Event that other classes can subscribe to
    public event TransactionStatusHandler OnTransactionStatusChanged;
    // Singleton instance
    private static SolanaPayInterface _instance;

    // Public property to access the instance
    public static SolanaPayInterface Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SolanaPayInterface>();
                
                // Ensure the instance is not destroyed when loading new scenes
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // If the instance doesn't exist, assign this instance
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        // If another instance already exists, destroy this one
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    public string platform
    {
        get { return m_platform; }
    }

    public string hostname
    {
        get { return m_hostname; }
    }

    public void reactSendHostname(string reactHostname)
    {
        //Debug.LogFormat("Hostname from React button is:" + reactHostname);
        m_hostname = reactHostname;
    }

    public void reactSendPlatform(string reactPlatform)
    {
        //Debug.LogFormat("Platform from React button is:" + reactPlatform);
        m_platform = reactPlatform;
    }
    public void Start()
    {
        StartCoroutine(GetItems((items) =>
        {
            foreach (var item in items)
            {
                //Debug.Log(item.description);
            }
        }));
        popup.SetActive(false);
        _extension = GetComponent<ExtensionMethods>();
#if UNITY_WEBGL
        OnReady("OnReady");
#endif
    }
    

    public  SolanaPlayDefinitions.MemberStub memberInfo = new SolanaPlayDefinitions.MemberStub();
    [SerializeField] private Image progressBarFill;

    public void SetProgress(float progress)
    {
        progressBarFill.fillAmount = Mathf.Clamp01(progress);
    }
    
    public IEnumerator GetItems(Action<List<SolanaPlayDefinitions.Item>> callback)
    {
        //Debug.Log(m_hostname + m_api + "/items" + m_platform );
        var req = new UnityWebRequest(m_hostname + m_api + "/items/" + m_platform, "GET");
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            //Debug.Log(req.error);
        }
        else
        {
            string jsonResponse = req.downloadHandler.text;
            List<SolanaPlayDefinitions.Item> items = JsonConvert.DeserializeObject<List<SolanaPlayDefinitions.Item>>(jsonResponse);
            callback?.Invoke(items);
        }
    }

    public IEnumerator GetPurchaseLink(string platformId, string itemId, int quantity, string playerId, string token, Action<string> callback)
    {
        //Debug.Log(platformId + " " + itemId + " " + quantity + " " + playerId + " " + token);
        string url = $"https://dev.moonshinelabs.io/api/solanaplay/qr/{platformId}/{itemId}/{playerId}?quantity={quantity}&payToken={token}";
        //Debug.Log(url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseJson = request.downloadHandler.text;
            SolanaPlayDefinitions.BuyResponse buyResponse = SolanaPlayDefinitions.BuyResponse.FromJson(responseJson);
            callback?.Invoke(buyResponse.link);

            // Start polling for payment status.
            StartCoroutine(CheckPaymentStatus(buyResponse.qrRefId)); // Assuming the BuyResponse has a qrRefId field.
        }

        else
        {
            //Debug.LogError("Error getting purchase link: " + request.error);
        }
    }
    
    
    // General Purpose
    
    public IEnumerator GetImage(string url, Action<Texture2D> callback)
    {
        UnityWebRequest imageRequest = UnityWebRequest.Get(url);
        imageRequest.SetRequestHeader("accept", "*/*");
        imageRequest.SetRequestHeader("accept-encoding", "gzip, deflate");
        imageRequest.SetRequestHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

        yield return imageRequest.SendWebRequest();
        if (imageRequest.result != UnityWebRequest.Result.Success)
        {
            //Debug.Log(imageRequest.error + " // " + url);
        }
        else
        {
            byte[] results = imageRequest.downloadHandler.data;
            Texture2D texture = null;
            bool isWebP = url.EndsWith(".webp", StringComparison.OrdinalIgnoreCase);

            if (isWebP)
            {
                texture = Texture2DExt.CreateTexture2DFromWebP(results, lMipmaps: true, lLinear: true, lError: out Error lError);

                if (lError != Error.Success)
                {
                    //Debug.LogError("Webp Load Error : " + lError.ToString());
                }
            }
            else
            {
                texture = new Texture2D(2, 2);
                if (!texture.LoadImage(results))
                {
                    //Debug.LogError("Non-Webp Load Error");
                }
            }

            if (texture != null)
            {
                callback?.Invoke(texture);
            }
        }
    }
public IEnumerator CheckPaymentStatus(string qrRefId)
{
    int retryCount = 0;
    const int maxRetries = 5;

    yield return new WaitForSeconds(5);

    while (true)
    {
        //Debug.Log($"[CheckPaymentStatus] Attempt number: {retryCount + 1}");
        string url = m_hostname + m_api + "/orders/status/" + qrRefId;

        //Debug.Log($"[CheckPaymentStatus] Sending request to URL: {url}");
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            retryCount++;
            //Debug.Log($"[CheckPaymentStatus] Received response: {request.downloadHandler.text}");

            PaymentStatusResponse statusResponse = JsonConvert.DeserializeObject<PaymentStatusResponse>(request.downloadHandler.text);
            OnTransactionStatusChanged?.Invoke(statusResponse);

            if (statusResponse.confirmed)
            {
                //Debug.Log($"[CheckPaymentStatus] Payment confirmed! Transaction Signature: {statusResponse.txSignature}");
                popup.SetActive(true);
                StartCoroutine(HidePopupAfterDelay(10f));
                break;
            }
            else
            {
                //Debug.Log($"[CheckPaymentStatus] Response successful but payment not confirmed. Message: {statusResponse.message}");
                yield return new WaitForSeconds(5f);
                popup.SetActive(true);
            }
        }
        else
        {
            //Debug.LogError($"[CheckPaymentStatus] Error received: {request.downloadHandler.text}");

            ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(request.downloadHandler.text);
            if (errorResponse.error.message.Contains("Order not yet created"))
            {
                retryCount++;
                if (retryCount > maxRetries)
                {
                    //Debug.LogError($"[CheckPaymentStatus] Maximum retry attempts ({maxRetries}) reached. Stopping payment checks.");
                    break;
                }
                //Debug.LogWarning("[CheckPaymentStatus] Order not yet created. Retrying in 1 second...");
                yield return new WaitForSeconds(1);
                continue;
            }

            //Debug.LogError($"[CheckPaymentStatus] Error checking payment status: {errorResponse.error.message}");
            break;
        }

        //Debug.Log("[CheckPaymentStatus] Waiting 1 second before next check...");
        yield return new WaitForSeconds(1);
    }

    //Debug.Log("[CheckPaymentStatus] Exiting CheckPaymentStatus Coroutine.");
}


IEnumerator HidePopupAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    popup.SetActive(false);
}

    [Serializable]
    public class PaymentStatusResponse
    {
        public bool confirmed;
        public string message;
        public string txSignature;
    }

    [Serializable]
    public class ErrorResponse
    {
        public ErrorDetails error;
    }

    [Serializable]
    public class ErrorDetails
    {
        public int code;
        public string message;
    }

    
    //Utils 
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    
    public List<string> progressQueued = new List<string>();
    Coroutine progressQueueCoroutine;
    private bool inProgress = false;
    float progressThing = 0;
    public SolanaPlayDefinitions.gameInfo GameInfo = new SolanaPlayDefinitions.gameInfo();
    public string showAuth;
    

}
