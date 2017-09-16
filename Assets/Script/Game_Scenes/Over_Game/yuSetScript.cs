using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class yuSetScript : MonoBehaviour {

	public Text numberText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setCount(int count ){
		numberText.text = "X" + count;
	}
}
