
using UnityEngine;
using System.Collections;
using UnityEditor;
 
public class NewBehaviourScript : Editor {
 
	[MenuItem("Tools/Build Google Project")]
	static public void BuildAssetBundles(){
		BuildPipeline.BuildPlayer(new string[]{ "Assets/Untitled.unity"} , Application.dataPath + "/../", BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
	}
}