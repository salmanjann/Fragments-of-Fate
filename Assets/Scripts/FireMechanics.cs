using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireMechanics : MonoBehaviour
{
    // beam heat collider
    private BoxCollider2D heat;
    // Start is called before the first frame update
    void Start()
    {
        heat = this.AddComponent<BoxCollider2D>();
        heat.offset = Vector2.zero;
        heat.size = new Vector2(24f,24f);
        heat.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(heat != null && heat.IsTouching(obj) && obj.CompareTag("Ice"))
        {
            var temp = obj.gameObject.transform.localScale;
            obj.gameObject.transform.localScale = temp/1.1f;
            if(temp.x <0.01f)
            {
                Destroy(obj);
            }
        }
    }
}
