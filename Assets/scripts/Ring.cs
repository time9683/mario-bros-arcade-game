using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private AudioSource audioSource;

        enum Direction{
        Left = -1,
        Right = 1
    }

    Direction currentDirection = Direction.Left;
    private float velocity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        
        Move();
        CheckScreenEdge();
                
    }

    // enter col
    private void OnCollisionEnter2D(Collision2D collision)
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


        private void Move(){
        // mover al enemigo hasta la direcion actual
        transform.position += new Vector3((int)currentDirection * velocity * Time.deltaTime, 0, 0);
       
        // segun el movimientoc cambiar la direccion del sprite
        if((int)currentDirection < 0){
            transform.localScale = new Vector3(1, 1, 1);
        }else{
            transform.localScale = new Vector3(1, 1, 1);
        }
       
        
    }


        private void CheckScreenEdge(){
        float screenEdgeX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        
        if(transform.position.x > screenEdgeX){
            currentDirection = Direction.Left;
            transform.position = new Vector3(screenEdgeX, 2.69f, 0);
        }else if(transform.position.x < -screenEdgeX){
            currentDirection = Direction.Right;
            transform.position = new Vector3(-screenEdgeX, 2.69f, 0);
        }
    }
}
