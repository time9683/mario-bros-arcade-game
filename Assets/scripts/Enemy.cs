using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isWeak = false;
    public float detectionRadius = 1f; // Radio de detección para el jugador
    public float flyAwayForce = 10f; // Fuerza para alejar al enemigo

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
        
    }

    // Cambiar el estado del enemigo a "débil"
    public void ChangeStateToWeak(){

        // quitar la corrutina
        StopAllCoroutines();
        isWeak = !isWeak;
    }

    // coroutine to change the state of the enemy
    IEnumerator ChangeStateToWeakCoroutine(){
        yield return new WaitForSeconds(5);
        isWeak = false;
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

}
