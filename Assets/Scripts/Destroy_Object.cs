using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Object : MonoBehaviour
{
    private static int buttonPressed = 0;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        buttonPressed++;
        Debug.Log(buttonPressed);

        if (buttonPressed == 3)
        {
            anim.SetBool("isOpen", true);
        }
    }
}