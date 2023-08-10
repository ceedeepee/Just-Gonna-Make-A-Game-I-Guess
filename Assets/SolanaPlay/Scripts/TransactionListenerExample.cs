using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionListenerExample : MonoBehaviour
{
    private SolanaPayInterface solanaPayInterface;

    private void Start()
    {
        // Reference the singleton instance
        solanaPayInterface = SolanaPayInterface.Instance;

        // Subscribe to the event
        solanaPayInterface.OnTransactionStatusChanged += HandleTransactionStatusChanged;
    }

    private void HandleTransactionStatusChanged(SolanaPayInterface.PaymentStatusResponse statusResponse)
    {
        // Handle the transaction status here, for example:
        Debug.Log("Received transaction status. Confirmed: " + statusResponse.confirmed);
    }
 
    private void OnDestroy()
    {
        // Always good practice to unsubscribe when not needed to prevent memory leaks
        solanaPayInterface.OnTransactionStatusChanged -= HandleTransactionStatusChanged;
    }
}
