using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeSound : MonoBehaviour
{
    public AudioSource explodeSound;
    public AudioClip[] explodeClips;
    // Start is called before the first frame update
    void Start()
    {
        explodeSound.PlayOneShot(explodeClips[Random.Range(0, explodeClips.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
