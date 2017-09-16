using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Timers;

public class timer : MonoBehaviour {
    private string time;
	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
        time = System.DateTime.Now.ToString("HH : mm");
        GetComponent<Text>().text = time;
	}
}
