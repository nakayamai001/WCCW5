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
	public class UniOSCMoveGameObject :  UniOSCEventTarget {

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
			float y = transformToMove.transform.position.y;
			float z = transformToMove.transform.position.z;

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
                    if (msg.Data[0] is float)
                    { // 确保数据可以被解析为float
                        float inputValue = (float)msg.Data[0]; // OSC消息提供的值，范围0到1
                        float startTarget = -8f; // z轴的目标开始范围
                        float endTarget = 50f; // z轴的目标结束范围

                        // 映射OSC消息的值到z轴的目标范围
                        float mappedZ = startTarget + (endTarget - startTarget) * inputValue;

                        // 应用更新后的位置
                        transformToMove.transform.position = new Vector3(x, y, mappedZ);
                    }
                    else
                    {
                        // 可能需要错误处理或转换逻辑
                    }
                    break;


            }


        }

	}

}