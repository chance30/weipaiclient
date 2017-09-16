using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
/**
 * 单人投票结果
*/
namespace AssemblyCSharp
{
	public class PlayerResult :MonoBehaviour
	{
		public Text name;
		public Text result;
		public PlayerResult ()
		{
		}

		public void  setInitVal(string namestr,string resultstr){
			name.text = namestr;
			result.text = resultstr;
		}

		public void changeResult(string resultstr){
			result.text = resultstr;
		}
			
	}
}

