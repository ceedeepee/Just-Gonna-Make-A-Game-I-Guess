using Solana.Unity.Wallet;
using UnityEngine;
using UnityEngine.UI;

public class WalletUIManager : MonoBehaviour
{
    public InputField publicKeyText;
    public InputField privateKeyField;
    public Button toggleShowHideButton;
    public InputField recipientAddressField;
    public InputField amountField;
    public Button transferButton;

    private SolanaWalletManager walletManager;
    private bool isPrivateKeyVisible = false;

    void Start()
    {
        // Initialize the SolanaWalletManager here.
        walletManager = FindObjectOfType<SolanaWalletManager>();

        // Display public key.
        publicKeyText.text = walletManager.GetPublicKey();
        publicKeyText.ForceLabelUpdate();

        // Display (hidden) private key.
        privateKeyField.text = walletManager.GetPrivateKey();
        privateKeyField.contentType = InputField.ContentType.Password;
        privateKeyField.ForceLabelUpdate();

        // Set up button listeners.
        toggleShowHideButton.onClick.AddListener(ToggleShowHide);
        transferButton.onClick.AddListener(TransferFunds);
    }

    public void ToggleShowHide()
    {
        isPrivateKeyVisible = !isPrivateKeyVisible;
        privateKeyField.contentType = isPrivateKeyVisible
            ? InputField.ContentType.Standard
            : InputField.ContentType.Password;
        privateKeyField.ForceLabelUpdate();
    }

    public void TransferFunds()
    {
        var recipientAddress = recipientAddressField.text;
        var amount = ulong.Parse(amountField.text);
        walletManager.TransferFunds(recipientAddress, amount);
    }
}