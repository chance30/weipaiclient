using System;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;


public class ZhanjiRoomItemScript :MonoBehaviour
{
	
	private ZhanjiRoomDataItem mItemData;
	public Text indexText;
	public Text roomIdText;
	public Text timeText;
	public List<Text> names;
	public List<Text> scores;
	void Start(){

	}
	void Upate(){

	}
	public ZhanjiRoomItemScript ()
	{
	}

	public void setUI(ZhanjiRoomDataItem  itemdata ,int index){
		mItemData = itemdata;
		indexText.text = index + "";
		roomIdText.text = mItemData.roomId + "";
		timeText.text = parseDateSpan (mItemData.data.createtime);
		pareseContent (mItemData.data.content);

	}

	private string parseDateSpan(long timenumber){
		
		DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		dateTimeStart=dateTimeStart.AddMilliseconds (timenumber);
		string month =(dateTimeStart.Month>9)? dateTimeStart.Month.ToString ():"0"+dateTimeStart.Month.ToString ();
		string day =(dateTimeStart.Day>9)? dateTimeStart.Day.ToString ():"0"+dateTimeStart.Day.ToString ();
		string hour = dateTimeStart.Hour.ToString ();
		string minute = dateTimeStart.Minute.ToString ();
		return month + "-" + day + "  " + hour + ":" + minute;
	
	}
	private void pareseContent(string content){
		if (content != null && content != "") {
			string[] infoList = content.Split (new char[1]{','});
			for (int i = 0; i < infoList.Length-1; i++) {
				string name = infoList [i].Split (new char[1]{':'})[0];
				string score = infoList [i].Split (new char[1]{':'})[1];
				names [i].text = name;
				scores [i].text = score;
			}
		}
	}

	public void clickItem(){
		CustomSocket.getInstance ().sendMsg (new ZhanjiRequest (mItemData.data.id + ""));
	}

}


