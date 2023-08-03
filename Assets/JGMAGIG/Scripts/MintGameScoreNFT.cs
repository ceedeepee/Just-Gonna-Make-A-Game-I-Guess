using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Json;
using System.Text;
using Solana.Unity.Metaplex.NFT.Library;
using Solana.Unity.Metaplex.Utilities;
using Solana.Unity.Programs;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Builders;
using Solana.Unity.Rpc.Core.Http;
using Solana.Unity.Rpc.Messages;
using Solana.Unity.Rpc.Models;
using Solana.Unity.SDK;

public class MintGameScoreNFT : MonoBehaviour
{
    // Wallet manager reference
    public SolanaWalletManager walletManager;
    
    // Player controller reference
    public PlayerController playerController;
    private static readonly IRpcClient rpcClient = ClientFactory.GetClient(Cluster.MainNet);

    // Server endpoint for metadata upload
    private string serverUploadEndpoint = "https://your-server-endpoint/upload";
    
    public async void MintNFT()
    {
        // Check if the player has approved the NFT minting.
        if (!playerController.gameOver)
        {
            return;
        }
        
        // Create the metadata for the NFT
        Metadata metadata = new Metadata()
        {
            name = "Solana Scroller Score",
            symbol = "SolScroll",
            uri = await UploadMetadataToServer(),
            sellerFeeBasisPoints = 0,
            creators = new List<Creator> { new Creator(walletManager.GetPublicKey(), 100, true) }
        };
        
        // Call the minting function
        await Mint(metadata);
    }
    
    private async Task<string> UploadMetadataToServer()
    {
        // TODO: Replace this with actual game data
        string gameData = $"Score: {playerController.objectsBlownUp}, Distance Traveled: {playerController.DistanceTraveled()}";

        // Convert game data to a byte array
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes(gameData));

        using (HttpClient client = new HttpClient())
        {
            var response = await client.PostAsync(serverUploadEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                UUIDResponse uuidResponse = JsonUtility.FromJson<UUIDResponse>(responseContent);
                
                // Return the UUID from the server response
                return uuidResponse.uuid;
            }
            else
            {
                throw new Exception($"Failed to upload metadata to the server. Status code: {response.StatusCode}");
            }
        }
    }

    [System.Serializable]
    public class UUIDResponse
    {
        public string uuid;
    }

    private async Task Mint(Metadata metadata)
    {
        var mint = walletManager.wallet.Account;

        Task<RequestResult<ResponseValue<LatestBlockHash>>> blockHash = rpcClient.GetLatestBlockHashAsync();
        var associatedTokenAccount = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Web3.Account, walletManager.wallet.Account.PublicKey);
        var transaction = new TransactionBuilder()
            .SetRecentBlockHash(blockHash.ToString())
            .SetFeePayer(Web3.Account)
            .AddInstruction(
                SystemProgram.CreateAccount(
                    Web3.Account,
                    mint.PublicKey,
                    (ulong).002f,
                    TokenProgram.MintAccountDataSize,
                    TokenProgram.ProgramIdKey))
            .AddInstruction(
                TokenProgram.InitializeMint(
                    mint.PublicKey,
                    0,
                    Web3.Account,
                    Web3.Account))
            .AddInstruction(
                AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                    Web3.Account,
                    Web3.Account,
                    mint.PublicKey))
            .AddInstruction(
                TokenProgram.MintTo(
                    mint.PublicKey,
                    associatedTokenAccount,
                    1,
                    Web3.Account))
            .AddInstruction(MetadataProgram.CreateMetadataAccount(
                PDALookup.FindMetadataPDA(mint), 
                mint.PublicKey, 
                Web3.Account, 
                Web3.Account, 
                Web3.Account.PublicKey, 
                metadata,
                TokenStandard.NonFungible, 
                true, 
                true, 
                null,
                metadataVersion: MetadataVersion.V3))
            .AddInstruction(MetadataProgram.CreateMasterEdition(
                    maxSupply: null,
                    masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
                    mintKey: mint,
                    updateAuthorityKey: Web3.Account,
                    mintAuthority: Web3.Account,
                    payer: Web3.Account,
                    metadataKey: PDALookup.FindMetadataPDA(mint),
                    version: CreateMasterEditionVersion.V3
                )
            );
    }
    
    [System.Serializable]
    public class Metadata
    {
        public string name;
        public string symbol;
        public string uri;
        public int sellerFeeBasisPoints;
        public List<Creator> creators;
    }

    [System.Serializable]
    public class Creator
    {
        public string address;
        public int share;
        public bool verified;

        public Creator(string address, int share, bool verified)
        {
            this.address = address;
            this.share = share;
            this.verified = verified;
        }
    }
}
