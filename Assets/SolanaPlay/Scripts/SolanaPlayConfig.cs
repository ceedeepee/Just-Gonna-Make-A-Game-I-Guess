using UnityEngine;

[CreateAssetMenu(fileName = "SolanaPlayConfig", menuName = "SolanaPlay/Configuration", order = 1)]
public class SolanaPlayConfig : ScriptableObject {
    public string apiEndpoint;
    public string platformId;
    public string[] acceptedDigitalAssets;
    // Add any additional configuration fields here
}
