/*
* UniOSC
* Copyright © 2014-2015 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using OSCsharp.Data;


namespace UniOSC{

	/// <summary>
	/// Moves a GameObject in normalized coordinates (ScreenToWorldPoint)
	/// </summary>
	[AddComponentMenu("UniOSC/MoveGameObject")]
	public class UniOSCMoveGameObject1 :  UniOSCEventTarget {

		[HideInInspector]
		public Transform transformToMove;
		public float nearClipPlaneOffset = 1;
		public enum Mode{Screen,Relative}
		public Mode movementMode;
		//movementModeProp = serializedObject.FindProperty ("movementMode");

		private Vector3 pos;

		void Awake(){

		}


		public override void OnEnable()
		{
			base.OnEnable();

			if(transformToMove == null){
				Transform hostTransform = GetComponent<Transform>();
				if(hostTransform != null) transformToMove = hostTransform;
			}
		}



		public override void OnOSCMessageReceived(UniOSCEventArgs args)
		{
			if(transformToMove == null) return;
			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1)return;

			float x = transformToMove.transform.position.x;
			float y =  transformToMove.transform.position.y;
			float z = transformToMove.transform.position.z;

			// 解析OSC消息地址
            string address = msg.Address;
            float value = 0;
            if (msg.Data[0] is float)
            {
                value = (float)msg.Data[0];
            }
            else
            {
                return; // 如果数据不是float类型，则直接返回
            }

            switch (movementMode) {

			case Mode.Screen:

				y = Screen.height * (float)msg.Data[0];
				
				if(msg.Data.Count >= 2){
					x = Screen.width* (float)msg.Data[1];
				}
				
				pos = new Vector3(x,y,Camera.main.nearClipPlane+nearClipPlaneOffset);
                //pos = transformToMove.transform.position; pos[0] = x;pos[1] = y;pos[2] = Camera.main.nearClipPlane + nearClipPlaneOffset;                   
                transformToMove.transform.position = Camera.main.ScreenToWorldPoint(pos);

				break;

                case Mode.Relative:
                    // 根据OSC消息的地址来决定更新哪个轴向
                    if (address.EndsWith("/10"))
                    { // 假设地址以"/x"结尾表示控制x轴
                        x += value;
                    }
                    else if (address.EndsWith("/11"))
                    { // 假设地址以"/y"结尾表示控制y轴
                        y += value;
                    }
                    else if (address.EndsWith("/12"))
                    { // 假设地址以"/z"结尾表示控制z轴
                        z += value;
                    }
                    break;


                    //case Mode.Relative:
                    //	z = 0f;
                    //	y =  (float)msg.Data[0];
                    //	if(msg.Data.Count >= 2){
                    //		x =  (float)msg.Data[1];
                    //	}
                    //	if(msg.Data.Count >= 3){
                    //		z =  (float)msg.Data[2];
                    //	}

                    //	pos = new Vector3(x,y,z);
                    //	transformToMove.transform.position += pos; 
                    //	break;

            }


        }

	}

}