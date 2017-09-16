using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class StartPanelCtrlScript : MonoBehaviour {
	
	void Start () {
		connectSocket ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void connectSocket(){
		//TestSocket s = new TestSocket ();
		CustomSocket.getInstance();
	}

	public void onClick(){
	}


}

