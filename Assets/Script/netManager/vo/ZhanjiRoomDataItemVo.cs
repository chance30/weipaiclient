using System;
/**
 *战绩每项内容 
 */
namespace AssemblyCSharp
{
	[Serializable]
	public class ZhanjiRoomDataItemVo
	{
		public int id;
		public int roomid;
		public string  content;
		public DateVo createtime;
		public ZhanjiRoomDataItemVo ()
		{
			
		}
	}
}

