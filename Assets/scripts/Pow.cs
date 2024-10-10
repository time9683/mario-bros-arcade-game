using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pow : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Duración del temblor
    public float shakeMagnitude = 0.1f; // Magnitud del temblor
    
    private int Uses = 3;
    private bool isUsed = false;
    // variable for animator
    private Animator animator;
    


    void Start()
    {
        // pause animation
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //  detecta cuando Player esta encima de Pow
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player")
        {
            // detectar si el player lo toca por arriba
            if (collision.contacts[0].normal.y < 0) return;
        
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject enemy in enemies)
            {
                // Cambiar el estado del enemigo a "débil"
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.ChangeStateToWeak();
                }
            }
                    if (animator != null && animator.enabled == false){
                        animator.enabled = true;
                    }
                 Uses--;
                 StartCoroutine(ShakeCamera());
                if (Uses == 1){
                    isUsed = true;
                }

                if (Uses == 0){
                    Destroy(gameObject);
                }



            animator.SetBool("isUsed", isUsed);

        }

             


        }
    



    private IEnumerator ShakeCamera()
    {
        Vector3 originalPosition = Camera.main.transform.position;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            Camera.main.transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = originalPosition;
    }


}
