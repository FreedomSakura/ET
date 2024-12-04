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
    [FriendOf(typeof(TestPopViewComponent))]
    public static partial class TestPopViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this TestPopViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TestPopViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this TestPopViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
