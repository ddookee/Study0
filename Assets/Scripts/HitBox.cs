using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private Player.hitType hitType;
    [SerializeField] private Enemy.hitType eHitType;
    Player player;
    Enemy enemy;

    bool isEnemy = false;
    
    // Start is called before the first frame update
    void Start()
    {
        string tag = transform.root.gameObject.tag;//자신이 속한 루트의 태그

        if (tag == "Enemy")//태그가 애너미라면 Enemy스크립트에서 가져오고
        {
            isEnemy = true;
            enemy = GetComponentInParent<Enemy>();
        }
        else//태그가 플레이어라면 Player스크립트에서 가져옴
        {
            player = GetComponentInParent<Player>();            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemy == true)
        {
            enemy.TriggerEnter(eHitType, collision);
        }
        else
        { 
            player.TriggerEnter(hitType, collision);
        }
    }
}
