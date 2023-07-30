using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Json;
using System.Text;

public class MintGameScoreNFT : MonoBehaviour
{
    // Wallet manager reference
    public SolanaWalletManager walletManager;
    
    // Player controller reference
    public PlayerController playerController;

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
            name = "Game Score NFT",
            symbol = "GSN",
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
        // TODO: Implement the minting code similar to your provided example using metadata as the NFT's metadata
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
