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
    [FriendOf(typeof(AngelViewComponent))]
    public static partial class AngelViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this AngelViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this AngelViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this AngelViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
