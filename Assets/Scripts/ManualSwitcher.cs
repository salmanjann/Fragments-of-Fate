using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManualSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     SceneManager.LoadScene("Level_5");
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     SceneManager.LoadScene("Level_5");
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha5))
        // {
        //     SceneManager.LoadScene("Level_2");
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha5))
        // {
        //     SceneManager.LoadScene("Level_3");
        // }

        // if(Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     SceneManager.LoadScene("Level_5");
        // }
        
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("Level_5");
        }
    }
}
