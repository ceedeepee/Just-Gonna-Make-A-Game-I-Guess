using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAD(gameObject, delay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DestroyAD(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider col)
    {
        if(col.gameObject.tag == "Obstacle")
        {
            col.gameObject.SendMessage("blowUp");
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground" || col.gameObject.tag == "Obstacle")
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; 
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground" || col.gameObject.tag == "Obstacle")
        {
            gameObject.GetComponent<Collider2D>().enabled = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; 
        }
    }
    
}
