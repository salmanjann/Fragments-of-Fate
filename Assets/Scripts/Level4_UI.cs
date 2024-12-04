using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Level4_UI : MonoBehaviour
{
    public TextMeshProUGUI shardsCollected;
    public TextMeshProUGUI shardsError;
    public int shardsCount =0;
    // Start is called before the first frame update
    void Start()
    {
        shardsCollected.text = shardsCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCount(){
        shardsCount++;
        shardsCollected.text = shardsCount.ToString();
    }

    public void ShowShardsError(){
        shardsError.text = "Collect All Shards!";
        Invoke("RemoveError",5f);
    }

    public void RemoveError(){
        shardsError.text = "";
    }

    public void ShowLevelComplete(){
        shardsError.color = new Color(0,246,0,1f);
        shardsError.text = "Level Completed";
    }
}
