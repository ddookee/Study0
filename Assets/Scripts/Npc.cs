using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    [SerializeField] Canvas canvas;//자식 위치를 알고있으니 Awake에 넣지 않고 시리얼라이즈필드로 넣어줌

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GameTag.Player.ToString())//들어오는게 플레이어라면
        {
            canvas.gameObject.SetActive(true);//Canvas를 활성화
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameTag.Player.ToString())//플레이어가 빠져나간다면
        {
            canvas.gameObject.SetActive(false);//Canvas를 비활성화
        }
    }
    private void Awake()
    {
        //canvas = GetComponent<Canvas>(); 겟컴포넌트는 꺼져있는 오브젝트를 찾지 못 함
        //canvas = transform.Find("Canvas").GetComponentInChildren<Canvas>(); 활성화 돼있지 않은 오브젝트를 찾을 경우
    }
}
