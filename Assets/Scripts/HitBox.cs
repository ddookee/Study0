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
        string tag = transform.root.gameObject.tag;//�ڽ��� ���� ��Ʈ�� �±�

        if (tag == "Enemy")//�±װ� �ֳʹ̶�� Enemy��ũ��Ʈ���� ��������
        {
            isEnemy = true;
            enemy = GetComponentInParent<Enemy>();
        }
        else//�±װ� �÷��̾��� Player��ũ��Ʈ���� ������
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
