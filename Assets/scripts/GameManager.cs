using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // create a varible for instance
    public static GameManager instance;
    private static int MAXPoints = 0;


    public int rings = 0;
    public TMPro.TextMeshProUGUI ringsText;
    public TMPro.TextMeshProUGUI lifeText;
    public TMPro.TextMeshProUGUI MAXPoinsText;

    // audioclip win
    public AudioClip winSound;
    // audioclip loose
    public AudioClip looseSound;
    // image win
    public GameObject winImage;
    // image loose
    public GameObject looseImage;


    void Awake(){

        // if the instance is null set this to the instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        // if the instance is not null destroy this
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {   


        if (ringsText == null)
        {
            Debug.LogError("ringsText no está asignado en el inspector.");
        }

        if (lifeText == null)
        {
            Debug.LogError("lifeText no está asignado en el inspector.");
        }

    
        if (MAXPoinsText == null){
            Debug.LogError("MAXPoinsText no está asignado en el inspector.");
        }

        // desactivar win and loose image
        winImage.SetActive(false);
        looseImage.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addRing(){
        rings++;
        // update MAXPoints
        if (rings > MAXPoints){
            MAXPoints = rings;
            MAXPoinsText.text = MAXPoints.ToString();
        }
        UpdateRingUI();
    }


    private void UpdateRingUI(){

        if (ringsText != null)
        {
            ringsText.text = rings.ToString();
        }
    }


    public void updateLifeUI(int life){
        if (lifeText != null){
            lifeText.text = life.ToString();
        }
    }


    public void CheckEnemies(){
        StartCoroutine(CheckEnemiesC());
    }

    private IEnumerator CheckEnemiesC(){
        
        yield return new WaitForSeconds(0.5f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length == 0){
            if (winSound != null){
                PlaySound(winSound);
                // show win image
                winImage.SetActive(true);
                // force to show the image
                winImage.GetComponent<SpriteRenderer>().enabled = true;

            }
            
            StartCoroutine(RestartLevel());
        }

    }

    public void GameOver(){
        if (looseSound != null){
            PlaySound(looseSound);
            looseImage.SetActive(true);
            
            
        }



        StartCoroutine(RestartLevel());
    }




    private IEnumerator RestartLevel(){

        yield return new WaitForSeconds(4f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    
        // reset the rings
        rings = 0;
        UpdateRingUI();
        // obtener el elemento canvas con la etiqueta "UI"
        GameObject canvas = GameObject.FindWithTag("UI");
        // obtener camara principal
        Camera camera = Camera.main;
        // asignar renderer camara al canvas
        canvas.GetComponent<Canvas>().worldCamera = camera;
        // vaciar texto 
        // desactive spriteRendere image loose
        looseImage.SetActive(false);
        // desactive spriteRendere image win
        winImage.SetActive(false);


    }

 

    private void PlaySound(AudioClip sound){
        if (sound != null){
            // get the audio source
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();
        }

    }

}
