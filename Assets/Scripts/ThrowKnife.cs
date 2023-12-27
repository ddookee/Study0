using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float timeDestroy = 0.85f;
    [SerializeField] private float damage = 1f;


    Rigidbody2D rigid;
    Vector2 force;
    bool isRight;
    Transform trsKnife;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Object.ToString())
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = collision.GetComponent<Enemy>();
            enemySc.Hit(damage);
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid.AddForce(force, ForceMode2D.Force);
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
        //if(isRight == false)
        //{
        //    transform.position -= transform.up * Time.deltaTime //* speed;
        //}
    }

    public void SetForce(Vector2 _force, bool _isRight)
    {
        force = _force;
        isRight = _isRight;
    }
}
