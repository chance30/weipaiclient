using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 这是声音控制面板，控制声音的大小，如果要显示或者隐藏该面板，请调用 showOrHidePanel（）方法
/// </summary>
public class AudioCtrlPanelScript : MonoBehaviour {
	AudioSource myAudioClip;
	public Text volumeText;
	public Slider audioSlider;
	// Use this for initialization
	void Start () {
		myAudioClip = GameObject.Find ("MyAudio").GetComponent<AudioSource> ();
		audioSlider.value = myAudioClip.volume;
		setText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showOrHidePanel(){
		gameObject.SetActive (!gameObject.activeSelf);
	}

	public void silderChange(){
		myAudioClip.volume = audioSlider.value;
		setText ();
	}


	void setText(){
		volumeText.text = (int)(100 * audioSlider.value) +"";
	}
}
