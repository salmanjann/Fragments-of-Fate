using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceBats : MonoBehaviour
{
    // vision of the bat
    public CircleCollider2D vision;
    // initial position of the bat
    private Vector2 startPosition;
    // this is the Animator on the bat to make sure its animations
    private Animator bat_animator;
    // Start is called before the first frame update
    void Start()
    {
        bat_animator = this.GetComponent<Animator>();
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D()
    {

    }
}
