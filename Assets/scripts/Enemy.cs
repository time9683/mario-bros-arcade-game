using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isWeak = false;
    public float detectionRadius = 1f; // Radio de detección para el jugador
    public float flyAwayForce = 10f; // Fuerza para alejar al enemigo

    enum Direction{
        Left = -1,
        Right = 1
    }

    Direction currentDirection = Direction.Left;
    private float velocity = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Animator animator = GetComponent<Animator>();
        if (isWeak)
        {
            animator.SetBool("IsWeak", true);
        }
        else
        {
            animator.SetBool("IsWeak", false);
        }

        DetectPlayerAndReact();
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



        private void DetectPlayerAndReact()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Player")
            {
                if (isWeak)
                {
                    // Aplicar fuerza para que el enemigo salga volando
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 flyDirection = (transform.position - hitCollider.transform.position).normalized;
                        rb.AddForce(flyDirection * flyAwayForce, ForceMode2D.Impulse);
                    }

                    // Desaparecer el enemigo después de un corto tiempo
                    // StartCoroutine(DisappearAfterDelay());
                }
                else{
                    // Hacer daño al jugador
                    Player player = hitCollider.GetComponent<Player>();
                    if (player != null) {
                        player.TakeDamage(1);
                    }
                }
            }
        }
    }


    // private IEnumerator DisappearAfterDelay()
    // {
    //     yield return new WaitForSeconds(1f); // Esperar 1 segundo
    //     Destroy(gameObject); // Destruir el enemigo
    // }


    private void Move(){
        // mover al enemigo hasta la direcion actual
        if (!isWeak){
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


    private void flipDirection(){
        currentDirection = currentDirection == Direction.Left ? Direction.Right : Direction.Left;
    }



}