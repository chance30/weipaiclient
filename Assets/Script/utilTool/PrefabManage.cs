using System;
using UnityEngine;

/**
 * prefab 管理器
 */ 
public class PrefabManage : MonoBehaviour
{
	public PrefabManage ()
	{
	}
	public static GameObject loadPerfab(string perfabName){

		GameObject panelCreateDialog = Instantiate (Resources.Load(perfabName)) as GameObject;
		panelCreateDialog.transform.parent = GlobalDataScript.getInstance ().canvsTransfrom;;
		panelCreateDialog.transform.localScale = Vector3.one;
		panelCreateDialog.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		panelCreateDialog.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		return panelCreateDialog;
	}
}
