using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isWeak = false;
    private bool isDead = false;
    public float detectionRadius = 1f; // Radio de detección para el jugador
    public float flyAwayForce = 10f; // Fuerza para alejar al enemigo
    // clipsound
    public AudioClip DeadSound;

    enum Direction{
        Left = -1,
        Right = 1
    }

    Direction currentDirection = Direction.Left;
    private float velocity = 2f;
    public GameObject ringPrefab;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
       Animator animator = GetComponent<Animator>();
       animator.SetBool("IsWeak", isWeak);
       animator.SetBool("IsDead", isDead);




        Move();
        CheckScreenEdge();
        
    }

    // Cambiar el estado del enemigo a "débil"
    public void ChangeStateToWeak(){

        // quitar la corrutina
        StopAllCoroutines();
        isWeak = !isWeak;
        if (isWeak){
            StartCoroutine(ChangeStateToWeakCoroutine());
        }
        
        if(!isWeak){
            flipDirection();
        }
        
    }

    // coroutine to change the state of the enemy
    IEnumerator ChangeStateToWeakCoroutine(){
        yield return new WaitForSeconds(5);
        isWeak = false;
        flipDirection();
    }




    private void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.tag == "Player"){

            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null){

                if (!isWeak){
                    // direction a la que se podrocera el knockback
                    Vector2 direction = (player.transform.position - transform.position).normalized;
                    player.TakeDamage(direction);
                }else{

                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 flyDirection = (transform.position - player.transform.position).normalized;
                       
                       
                        // instaciar el anillo en la posicion del enemigo
                        Instantiate(ringPrefab, transform.position, Quaternion.identity);
                        rb.AddForce(flyDirection * flyAwayForce, ForceMode2D.Impulse);
                    }
                    
                    isDead = true;
                    // reproducir el sonido de muerte
                    // get the audio source
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.clip = DeadSound;
                    audioSource.Play();
                    Debug.Log("Dead");
                    // desactivar el collider
                    GetComponent<Collider2D>().enabled = false;
                    StartCoroutine(DisappearAfterDelay());

                }



            }
      }else  if (collision.gameObject.tag == "enemy")
        {
            flipDirection();
        }
    }


    // cuando el jugador sale de la colision llamar stop damage
    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Player"){
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null){
                player.StopDamage();
            }
        }
    }



    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Esperar 1 segundo
        Destroy(gameObject); // Destruir el enemigo
        // call game manager to check enemies
        GameManager.instance.CheckEnemies();

        
        // instanciar el anillo
        // get gameObject by tag enemySpawn
        GameObject enemySpawn = GameObject.FindGameObjectWithTag("enemySpawn");
        // log for debug
        Debug.Log(enemySpawn.transform.position);
        // 
        // colocar el anillo en la posicion del spawn
        Instantiate(ringPrefab, enemySpawn.transform.position, Quaternion.identity);
        // log for debug
        Debug.Log("Ring");
    }


    private void Move(){
        // mover al enemigo hasta la direcion actual
        if (!isWeak && !isDead){
        transform.position += new Vector3((int)currentDirection * velocity * Time.deltaTime, 0, 0);
       
        // segun el movimientoc cambiar la direccion del sprite
        if((int)currentDirection < 0){
            transform.localScale = new Vector3(1, 1, 1);
        }else{
            transform.localScale = new Vector3(-1, 1, 1);
        }
       
        }
    }


    // si el enemigo llega al borde de la pantalla, cambiar la dirección y transportar la direcion y a 2.69y, y la x contraria
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


    public void flipDirection(){
        currentDirection = currentDirection == Direction.Left ? Direction.Right : Direction.Left;
    }




}