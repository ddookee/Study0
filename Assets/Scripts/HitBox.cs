using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private Player.hitType hitType;
    Player player;
    Enemy enemy;

    bool isEnemy = false;
    
    // Start is called before the first frame update
    void Start()
    {
        string tag = transform.root.gameObject.tag;

        if (tag == "Enemy")
        {
            enemy = GetComponentInParent<Enemy>();
        }
        else
        {
            player = GetComponentInParent<Player>();            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemy == true)
        {
            //enemy.Trigg\
        }
        else
        { 
            player.TriggerEnter(hitType, collision);
        }
    }
}
