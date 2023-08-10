using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Json;
using System.Text;
// using Solana.Unity.Metaplex.NFT.Library;
// using Solana.Unity.Metaplex.Utilities;
// using Solana.Unity.Programs;
// using Solana.Unity.Rpc;
// using Solana.Unity.Rpc.Builders;
// using Solana.Unity.Rpc.Core.Http;
// using Solana.Unity.Rpc.Messages;
// using Solana.Unity.Rpc.Models;
// using Solana.Unity.SDK;
// using Solana.Unity.Wallet;

public class MintGameScoreNFT : MonoBehaviour
{
    // Wallet manager reference
    // public SolanaWalletManager walletManager;
    
    // Player controller reference
    public PlayerController playerController;
    // private static readonly IRpcClient rpcClient = ClientFactory.GetClient("");

    // Server endpoint for metadata upload
    private string serverUploadEndpoint = "https://your-server-endpoint/upload";
    
    public async void MintNFT()
    {
        // Check if the player has approved the NFT minting.
        if (!playerController.gameOver)
        {
            Debug.Log("Game is not over yet, cannot mint NFT.");
            return;
        }
        
        // Create the metadata for the NFT
        // Metadata metadata = new Metadata()
        // {
        //     name = "Solana Scroller Score",
        //     symbol = "SolScroll",
        //     uri = await UploadMetadataToServer(),
        //     sellerFeeBasisPoints = 0,
        //     creators = new List<Creator> { new Creator(walletManager.wallet.Account.PublicKey, 100, true) }
        // };
        // Debug.Log($"Minting NFT with metadata: {JsonUtility.ToJson(metadata)}");
        //
        // // Call the minting function
        // await Mint(metadata);
    }
    
    private async Task<string> UploadMetadataToServer()
    {
        // TODO: Replace this with actual game data
        string gameData = $"Score: {playerController.objectsBlownUp}, Distance Traveled: {playerController.DistanceTraveled()}";

        // Convert game data to a byte array
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes(gameData));

        using (HttpClient client = new HttpClient())
        {
// //            var response = await client.PostAsync(serverUploadEndpoint, content);
//
//             if (response.IsSuccessStatusCode)
//             {
//                 var responseContent = await response.Content.ReadAsStringAsync();
//                 UUIDResponse uuidResponse = JsonUtility.FromJson<UUIDResponse>(responseContent);
//                 
//                 // Return the UUID from the server response
                return "uuidResponse.uuid";
            // }
            // else
            // {
            //     throw new Exception($"Failed to upload metadata to the server. Status code: {response.StatusCode}");
            // }
        }
    }

    [System.Serializable]
    public class UUIDResponse
    {
        public string uuid;
    }

    // private async Task Mint(Metadata metadata)
    // {
    //
    //     Debug.Log("Starting Minting NFT...");
    //     var mint = new Account();
    //     Debug.Log($"newMint: {mint.PublicKey}");
    //     var blockHashResult = await rpcClient.GetLatestBlockHashAsync();
    //     var minimumRentResult = await rpcClient.GetMinimumBalanceForRentExemptionAsync(679);
    //     var minimumRent = minimumRentResult.Result;
    //     Debug.Log($"Block hash: {blockHashResult.Result.Value.Blockhash}, minimum rent: {minimumRent}");
    //     var associatedTokenAccount = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(walletManager.wallet.Account.PublicKey, mint.PublicKey);
    //     Debug.Log($"Associated token account: {associatedTokenAccount}");
    //
    //     var transaction = new TransactionBuilder()
    //         .SetRecentBlockHash(blockHashResult.ToString())
    //         .SetFeePayer(walletManager.wallet.Account.PublicKey)
    //         .AddInstruction(
    //             SystemProgram.CreateAccount(
    //                 walletManager.wallet.Account.PublicKey,
    //                 mint.PublicKey,
    //                 minimumRent,
    //                 TokenProgram.MintAccountDataSize,
    //                 TokenProgram.ProgramIdKey))
    //         .AddInstruction(
    //             TokenProgram.InitializeMint(
    //                 mint.PublicKey,
    //                 0,
    //                 walletManager.wallet.Account.PublicKey,
    //                 walletManager.wallet.Account.PublicKey))
    //         .AddInstruction(
    //             AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
    //                 walletManager.wallet.Account.PublicKey,
    //                 walletManager.wallet.Account.PublicKey,
    //                 mint.PublicKey))
    //         .AddInstruction(
    //             TokenProgram.MintTo(
    //                 mint.PublicKey,
    //                 associatedTokenAccount,
    //                 1,
    //                 walletManager.wallet.Account.PublicKey))
    //         .AddInstruction(MetadataProgram.CreateMetadataAccount(
    //             PDALookup.FindMetadataPDA(mint), 
    //             mint.PublicKey, 
    //             walletManager.wallet.Account.PublicKey, 
    //             walletManager.wallet.Account.PublicKey, 
    //             walletManager.wallet.Account.PublicKey, 
    //             metadata,
    //             TokenStandard.NonFungible, 
    //             true, 
    //             true, 
    //             null,
    //             metadataVersion: MetadataVersion.V3))
    //         .AddInstruction(MetadataProgram.CreateMasterEdition(
    //                 maxSupply: null,
    //                 masterEditionKey: PDALookup.FindMasterEditionPDA(mint),
    //                 mintKey: mint,
    //                 updateAuthorityKey: walletManager.wallet.Account.PublicKey,
    //                 mintAuthority: walletManager.wallet.Account.PublicKey,
    //                 payer: walletManager.wallet.Account.PublicKey,
    //                 metadataKey: PDALookup.FindMetadataPDA(mint),
    //                 version: CreateMasterEditionVersion.V3
    //             )
    //         );
    //
    //
    //     Debug.Log("Minting NFT...");
    // }
    
    // [System.Serializable]
    // // public class Metadata
    // // {
    // //     public string name;
    // //     public string symbol;
    // //     public string uri;
    // //     public int sellerFeeBasisPoints;
    // //     public List<Creator> creators;
    // // }
    //
    // [System.Serializable]
    // public class Creator
    // {
    //     public string address;
    //     public int share;
    //     public bool verified;
    //
    //     public Creator(string address, int share, bool verified)
    //     {
    //         this.address = address;
    //         this.share = share;
    //         this.verified = verified;
    //     }
    // }
}
