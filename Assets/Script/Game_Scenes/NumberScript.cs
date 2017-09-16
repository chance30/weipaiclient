using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumberScript : MonoBehaviour {
    public double Lasttime;
    private float timer=0;
    private int Direction;
    // Use this for initialization
    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        Lasttime = 10 - timer;
        Lasttime = Math.Floor(Lasttime);

        this.GetComponent<Text>().text = Lasttime.ToString();
    }

    public void number()
    {
        
    }
}
