using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    [SerializeField] Canvas canvas;//�ڽ� ��ġ�� �˰������� Awake�� ���� �ʰ� �ø���������ʵ�� �־���

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GameTag.Player.ToString())//�����°� �÷��̾���
        {
            canvas.gameObject.SetActive(true);//Canvas�� Ȱ��ȭ
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())//�÷��̾ ���������ٸ�
        {
            canvas.gameObject.SetActive(false);//Canvas�� ��Ȱ��ȭ
        }
    }
    private void Awake()
    {
        //canvas = GetComponent<Canvas>(); ��������Ʈ�� �����ִ� ������Ʈ�� ã�� �� ��
        //canvas = transform.Find("Canvas").GetComponentInChildren<Canvas>(); Ȱ��ȭ ������ ���� ������Ʈ�� ã�� ���
    }
}
