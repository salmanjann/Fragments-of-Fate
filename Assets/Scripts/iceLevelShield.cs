using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class iceLevelShield : MonoBehaviour
{
    // prefab of shield that will be spawned
    public GameObject shield;
     // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.CompareTag("Player"))
        {
            GameObject temp = Instantiate(shield);
            temp.transform.position = obj.gameObject.transform.position;
            temp.transform.parent = obj.gameObject.transform;
            Destroy(this.gameObject);
        }
    }
}
