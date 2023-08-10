using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripePaymentListenerExample : MonoBehaviour
{
    //Normally you'd want to do this in a new script, but for the sake of simplicity we'll do it here
    public PlayerController _playerController;
    private void Start()
    {
        StripePaymentHandler.Instance.OnPaymentStatusChanged += HandlePaymentStatus;
    }

    private void HandlePaymentStatus(StripePaymentHandler.PaymentStatus status)
    {
        if (status.success)
        {
            Debug.Log("Payment Successful: " + status.message);
            _playerController.EnableTripleShot();
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
