using System;
using System.Collections.Generic;
using UnityEngine;

	public class GameToolScript
	{
		public GameToolScript ()
		{
		}
		/// <summary>
		/// Sets the other card object position.
		/// </summary>
		/// <param name="tempList">Temp list.</param>
		/// <param name="initDiretion">Init diretion.</param>
		/// <param name="Type">Type.</param> 1- 碰，2-杠
		public void setOtherCardObjPosition(List<GameObject> tempList,String initDiretion,int type)
	    {
			if (type == 1) {
				switch (initDiretion) {
					case DirectionEnum.Top: //上
						tempList [0].transform.localPosition = new Vector3 (-273f,0f); //位置                      
						break;
					case DirectionEnum.Left: //左
						tempList [0].transform.localPosition = new Vector3 (0, -173); //位置              
						break;
					case DirectionEnum.Right: //右
						tempList [0].transform.localPosition = new Vector3 (0, 180f); //位置                  
						break;
				}
				

				for (int i = 1; i < tempList.Count; i++) {

					switch (initDiretion) {
					case DirectionEnum.Top: //上
						tempList [i].transform.localPosition = new Vector3 (-204f + 37 * (i - 1), 0); //位置                      
						break;
					case DirectionEnum.Left: //左
						tempList [i].transform.localPosition = new Vector3 (0, -105 + (i - 1) * 30); //位置              
						break;
					case DirectionEnum.Right: //右
						tempList [i].transform.localPosition = new Vector3 (0, 119 - (i - 1) * 30); //位置                  
						break;
					}
				}
			} else {
				
				for (int i = 0; i < tempList.Count; i++) {

					switch (initDiretion) {
					case DirectionEnum.Top: //上
						tempList [i].transform.localPosition = new Vector3 (-204 + 37 * i, 0); //位置                      
						break;
					case DirectionEnum.Left: //左
						tempList [i].transform.localPosition = new Vector3 (0, -105 + i * 30); //位置              
						break;
					case DirectionEnum.Right: //右
						tempList [i].transform.localPosition = new Vector3 (0, 119 - i * 30); //位置                  
						break;
					}
				}
			}
		}
	}

	public class DirectionEnum{
		public const string Bottom = "B";
		public const string Right = "R";
		public const string Top = "T";
		public const string Left = "L";
	}
