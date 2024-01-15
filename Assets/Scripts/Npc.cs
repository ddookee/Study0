using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == GameTag.Player.ToString())
        {

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
