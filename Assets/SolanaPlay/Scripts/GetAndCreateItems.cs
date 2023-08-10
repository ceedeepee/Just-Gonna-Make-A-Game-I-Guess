using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetAndCreateItems : MonoBehaviour
{
    public Transform itemParent;
    public GameObject itemPrefab;
    
    public SolanaPayInterface solanaPayInterface;
    private void OnEnable()
    {
        solanaPayInterface = GameObject.Find("SolanaPayInterface").GetComponent<SolanaPayInterface>();
        StartCoroutine(solanaPayInterface.GetItems((items) =>
        {
            foreach (var item in items)
            {
                foreach (var priceInfo in item.priceArray)
                {
                    //Debug.Log(item.description);
                    GameObject itemInstance = Instantiate(itemPrefab, itemParent);
                    itemInstance.SetActive(true);
                    ItemObject itemObj = itemInstance.GetComponent<ItemObject>();
                    itemObj.itemName.text = item.label;
                    itemObj.itemValue.text = priceInfo.price.ToString();
                    itemObj.itemDescription.text = item.description;
                    itemObj.splToken.text = priceInfo.tokenName;
                    itemObj.itemId = item.itemId;
                    itemObj.platformId = item.platformId;
                    itemObj.token = priceInfo.token;
                    StartCoroutine(solanaPayInterface.GetImage(item.image, (texture) =>
                    {
                        itemObj.itemImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    }));
                }
            }
        }));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}