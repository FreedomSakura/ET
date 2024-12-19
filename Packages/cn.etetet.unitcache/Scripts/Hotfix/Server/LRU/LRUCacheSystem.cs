using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(ET.Server.LRUCache))]
    [FriendOfAttribute(typeof(ET.Server.LRUCache))]
    [FriendOfAttribute(typeof(ET.Server.LRUNode))]
    public static partial class LRUCacheSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.LRUCache self)
        {
            self.MinFrequency = 0;
            self.FrequencyDict.Add(0, new LinkedList<EntityRef<LRUNode>>());
        }
        [EntitySystem]
        private static void Destroy(this ET.Server.LRUCache self)
        {
            self.LRUNodeDict.Clear();
            self.FrequencyDict.Clear();
            self.MinFrequency = 0;
        }

        public static void Call(this LRUCache self, long key)
        {
            EntityRef<LRUNode> nodeRef;
            LRUNode node;
            if (self.LRUNodeDict.TryGetValue(key, out nodeRef))
            {
                node = nodeRef;
                self.FrequencyDict[node.Frequency].Remove(node);
                node.Frequency++;
                if (!self.FrequencyDict.ContainsKey(node.Frequency))
                {
                    self.FrequencyDict.Add(node.Frequency, new LinkedList<EntityRef<LRUNode>>());
                }

                self.FrequencyDict[node.Frequency].AddLast(node);

                if (self.FrequencyDict[self.MinFrequency].Count == 0)
                {
                    self.MinFrequency = node.Frequency;
                }

                return;
            }

            node = self.AddChild<LRUNode, long>(key);
            node.Frequency = 0;

            self.FrequencyDict[0].AddLast(node);
            self.MinFrequency = 0;
            self.LRUNodeDict[key] = node;

            if (self.LRUNodeDict.Count >= 3000)
            {
                LRUNode fn = self.FrequencyDict[0].First.Value;
                long unitId = fn.Key;
                self.FrequencyDict[self.MinFrequency].RemoveFirst();
                self.LRUNodeDict.Remove(unitId);
                fn.Dispose();

                EventSystem.Instance.Invoke(SceneType.UnitCache, new LRUUnitCacheDelete() { LruCache = self, Key = unitId });
            }

        }
    }
}

