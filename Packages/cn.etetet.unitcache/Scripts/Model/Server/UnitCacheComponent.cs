using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class UnitCacheComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, EntityRef<UnitCache>> UnitCaches = new Dictionary<string, EntityRef<UnitCache>>(); //k: TypeName, v: unitCache
        public List<string> UnitCacheKeyList = new List<string>();  // 工程中所有可以被缓存的类型的全名
    }
}