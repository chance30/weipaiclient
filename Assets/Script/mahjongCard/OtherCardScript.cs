using UnityEngine;
using System.Collections;

public class OtherCardScript : MonoBehaviour {
    private int cardPoint;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setPoint(int _cardPoint)
    {
        cardPoint = _cardPoint;//设置所有牌指针
    }

    public int getPoint()
    {
        return cardPoint;
    }
}
