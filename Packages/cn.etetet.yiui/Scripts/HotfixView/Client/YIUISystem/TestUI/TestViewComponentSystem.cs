using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.12.1
    /// Desc
    /// </summary>
    [FriendOf(typeof(TestViewComponent))]
    public static partial class TestViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this TestViewComponent self)
        {
            //Debug.Log("YIUIInitialize TestViewComponentSystem");
        }

        [EntitySystem]
        private static void Destroy(this TestViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this TestViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
