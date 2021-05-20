using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("NextLevel"))
        {
         
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
       

            if (SceneManager.GetActiveScene().buildIndex < 3)
            {
                //Debug.Log(SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
            
        }
    }
}
