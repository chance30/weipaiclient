using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginTest : MonoBehaviour {
    private Dropdown dp;
    private string username;
    private string password;
	// Use this for initialization
	void Start () {
		//dp=GameObject.Find("Dropdown").GetComponent<Dropdown>();
	}
    /// <summary>
    /// 选择账号
    /// </summary>
    public void SelectCount() { 
    //当选择的时候调用数据库相应的账号密码
        if(dp.captionText.text=="1"){
            username = "123";
            password = "123";
        }
        else if(dp.captionText.text=="2"){
            username = "1234";
            password = "1234";
            Debug.Log("你选择了2。。。。。。。。。");
        }
        else if(dp.captionText.text=="3"){
            username = "12345";
            password = "12345";
        
        }
        else if (dp.captionText.text == "4")
        {
            username = "123456";
            password = "123456";

        }
        else if (dp.captionText.text == "5")
        {
            username = "1234567";
            password = "1234567";

        }
        
    
    }
    /// <summary>
    /// 登录
    /// </summary>
    public void Login() {

        CustomSocket.getInstance().sendMsg(new LoginRequest(null));

        //if(username=="123"&&password=="123"){
        // //执行登录操作
        //    Debug.Log("登录中...............");
        //    CustomSocket.getInstance().sendMsg(new LoginRequest(null));
        //}
        //else if(username=="1234"&&password=="1234"){
        //    Debug.Log("2........登录中...............");
        //    CustomSocket.getInstance().sendMsg(new LoginRequest(null));
        //}
        //else if (username == "12345" && password == "12345")
        //{
        //    Debug.Log("2........登录中...............");
        //    CustomSocket.getInstance().sendMsg(new LoginRequest(username));
        //}
        //else if (username == "123456" && password == "123456")
        //{
        //    Debug.Log("2........登录中...............");
        //    CustomSocket.getInstance().sendMsg(new LoginRequest(username));
        //}
        //else if (username == "1234567" && password == "1234567")
        //{
        //    Debug.Log("2........登录中...............");
        //    CustomSocket.getInstance().sendMsg(new LoginRequest(username));
        //}
    }
	// Update is called once per frame
	void Update () {
		
	}
}
