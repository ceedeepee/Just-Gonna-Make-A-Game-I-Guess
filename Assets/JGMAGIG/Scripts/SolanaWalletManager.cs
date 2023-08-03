using Solana.Unity.Programs;
using Solana.Unity.Programs.Models;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Wallet;
using Solana.Unity.Wallet.Bip39;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SolanaWalletManager : MonoBehaviour
{
    public Wallet wallet;
    private IRpcClient rpcClient;
    private const string MnemonicKey = "MNEMONIC";
    private const string PrivateKeyKey = "PRIVATEKEY";

    void Start()
    {
        LoadWallet();
    }

    public string GetPublicKey()
    {
        return wallet?.Account.PublicKey.Key;
    }

    public string GetPrivateKey()
    {
        return wallet?.Account.PrivateKey.Key;
    }

    public void LoadWallet()
    {
        string mnemonic = PlayerPrefs.GetString(MnemonicKey, "");
        if (!string.IsNullOrEmpty(mnemonic))
        {
            wallet = new Wallet(mnemonic, WordList.English);
            rpcClient = ClientFactory.GetClient(Cluster.TestNet);
            Debug.Log(GetPublicKey());
        }
        else
        {
            CreateWallet();
        }
    }

    public void CreateWallet()
    {
        wallet = new Wallet(WordCount.TwentyFour, WordList.English);
        PlayerPrefs.SetString(MnemonicKey, wallet.Mnemonic.ToString());
        PlayerPrefs.SetString(PrivateKeyKey, wallet.Account.PrivateKey.Key);
        rpcClient = ClientFactory.GetClient(Cluster.TestNet);
        Debug.Log(GetPublicKey());
    }

    public async void TransferFunds(string recipientPublicKey, ulong amount)
    {
        Account fromAccount = wallet.GetAccount(0); // Adjust as needed
        PublicKey toPublicKey = new PublicKey(recipientPublicKey);

        var blockHash = await rpcClient.GetRecentBlockHashAsync();

        byte[] transaction = new TransactionBuilder()
            .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
            .SetFeePayer(fromAccount)
            .AddInstruction(SystemProgram.Transfer(fromAccount.PublicKey, toPublicKey, amount))
            .Build(fromAccount);

        await rpcClient.SendTransactionAsync(transaction);
    }
}
