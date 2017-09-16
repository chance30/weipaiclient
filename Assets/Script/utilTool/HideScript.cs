using UnityEngine;
using System.Collections;

public class HideScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hide(){
		gameObject.SetActive (false);
	}
}
