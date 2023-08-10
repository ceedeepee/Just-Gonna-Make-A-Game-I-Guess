using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripePaymentListenerExample : MonoBehaviour
{
    private void Start()
    {
        StripePaymentHandler.Instance.OnPaymentStatusChanged += HandlePaymentStatus;
    }

    private void HandlePaymentStatus(StripePaymentHandler.PaymentStatus status)
    {
        if (status.success)
        {
            Debug.Log("Payment Successful: " + status.message);
        }
        else
        {
            Debug.Log("Payment Failed: " + status.message);
        }
    }

    private void OnDestroy()
    {
        StripePaymentHandler.Instance.OnPaymentStatusChanged -= HandlePaymentStatus;
    }
}
