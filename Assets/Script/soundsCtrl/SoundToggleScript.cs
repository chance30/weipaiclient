using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoundToggleScript : MonoBehaviour {
	public GameObject openBtn;
	public GameObject closeBtn;
	public GameObject showFramebtn;
	private bool isShowFrame = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openClick(){
		openBtn.SetActive(false);
		closeBtn.SetActive(true);
		GlobalDataScript.soundToggle = true;
	}

	public void closeClick(){
		openBtn.SetActive(true);
		closeBtn.SetActive(false);
		GlobalDataScript.soundToggle = false;

	}

	public void clicksettingbtn(){
		if (!isShowFrame) {
			showSettingframe ();
			isShowFrame = true;
			//showFramebtn.transform.Rotate( new Vector3 (0, 0, -90));
		} else {
			hideSettingFrame ();
			isShowFrame = false;
			//showFramebtn.transform.Rotate ( new Vector3 (0, 0, 90));
		}

	}

	private void showSettingframe(){
		gameObject.transform.DOLocalMove (new Vector3(65,-5), 0.4f);

	}

	private void hideSettingFrame(){
		gameObject.transform.DOLocalMove (new Vector3(65,94), 0.4f);
	}
}
