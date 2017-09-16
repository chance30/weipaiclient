using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopAndBottomCardScript : MonoBehaviour {
    private int cardPoint;

    //=========================================
	public Image cardImg;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update () {
	
	}

    public void setPoint(int _cardPoint)
    {
        cardPoint = _cardPoint;//设置所有牌指针
		cardImg.sprite = Resources.Load("Cards/Small/s"+cardPoint,typeof(Sprite)) as Sprite;

    }

	public void setLefAndRightPoint(int _cardPoint){
		cardPoint = _cardPoint;//设置所有牌指针
		cardImg.sprite = Resources.Load("Cards/Left&Right/lr"+cardPoint,typeof(Sprite)) as Sprite;

	}

    public int getPoint()
    {
        return cardPoint;
    }
}
