using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class bottomScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Rigidbody2D pai;
   
   // public GameObject Bigmajiang;
   // public GameObject Image;
    private float timer = 0;
    private int cardPoint;
    private Vector3 RawPosition;
    private Vector3 oldPosition;
	private bool dragFlag = false;
    //==================================================
    public Image image;
    public Text showLabel;
    public float speed = 1.0f;
    public float ShowTime = 1.5f;
    //
    public delegate void EventHandler(GameObject obj);
    public event EventHandler onSendMessage;
	public event EventHandler reSetPoisiton;
	public bool selected = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (GlobalDataScript.isDrag)
        {
			dragFlag = true;
            GetComponent<RectTransform>().pivot.Set(0, 0);
            transform.position = Input.mousePosition;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
		if (GlobalDataScript.isDrag) {
			if (selected == false) {
				selected = true;
				oldPosition = transform.localPosition;
			} else {
				sendObjectToCallBack ();
			}
		}

    }

    public void OnPointerUp(PointerEventData eventData)
    {
		if (GlobalDataScript.isDrag) {
			if (transform.localPosition.y > -122f) {
				sendObjectToCallBack ();
			} else {
				if (dragFlag) {
					transform.localPosition = oldPosition;
				} else {
					reSetPoisitonCallBack ();
				}
			}
			dragFlag = false;
		}
    }

	private void sendObjectToCallBack(){
		if (onSendMessage != null)     //发送消息
		{
			onSendMessage(gameObject);//发送当前游戏物体消息
		}
	}

	private void reSetPoisitonCallBack(){
		if (reSetPoisiton != null) {
			reSetPoisiton (gameObject);
		}
	}

    public void setPoint(int _cardPoint)
    {
        cardPoint = _cardPoint;//设置所有牌指针
		image.sprite = Resources.Load("Cards/Big/b"+cardPoint,typeof(Sprite)) as Sprite;

    }

    public int getPoint()
    {
        return cardPoint;
    }

    private void destroy()
    {
       // Destroy(this.gameObject);
    }

}
