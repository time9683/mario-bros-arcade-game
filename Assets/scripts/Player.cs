using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
  enum Direction{
        Left = -1,
        None=0,
        Right = 1
    }
    Direction currentDirection = Direction.None;
    public float MaxSpeed = 5f;
    public float speed;
    public float acceleration = 0.05f;

    public float jumpForce = 5f;

    public LayerMask groundLayer;
    public  float heightRaycast = 0.1f;
    private bool isGrounded;
    public Animator animator;

    
    private Dictionary<string, AudioClip> AudioClips;
    
    
    private AudioSource audioSource;

    public AudioClip HurtSound;
    public AudioClip jumpSound;





     Rigidbody2D rb2d;
     private bool isDead = false;
     private bool isHurt = false;
     private int life = 3;


    private void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

          if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D no está asignado.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource no está asignado.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator no está asignado.");
        }

    }

    // Start is called before the first frame update
    void Start()
    {

        AudioClips = new Dictionary<string, AudioClip>(){
            {"Hurt", HurtSound},
            {"Jump", jumpSound}
        };

        if(GameManager.instance != null){
        
        GameManager.instance.updateLifeUI(life);
        }

         if (HurtSound == null)
        {
            Debug.LogError("HurtSound no está asignado.");
        }

        if (jumpSound == null)
        {
            Debug.LogError("jumpSound no está asignado.");
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        currentDirection = Direction.None;

      if(!isDead){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            Jump();
        }

        if(Input.GetKey(KeyCode.A))
        {
            currentDirection = Direction.Left;
        }

        if(Input.GetKey(KeyCode.D))
        {
            currentDirection = Direction.Right;
        }
      }

    }

    private void FixedUpdate(){

        if(currentDirection != Direction.None){
            speed += acceleration;
            speed = Mathf.Clamp(speed, 0, MaxSpeed);
        }else{
            speed = 1;
        }


        Vector2 velocity = new Vector2((float)currentDirection * speed,rb2d.velocity.y);
        animator.SetFloat("movement", velocity.x);
        animator.SetBool("isDeath", isDead);
        rb2d.velocity =  velocity;
        if(velocity.x < 0.0){
            
             transform.localScale = new Vector3(-1, 1, 1);
            
        }else{
            
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, heightRaycast, groundLayer);
       isGrounded = hit.collider != null;
       
       if( velocity.y > 0 ){
       animator.SetBool("isground", isGrounded);
       }else if(isGrounded){ 
            animator.SetBool("isground", isGrounded);
       }


         CheckScreenEdge();

    }

    void Jump(){
        PlaySound("Jump");
        Vector2 jumpVelocity = new Vector2(0, jumpForce);
        rb2d.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * heightRaycast);
    }



    public void TakeDamage(Vector2 direction){
        if (!isHurt && !isDead){
            isHurt = true;
            PlaySound("Hurt");
            life -= 1;
            GameManager.instance.updateLifeUI(life);
            rb2d.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
            // desactivar el collider del jugador directamnete
            isDead = true; 
            GetComponent<Collider2D>().enabled = false;

        }

    }


    public void StopDamage(){
        isHurt = false;
        rb2d.velocity = Vector2.zero;
    }





    private void PlaySound(string soundName){


        if(AudioClips.ContainsKey(soundName)){
            audioSource.clip = AudioClips[soundName];
            audioSource.Play();
        }



    }


     private void CheckScreenEdge()
    {

        float screenEdgeX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;


        if (transform.position.x > screenEdgeX)
        {
            // Transportar al jugador al lado izquierdo de la pantalla
            transform.position = new Vector3(-screenEdgeX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -screenEdgeX)
        {
            // Transportar al jugador al lado derecho de la pantalla
            transform.position = new Vector3(screenEdgeX, transform.position.y, transform.position.z);
        }
    }

    // create a coroutine to respawn the player

    public void fallPlayer(){
            if(life <= 0){
                GameManager.instance.GameOver();
                // desactivar el collider
                // GetComponent<Collider2D>().enabled = false;
                return;
            }   

            StartCoroutine(RewSpawn());


    }






    private IEnumerator RewSpawn(){
        yield return new WaitForSeconds(2f);
        isDead = false;
        // only RewSpawn if remaining life
        if(life > 0){
            // obtener el elemento con la etiqueta "Respawn"
            GameObject respawn = GameObject.FindWithTag("Respawn");
            // mover al jugador a la posición del objeto "Respawn"
            transform.position = respawn.transform.position;
            // activar el collider
            GetComponent<Collider2D>().enabled = true;
        }

    }

}