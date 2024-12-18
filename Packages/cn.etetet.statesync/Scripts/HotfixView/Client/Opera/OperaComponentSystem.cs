using System;
using ET.Server;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [EntitySystemOf(typeof(OperaComponent))]
    public static partial class OperaComponentSystem
    {
        
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
            self.mapMask = LayerMask.GetMask("Map");

            var global = self.Root().GetComponent<GlobalComponent>();
            var playerInput = global.InputController.GetComponent<PlayerInput>();
            self.operaActionMap = playerInput.currentActionMap;

            self.operaActionMap.Enable();
        }

        [EntitySystem]
        private static void Update(this OperaComponent self)
        {
            self.OnClick();
            // if (Input.GetMouseButtonDown(1))
            // {
            //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //     RaycastHit hit;
            //     if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
            //     {
            //         C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
            //         c2MPathfindingResult.Position = hit.point;
            //         self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
            //     }
            // }
            //
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //     CodeLoader.Instance.Reload();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     self.Test1().NoContext();
            // }
            //     
            // if (Input.GetKeyDown(KeyCode.W))
            // {
            //     self.Test2().NoContext();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     self.TestCancelAfter().WithContext(new ETCancellationToken());
            // }
            //
            // if (Input.GetKeyDown(KeyCode.T))
            // {
            //     C2M_TransferMap c2MTransferMap = C2M_TransferMap.Create();
            //     self.Root().GetComponent<ClientSenderComponent>().Call(c2MTransferMap).NoContext();
            // }
        }
        
        #region Origin
        private static async ETTask Test1(this OperaComponent self)
        {
            Log.Debug($"Croutine 1 start1 ");
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(1, 20000, 3000))
            {
                await self.Root().GetComponent<TimerComponent>().WaitAsync(6000);
            }

            Log.Debug($"Croutine 1 end1");
        }
            
        private static async ETTask Test2(this OperaComponent self)
        {
            ETCancellationToken oldCancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
            Log.Debug($"Croutine 2 start2");
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(1, 20000, 3000))
            {
                await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            }
            Log.Debug($"Croutine 2 end2");
        }
        
        private static async ETTask TestCancelAfter(this OperaComponent self)
        {
            ETCancellationToken oldCancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
            
            Log.Debug($"TestCancelAfter start");
            ETCancellationToken newCancellationToken = new();
            await self.Root().GetComponent<TimerComponent>().WaitAsync(3000).TimeoutAsync(newCancellationToken, 1000);
            if (newCancellationToken.IsCancel())
            {
                Log.Debug($"TestCancelAfter newCancellationToken is cancel!");
            }
            
            if (oldCancellationToken != null && !oldCancellationToken.IsCancel())
            {
                Log.Debug($"TestCancelAfter oldCancellationToken is not cancel!");
            }
            Log.Debug($"TestCancelAfter end");
        }
        #endregion

        #region new

        private static void OnClick(this OperaComponent self)
        {
            var mouse = Mouse.current;
            var mousePos = mouse.position.ReadValue();
            //var mousePos = self.operaActionMap.FindAction("Click").ReadValue<Vector2>();
            var isClick = self.operaActionMap.FindAction("Click").ReadValue<float>();
            if (isClick <= 0)
            {
                return;
            }
            Log.Console(string.Format("mousePos: {0} / isClick: {1}", mousePos, isClick));
            
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
            {
                C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
                c2MPathfindingResult.Position = hit.point;
                self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
            }
        }
        

        #endregion
        
        [EntitySystem]
        private static void Destroy(this OperaComponent self)
        {
            if (self.operaActionMap != null)
            {
                self.operaActionMap.Disable();
            }
        }
        
        // private static void OnClick(InputAction.CallbackContext context)
        // {
        //     
        //     var mouse = Mouse.current;
        //     var mousePos = mouse.position.ReadValue();
        //     Log.Console(string.Format("mousePos: {0}", mousePos));
        //     Ray ray = Camera.main.ScreenPointToRay(mousePos);
        //     RaycastHit hit;
        //     // if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
        //     // {
        //     //     C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
        //     //     c2MPathfindingResult.Position = hit.point;
        //     //     self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
        //     // }
        //     
        //     // if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
        //     // {
        //     //     C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
        //     //     c2MPathfindingResult.Position = hit.point;
        //     //     self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
        //     // }
        // }
    }
}