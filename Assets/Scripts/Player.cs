using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector3 moveDir;
    [SerializeField] float moveSpeed = 5f;

    BoxCollider2D boxCollider2D;
    private bool isGround = false;
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moving();
        checkGround();
    }

    /// <summary>
    /// 캐릭터의 움직임
    /// </summary>
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    /// <summary>
    /// 땅에 닿아있는지 체크
    /// </summary>
    private void checkGround()
    {

    }
}
