using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionListenerExample : MonoBehaviour
{
    
    // again normally, you'd want to do this in a new script, but for the sake of simplicity we'll do it here
    private SolanaPayInterface solanaPayInterface;
    public PlayerController _playerController;
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
        _playerController.EnableShield();
    }
 
    private void OnDestroy()
    {
        // Always good practice to unsubscribe when not needed to prevent memory leaks
        solanaPayInterface.OnTransactionStatusChanged -= HandleTransactionStatusChanged;
    }
}
