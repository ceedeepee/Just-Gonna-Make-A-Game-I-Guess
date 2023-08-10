using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameAsset/GameAssetData")]
public class GameAssetData : ScriptableObject
{
    public string name;
    public Sprite Sprite;
    public float Value;
    public string Rarity;
    public string image;
    public string id;
}
