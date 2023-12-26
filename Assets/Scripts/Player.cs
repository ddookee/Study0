using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;

    Rigidbody2D rigid;
    Vector3 moveDir;
    [SerializeField] float moveSpeed = 5f;

    BoxCollider2D boxCollider2D;
    [SerializeField] bool isGround = false;

    private float verticalVelocity;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5;
    private bool isJump = false;

    private Camera mainCam;

    //���� ȸ���Ҷ� ������ �ִ��� Ȯ���ϱ� ����
    private bool isPlayerLookAtRightDirection;
    Transform trsHand;
    [Header("��ô����")]
    [SerializeField] GameObject objThrowKnife;
    Transform trsThrowKnife;
    [SerializeField] Transform trsObjDynamic;



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
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        trsHand = transform.GetChild(0);
        anim = GetComponent<Animator>();
        trsThrowKnife = transform.Find("ThrowPos");
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        doAnimation();

        moving();
        checkGround();

        checkJumping();

        checkGravity();

        checkMouse();

    }

    private void doAnimation()
    {
        anim.SetBool("IsGround", isGround);
        anim.SetInteger("Horizontal", (int)moveDir.x);

        int curHorizontal = anim.GetInteger("Horizontal");
    }

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
        if(verticalVelocity > 0)
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
        if(Input.GetKeyDown(KeyCode.Space) && isGround == true)
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
            if(verticalVelocity < -10.0f)
            {
                verticalVelocity = -10;
            }
        }

        else if (isGround == true)
        {
            verticalVelocity = 0;
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
        if(distanceMouseToPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1,1,1);
            isPlayerLookAtRightDirection = true;
        }

        //������ �ٶ󺸴�
        else if(distanceMouseToPlayer.x < 0)
        {
            transform.localScale = new Vector3(1,1,1);
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
        //������ ����
        if (/*GamePause == false &&*/ Input.GetKeyDown(KeyCode.Q))
        {
            throwKnife();
            //throwTimer = throwlimit;
        }
    }

    private void throwKnife()
    {
        Vector3 rot = trsThrowKnife.localRotation.eulerAngles;
        rot.z += -90;
        GameObject obj = Instantiate(objThrowKnife, trsThrowKnife.position, Quaternion.Euler(rot), trsObjDynamic);
        ThrowKnife sc = obj.GetComponent<ThrowKnife>();
        Vector2 throwForce = new Vector2(10.0f, 0f);
        if (isPlayerLookAtRightDirection == false)
        {
            //throwForce = new Vector2(-10.0f, 0f);
            throwForce.x = -10.0f;
            
        }
        //sc.SetForce(trsThrowKnife.rotation * throwForce, isPlayerLookAtRightDirection);
        sc.SetForce(trsThrowKnife.rotation * throwForce, isPlayerLookAtRightDirection);
    }
}
