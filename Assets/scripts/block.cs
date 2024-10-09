using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{

    // radio de detección de enemigos
    public float detectionRadius = 1.5f;
    public bool isMoving = false;
    private Vector2 initialPosition;



    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // make colision enter
    private void OnCollisionEnter2D(Collision2D collision){
        // if the object that colide is the player and colide from the bottom move the block to the up
        if (collision.gameObject.tag == "Player" && collision.contacts[0].normal.y > 0 && !isMoving)
        {
            
            StartCoroutine(MoveBlock(collision.contacts[0].point));


        }
}


// corrutine to move the block to the up
IEnumerator MoveBlock(Vector2 contactPoint)
{
    isMoving = true;
    // get the new position of the block (move up by 0.5 units instead of 1)
    Vector2 newPos = new Vector2(initialPosition.x, initialPosition.y + 0.5f);
        // Mover el bloque a la nueva posición
        while (Vector2.Distance(transform.position, newPos) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, newPos, 0.05f);
            yield return null;
        }

        // Detectar enemigos cercanos y cambiar su estado a "débil"
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(newPos, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "enemy")
            {
                // Cambiar el estado del enemigo a "débil"
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ChangeStateToWeak();
                }
            }
        }

        // Retornar el bloque a la posición original
        while (Vector2.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, initialPosition, 0.05f);
            yield return null;
        }
        isMoving = false;
    }
} 



