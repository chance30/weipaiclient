using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanelScript : MonoBehaviour {

	public Text label;

	// Use this for initialization
	void Start () {
	
	}

	public void setText(string str){
		label.text = str;
	}

	public void startAction(){
		Invoke ("TipsMoveCompleted",4f);
	}

	private void move(){
		gameObject.transform.DOLocalMove (new Vector3(0,-100),0.7f).OnComplete(TipsMoveCompleted);
	}

	public void TipsMoveCompleted(){
		Destroy (gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
