using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using DG.Tweening;
using UnityEngine.UI;
using System;

using LitJson;

using cn.sharesdk.unity3d;


public class LotteryPanelScript : MonoBehaviour
{
    public  Text curTime;
	// Use this for initialization
	bool action = false;
	bool callBack = false;
	private int x = 0;
    public Text choujiangNum;
    private int prizecount;
    float end = 270;
	public Image turnImage;
    public float smoothing;
   // public float endFlag=20;
	public List<LotteryItemScript> lotteryItems;
    public Text Congratulations;
	public int  StopIndex = 1;


	public GameObject rulePanel;//活动说明对话框
	public Text ruleText;//活动说明文字

	private GiftList giftDes;

	void Start ()
	{
		SocketEventHandle.getInstance ().giftResponse += giftResponse;
		CommonEvent.getInstance ().prizeCountChange += prizeCountChange;	
		CustomSocket.getInstance ().sendMsg (new GetGiftRequest ("0"));
	    choujiangNum.text = GlobalDataScript.loginResponseData.account.prizecount+"";

	}


	private void prizeCountChange(){
		TipsManagerScript.getInstance ().setTips ("您的抽奖次数已经更新");
		choujiangNum.text = GlobalDataScript.loginResponseData.account.prizecount+"";
	}

	public class Drawl{
		public  string type;
		public int data;
	}
	/**
    private void ItemSprite(ClientResponse response)
    {
        //itemList[0].GetComponentInChildren<Image>().sprite=response.
    }
*/

    // Update is called once per frame
    void Update ()
    {
        curTime.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");


    }
		
	void FixedUpdate()
	{
		if (action)
		{
			int b = 0;
			if(callBack){
				x -= 30;
				int a = x/-360;
				b = 22-a;
			}else{
				b = 22;
			}
			if (b > 2)
			{
				turnImage.transform.Rotate (new Vector3 (0, 0, -end), b);
			}
			else
			{
				float result = Math.Abs (turnImage.transform.localRotation.eulerAngles.z - end);
				if (result < 2) {
					action = false;
					callBack = false;
					end = 0;
				} else
				{
				    //float lerp=Mathf.Lerp(b, 0, Time.deltaTime*smoothing);
                    turnImage.transform.Rotate(new Vector3(0, 0, -end), 2f);
					Invoke ("turnOver", 3);
                    //turnOver();
				}
			}
		}
	}

    public void turnOver()
    {
        Congratulations.text = "恭喜你！你抽到了"+lotteryItems[StopIndex-1].nameTxt.text;
    }

	public void giftResponse(ClientResponse response){
		callBack = true;
		JsonData data = JsonMapper.ToObject<JsonData> (response.message);
		if (int.Parse(data ["type"].ToString())== 2) {
			TipsManagerScript.getInstance ().setTips ("抽奖活动暂时没有开放，3秒后将关闭对话框");
			Invoke ("closeDialog",3f);
		} else {
			try
			{ 
				giftDes = JsonMapper.ToObject<GiftList> (response.message);
				if (giftDes.type == "0") {//所有
					//ruleText.text = giftDes.data[0].notice;
					for(int i=0;i<giftDes.data.Count;i++){
						GiftItemVo itemData = giftDes.data [i];
						lotteryItems [i].nameTxt.text = itemData.prizeName;
						lotteryItems [i].setPic (itemData.imageUrl);
					}

				}
			}catch (Exception e){
				if (GlobalDataScript.loginResponseData.account.prizecount > 0) {
					GlobalDataScript.loginResponseData.account.prizecount--;
					choujiangNum.text = GlobalDataScript.loginResponseData.account.prizecount+"";
				}
				Drawl returndata = JsonMapper.ToObject<Drawl> (response.message);
				StopIndex = returndata.data;
				MyDebug.Log ("StopIndex" + StopIndex);
				if (action == false) {
					float a = UnityEngine.Random.Range(-2, 2f);
					end = Math.Abs(StopIndex * 36 - 34+a);
					MyDebug.Log ("end = "+end);
					x = 0;
					action = true;
					callBack = true;
				}
			}
		}



	

	}

	public void shareToWeChat(){
		GlobalDataScript.getInstance ().wechatOperate.shareAchievementToWeChat (PlatformType.WeChatMoments);
	}

	/***
	 *关闭对话框 
	 */
	public void closeDialog(){
		SocketEventHandle.getInstance ().giftResponse -= giftResponse;
		Destroy (this);
		Destroy (gameObject);
	}


	public void startTurn(){
		MyDebug.Log ("sssssssssssssssssss");
	    if (GlobalDataScript.loginResponseData.account.prizecount > 0)
	    {
		    CustomSocket.getInstance ().sendMsg (new GetGiftRequest ("1"));
        }
	    else
	    {
	        TipsManagerScript.getInstance().setTips("对不起，抽奖次数不足");
	    }
    }

	/*显示活动规则*/
	public void showRule(){
		rulePanel.SetActive (true);
	}

	/*关闭规则对话框*/
	public void closeRule(){
		rulePanel.SetActive (false);
	}
}
