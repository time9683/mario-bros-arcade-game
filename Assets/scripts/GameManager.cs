using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // create a varible for instance
    public static GameManager instance;

    public int rings = 0;
    public TMPro.TextMeshProUGUI ringsText;
    public TMPro.TextMeshProUGUI lifeText;


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
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addRing(){
        rings++;
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


}
