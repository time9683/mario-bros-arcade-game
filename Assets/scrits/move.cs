using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    enum Direction{
        Left = -1,
        None=0,
        Right = 1
    }
    Direction currentDirection = Direction.None;
    public float speed;
    public float jumpForce = 5f;

    public LayerMask groundLayer;
    public  float heightRaycast = 0.1f;
    private bool isGrounded;

    public Animator animator;

     Rigidbody2D rb2d;

    private void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        currentDirection = Direction.None;
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

    private void FixedUpdate(){
        Vector2 velocity = new Vector2((float)currentDirection * speed,rb2d.velocity.y);
        animator.SetFloat("movement", velocity.x);
        animator.SetFloat("jump", velocity.y);
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

    }

    void Jump(){
        Vector2 jumpVelocity = new Vector2(0, jumpForce);
        rb2d.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * heightRaycast);
    }
}
