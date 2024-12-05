using UnityEngine;

namespace ET
{
    [EntitySystemOf(typeof(GlobalComponent))]
    public static partial class GlobalComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GlobalComponent self)
        {
            self.Global = GameObject.Find("/Global").transform;
            self.Unit = GameObject.Find("/Global/Unit").transform;
            self.UI = GameObject.Find("/Global/UI").transform;
            
            self.InputController = GameObject.Find("/Global/InputController").transform;
            
            self.GlobalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
        }
    }
}