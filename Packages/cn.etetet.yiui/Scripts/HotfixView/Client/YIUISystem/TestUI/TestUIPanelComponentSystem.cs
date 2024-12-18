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
    [FriendOf(typeof(TestUIPanelComponent))]
    public static partial class TestUIPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this TestUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TestUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this TestUIPanelComponent self)
        {
            Debug.Log("Yzxcv");
            
            //var firstView = YIUIMgrComponent.Inst.GetPanelView<TestUIPanelComponent, TestViewComponent>();
            //await firstView.UIView.Open();
            
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        
        [YIUIInvoke(TestUIPanelComponent.OnEventCloseInvoke)]
        private static void OnEventCloseInvoke(this TestUIPanelComponent self)
        {
            
            YIUIMgrComponent.Inst.ClosePanel<TestUIPanelComponent>();
        }
        #endregion YIUIEvent结束
    }
}
