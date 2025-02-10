using System.Collections.Generic;


namespace ET.Server
{
	public static partial class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone, string account)
		{
			ulong hash = (ulong)account.GetLongHashCode();

			// var cfg = StartSceneConfigCategory.Instance.Get(zone);
			// if (cfg == null)
			// {
			// 	return null;
			// }
			//
			// zone = cfg.Zone;
			
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.GetBySceneType(zone, SceneType.Gate);
			
			return zoneGates[(int)(hash % (ulong)zoneGates.Count)];
		}
	}
}
