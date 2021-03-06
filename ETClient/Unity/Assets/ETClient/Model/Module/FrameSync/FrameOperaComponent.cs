﻿using System;
using ETModel;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class FrameOperaComponentAwakeSystem : AwakeSystem<FrameOperaComponent>
    {
	    public override void Awake(FrameOperaComponent self)
	    {
		    self.Awake();
	    }
    }

	[ObjectSystem]
	public class FrameOperaComponentUpdateSystem : UpdateSystem<FrameOperaComponent>
	{
		public override void Update(FrameOperaComponent self)
		{
			self.Update();
		}
	}

	public class FrameOperaComponent: Component
    {
        public Vector3 ClickPoint;

	    public int mapMask;

	    public void Awake()
	    {
		    this.mapMask = LayerMask.GetMask("Map");
	    }

        public void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
	            if (Physics.Raycast(ray, out hit, 1000, this.mapMask))
	            {
					this.ClickPoint = hit.point;
		            SessionComponent.Instance.Session.Send(new Frame_ClickMap() { X = (int)(this.ClickPoint.x * 1000), Z = (int)(this.ClickPoint.z * 1000) });

					// 测试actor rpc消息
					//this.TestActor();
				}
            }
        }

	    public async void TestActor()
	    {
		    try
		    {
			    M2C_TestActorResponse response = (M2C_TestActorResponse)await SessionComponent.Instance.Session.Call(
						new C2M_TestActorRequest() { Info = "actor rpc request" });
			    Log.Info(response.Info);
			}
		    catch (Exception e)
		    {
				Log.Error(e);
		    }
		}
    }
}
