using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // enter col
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // desactivar el collider
            GetComponent<Collider2D>().enabled = false;
            // add ring
            GameManager.instance.addRing();
            // make sprite invisible
            GetComponent<SpriteRenderer>().enabled = false;
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);

        }

}
}
