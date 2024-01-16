using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum hitType
    {
        HitCheck,
        EnemyCheck,
        RotATKCheck,
    }

    //Animator anim;

    Rigidbody2D rigid;
    Vector3 moveDir;
    public Vector3 MoveDir
    {
        get => moveDir; //{return moveDir}
        set => moveDir = value; //{moveDir = value;}
    }
    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    [SerializeField] float moveSpeed = 5f;

    BoxCollider2D boxCollider2D;
    [Header("�÷��̾�")]
    [SerializeField] bool isGround = false;
    private float verticalVelocity;
    [SerializeField,Tooltip("���� ������ �� �����ϴ� ��")] float zeroVelocity = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5;
    private bool isJump = false;
    //�÷��̾��� ü��
    [SerializeField] private float MaxHp = 3;
    private float CurHp;

    private SpriteRenderer sr;

    private Camera mainCam;

    //���� ȸ���Ҷ� ������ �ִ��� Ȯ���ϱ� ����
    private bool isPlayerLookAtRightDirection;
    Transform trsHand;
    [Header("��ô����")]
    [SerializeField] GameObject objThrowKnife;
    Transform trsThrowKnife;
    [SerializeField] Transform trsObjDynamic;
    [SerializeField] float throwlimit = 0.3f;
    float throwTimer = 0.0f;

    [Header("�⺻��������")]
    [SerializeField] private float attackDamage;
    //Collider2D attackColl;
    //private bool isAttack = false;

    [Header("ȸ�� ����")]
    [SerializeField] private float RotATKDamage = 0;
    //Collider2D RotATKColl;
    //private bool isRotATK = false;
    int count = 0;

    PlayerAnim playerAnim;




    private void OnDrawGizmos()
    {
        if (boxCollider2D != null)
        {
            Gizmos.color = Color.red;
            Vector3 pos = boxCollider2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, boxCollider2D.bounds.size);
        }
    }

    private void Awake()
    {
        //�÷��̾��� ü��
        CurHp = MaxHp;

        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        trsHand = transform.GetChild(0);
        sr = GetComponent<SpriteRenderer>();
        playerAnim = GetComponentInChildren<PlayerAnim>();
        //anim = GetComponent<Animator>();

        //�÷��̾��� ����
        Transform childPlayerAnim = transform.Find("PlayerAnim");

        trsThrowKnife = childPlayerAnim.Find("ThrowPos");

        //Transform childAttack = childPlayerAnim.Find("AttackPos");
        //attackColl = childAttack.GetComponent<BoxCollider2D>();
        ////ȸ������
        //Transform childRotATK = childPlayerAnim.Find("RotateATKPos");
        //RotATKColl = childRotATK.GetComponent<BoxCollider2D>();

    }


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        //doAnimation();

        moving();
        checkGround();

        checkJumping();

        checkGravity();

        checkMouse();

    }


    public void TriggerEnter(hitType _type, Collider2D _collision)
    {
        //������ ������ ������� ����
        if (_type == hitType.EnemyCheck && _collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = _collision.gameObject.GetComponent<Enemy>();
            CurHp -= enemySc.damage;
            if (CurHp <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                playerAnim.Hit();//PlayerAnim���� ������� �޾����� ���� �ٲٶ� ���
            }
        }

        //������ ������������ ���� ������� ����
        else if (_type == hitType.HitCheck && _collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = _collision.GetComponent<Enemy>();
            enemySc.Hit(attackDamage);
        }

        //ȸ�� ������ ���� ������� ����
        else if (_type == hitType.RotATKCheck && _collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = _collision.GetComponent<Enemy>();
            enemySc.Hit(RotATKDamage);
        }
    }

    

    //private void doAnimation()
    //{
    //    anim.SetBool("IsGround", isGround);
    //    anim.SetInteger("Horizontal", (int)moveDir.x);

    //    int curHorizontal = anim.GetInteger("Horizontal");


    //    //�������� �ִϸ��̼�
    //    anim.SetBool("Attack", isAttack);

    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        isAttack = true;
    //    }



    //    //ȸ���ϸ� �����ϴ� �ִϸ��̼�
    //    anim.SetBool("RotateATK", isRotATK);
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        isRotATK = true;

    //    }
    //}

    /// <summary>
    /// ĳ������ ������
    /// </summary>
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    /// <summary>
    /// ���� ����ִ��� üũ
    /// </summary>
    private void checkGround()
    {
        isGround = false;
        if (verticalVelocity > 0)
        {
            return;
        }
        //�������� ���� ���� ����ִ��� üũ
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    private void checkJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            isJump = true;
        }

    }
    /// <summary>
    /// �߷�üũ
    /// </summary>
    private void checkGravity()
    {
        if (isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if (verticalVelocity < -10.0f)
            {
                verticalVelocity = -10;
            }
        }

        else if (isGround == true)
        {
            if (verticalVelocity < 0)//�޴� ���� �����϶� õõ�� 0���� �������
            { 
                verticalVelocity += Time.deltaTime * zeroVelocity;
            }
            else//������ �ƴ϶�� 0
            { 
                verticalVelocity = 0; 
            }
        }

        //����
        if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    /// <summary>
    /// ���콺��ġ�� ���� ĳ���� ������ȯ
    /// </summary>
    private void checkMouse()
    {
        //���콺�� ��ġ ��������
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);


        //���콺�� �¿� Ȯ��
        //������ �ٶ󺸴�
        Vector3 distanceMouseToPlayer = mouseWorldPos - transform.position;
        if (distanceMouseToPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isPlayerLookAtRightDirection = true;
        }

        //������ �ٶ󺸴�
        else if (distanceMouseToPlayer.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isPlayerLookAtRightDirection = false;
        }

        Vector3 direction = Vector3.right;
        if (isPlayerLookAtRightDirection == false)
        {
            direction = Vector3.left;
        }

        float angle = Quaternion.FromToRotation(direction, distanceMouseToPlayer).eulerAngles.z;
        if (isPlayerLookAtRightDirection == true)
        {
            angle = -angle;
        }
        trsHand.localEulerAngles = new Vector3(trsHand.localEulerAngles.x,
            trsHand.localEulerAngles.y, angle);


        if (throwTimer != 0.0f)
        {
            throwTimer -= Time.deltaTime;
            if (throwTimer < 0.0f)
            {
                throwTimer = 0.0f;
            }
        }

        //������ ����
        if (/*GamePause == false &&*/ Input.GetKeyDown(KeyCode.Q) && throwTimer == 0.0f)
        {
            throwKnife();
            throwTimer = throwlimit;
        }
    }

    /// <summary>
    /// Į�� ������ ��ų
    /// </summary>
    private void throwKnife()
    {
        Vector3 rot = trsThrowKnife.localRotation.eulerAngles;
        rot.z += -90;
        if (isPlayerLookAtRightDirection == false)
        {
            rot.z = +90;
        }
        GameObject obj = Instantiate(objThrowKnife, trsThrowKnife.position, Quaternion.Euler(rot), trsObjDynamic);
        ThrowKnife sc = obj.GetComponent<ThrowKnife>();
        Vector2 throwForce = new Vector2(10.0f, 0f);// * Time.deltaTime *speed;
        if (isPlayerLookAtRightDirection == false)
        {
            throwForce = new Vector2(-10.0f, 0f);// * Time.deltaTime * speed;
            //throwForce.x = -10.0f;

        }

        sc.SetForce(trsThrowKnife.rotation * throwForce, isPlayerLookAtRightDirection);
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == GameTag.Enemy.ToString())
    //    {
    //        Enemy enemySc = collision.GetComponent<Enemy>();
    //        enemySc.Hit(attackDamage);
    //    }
    //}

    /// <summary>
    /// �⺻���� �ݶ��̴� Ȱ��ȭ
    /// </summary>
    //private void onAttack()
    //{
    //    //�⺻����
    //    attackColl.enabled = true;
    //}
    /// <summary>
    /// ȸ������ �ݶ��̴� Ȱ��ȭ
    /// </summary>
    //private void onRotATK()
    //{
    //    //ȸ������
    //    RotATKColl.enabled = true;

    //}

    /// <summary>
    /// �⺻���� �ݶ��̴� ��Ȱ��ȭ
    /// </summary>
    //private void offAttack()
    //{
    //    //�⺻����
    //    attackColl.enabled = false;

    //    //Attack �ִϸ��̼� false�� �������
    //    isAttack = false;

    //}
    /// <summary>
    /// ȸ�� ���� �ݶ��̴� ��Ȱ��ȭ
    /// </summary>
    //private void offRotATK()
    //{
    //    //ȸ������
    //    RotATKColl.enabled = false;

    //    //ȸ������ �ִϸ��̼��� false��
    //    count++;
    //    rotCase();
    //}
    //private void rotCase()
    //{
    //    Debug.Log(count);
    //    switch (count)
    //    {
    //        case 1:
    //            {
    //                if (Input.GetKey(KeyCode.E))
    //                {
    //                    isRotATK = true;
    //                }
                    
    //            }
    //            break;
    //        case 2:
    //            {
    //                if (Input.GetKey(KeyCode.E))
    //                {
    //                    isRotATK = true;
    //                }
    //            }
    //            break;
    //        case 3:
    //            {
    //                //Input.GetKey(KeyCode.E);
    //                isRotATK = false;
    //                count = 0;
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
