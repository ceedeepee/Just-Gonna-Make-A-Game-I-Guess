using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SolanaPlayDefinitions : MonoBehaviour
{
public class gameInfo
    {
        public string type;
        public string name;
        public string auth;
        public string owner;
        public List<string> @params = new List<string>();
        public List<string> jparams = new List<string>();
    }    
    public class Progress
    {
        public List<string> @params = new List<string>();
        public List<string> jparams = new List<string>();
        public int round = 1;
    }
    public class gameProgress
    {
        public string type;
        public string name;
        public string auth;
        public string owner;
        public Progress progress = new Progress();
    }
    public partial class GotMessage
    {
        public static GotMessage FromJson(string json) => JsonConvert.DeserializeObject<GotMessage>(json, SolanaPayInterface.Converter.Settings);
    }
    public partial class GotMessage
    {
        [JsonProperty("success")] public bool success;
        [JsonProperty("message")] public string message;
        [JsonProperty("result")] public GotResult result;
    }
    public partial class GotResult
    {
        public bool acknowledged;
        public int modifiedCount;
        public string upsertedId;
        public int upsertedCount;
        public int matchedCount;
    }
    public partial class POGs
    {
        public static POGs FromJson(string json) => JsonConvert.DeserializeObject<POGs>(json, SolanaPayInterface.Converter.Settings);
    }
    public partial class POGs
    {
        [JsonProperty("pogsOKBPE")] public GameAsset[] pogsOKBPE;
        [JsonProperty("pogsOKBCE")] public GameAsset[] pogsOKBCE;
        [JsonProperty("pogsPogman")] public GameAsset[] pogsPogman;
        [JsonProperty("pogsFounding")] public GameAsset[] pogsFounding;
    }
    public partial class GameAsset
    {
        [JsonProperty("name")]
        public string name;
        [JsonProperty("id")]
        public string id;
        [JsonProperty("image")]
        public string image;
        [JsonProperty("attributes")]
        public Attribute[] attributes;
    }    
    public partial class Attribute
    {
        [JsonProperty("trait_type")]
        public string trait_type;  
        [JsonProperty("value")]
        public string value;
    }
    public partial class MemberStub
    {
        public static MemberStub FromJson(string json) => JsonConvert.DeserializeObject<MemberStub>(json, SolanaPayInterface.Converter.Settings);
    }

    public partial class MemberStub
    {
        [JsonProperty("success")]
        public bool success = true;
        [JsonProperty("wallet")]
        public string wallet;
        [JsonProperty("cpubkey")]
        public string cpubkey;
        [JsonProperty("name")]
        public string name;
        [JsonProperty("username")]
        public string username;
    }
    public partial class walletData
    {
        public static walletData FromJson(string json) => JsonConvert.DeserializeObject<walletData>(json, SolanaPayInterface.Converter.Settings);
    }

    public partial class walletData
    {
        [JsonProperty("linkedAccountData")]
        public MemberStub linkedAccountData;
    }
    public class Item
    {
        public string platformId { get; set; }
        public string reference { get; set; }
        public string platformRefId { get; set; }
        public string label { get; set; }
        public string itemId { get; set; }
        public string image { get; set; }
        public List<Price> priceArray { get; set; }
        public string description { get; set; }
        public bool inStock { get; set; }
        public long createdUTC { get; set; }
        public string lastModified { get; set; }
    }

    public class Price
    {
        public string tokenName { get; set; }
        public double price { get; set; }
        public string token { get; set; }
        public bool active { get; set; }
    }

    public partial class BuyResponse
    {
        public static BuyResponse FromJson(string json) => JsonConvert.DeserializeObject<BuyResponse>(json, SolanaPayInterface.Converter.Settings);
    }

    public partial class BuyResponse
    {
        [JsonProperty("label")]
        public string label { get; set; }
    
        [JsonProperty("icon")]
        public string icon { get; set; }
    
        [JsonProperty("link")]
        public string link { get; set; }
    
        [JsonProperty("qrRefId")] // Add this line
        public string qrRefId { get; set; } // And this line
    }

}
