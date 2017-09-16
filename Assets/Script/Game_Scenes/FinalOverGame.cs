using System;
using UnityEngine;
using LitJson;
using AssemblyCSharp;


public class FinalOverGame: MonoBehaviour
{



	// Use this for initialization
	void Start () {
		addListener ();
	}

	// Update is called once per frame
	void Update () {

	}
	public FinalOverGame ()
	{
		
	}

	public void addListener(){
		SocketEventHandle.getInstance ().FinalGameOverCallBack += finalGameOverCallBack;

	} 
	public void removeListener(){
		SocketEventHandle.getInstance ().FinalGameOverCallBack -= finalGameOverCallBack;

	}
		

	private void finalGameOverCallBack(ClientResponse response){
		GlobalDataScript.finalGameEndVo = JsonMapper.ToObject<FinalGameEndVo> (response.message);
	

	/*
		if (GlobalDataScript.surplusTimes > 1) {
			Invoke ("finalGameOver", 10);
		} else {
			Invoke ("finalGameOver",10);
		}
*/
	}

	private void finalGameOver(){

		loadPerfab ("prefab/Panel_Game_Over",1);


	//	GlobalDataScript.singalGameOver.GetComponent<GameOverScript> ().closeDialog ();

 		if (GlobalDataScript.singalGameOverList.Count > 0) {
            for (int i = 0; i < GlobalDataScript.singalGameOverList.Count; i++)
            {
                //GlobalDataScript.singalGameOverList [i].GetComponent<GameOverScript> ().closeDialog ();
                Destroy(GlobalDataScript.singalGameOverList[i].GetComponent<GameOverScript>());
                Destroy(GlobalDataScript.singalGameOverList[i]);
            }
            //int count = GlobalDataScript.singalGameOverList.Count;
            //for (int i = 0; i < count; i++) {
            //	GlobalDataScript.singalGameOverList.RemoveAt (0);
            //}
            GlobalDataScript.singalGameOverList.Clear();
        }
			
		CommonEvent.getInstance ().closeGamePanel ();
	}

	private void  loadPerfab(string perfabName ,int openFlag){
		GameObject obj= PrefabManage.loadPerfab (perfabName);
		obj.GetComponent<GameOverScript> ().setDisplaContent (openFlag,GlobalDataScript.roomAvatarVoList,null,GlobalDataScript.hupaiResponseVo.validMas);
		obj.transform.SetSiblingIndex (2);
	}
}


