using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogPanelScript : MonoBehaviour {
	public  delegate void ButtonOnClick ();
	public ButtonOnClick onOkClickListener;//确认键监听
	public ButtonOnClick onCancleClickListener;//取消键监听

	public Text title;
	public Text msg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void setTitle(string titleStr){
		title.text = titleStr;

	}

	private void setMsg(string msgStr){
		msg.text = msgStr;
	}


	public void clickOk(){
		onOkClickListener ();
		DialogPanelScript self = GetComponent<DialogPanelScript> ();
		Destroy (self.title);
		Destroy (self.msg);
		Destroy (this);
		Destroy (gameObject);
	}

	public void clickCancle(){
		onCancleClickListener ();
		DialogPanelScript self = GetComponent<DialogPanelScript> ();
		Destroy (self.title);
		Destroy (self.msg);
		Destroy (this);
		Destroy (gameObject);
	}

	public void setContent(string titlestr,string msgstr, bool flag,ButtonOnClick yesCallBack,ButtonOnClick noCallBack){
		setTitle(titlestr);
		setMsg (msgstr);
		if (yesCallBack != null) {
			onOkClickListener += yesCallBack;
		}
		if (noCallBack != null) {
			onCancleClickListener += noCallBack;
		}
	}

}
