using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System;
using Leap;
using Leap.Unity;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
	// Use this for initialization
#if UNITY_EDITOR
[CustomEditor(typeof(DetectTool))]

	public class CaptureGes : Editor 
	{
	Hand han;
	int count = 1;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

		DetectTool  myScript = (DetectTool)target;

		if (EditorApplication.isPlaying) {

//			if(GUILayout.Button("Creat Gesture Object  :"+count))
//			{
//				
//
//			}

			if (GUILayout.Button ("Capture "+"Status : "+myScript.Savegesture)) {
				myScript.Savegesture = !myScript.Savegesture;	
				myScript.Usinggesture = false;

			}



				
			if (GUILayout.Button ("Test Gesture  "+"Status : "+myScript.Usinggesture)) {

				myScript.Usinggesture = !myScript.Usinggesture;
				myScript.Savegesture = false;
			} 

			if(GUILayout.Button ("Save Gesture", EditorStyles.toolbarButton))
			{

				string path = EditorUtility.SaveFilePanel ("Save Gesture", "", "gesturename", "gs");
				if(path.Length !=0)
				{
					string[] statusfinger = new string[9] {myScript.Thumb.ToString(),myScript.Index.ToString(),myScript.Middle.ToString(),myScript.Ring.ToString(),myScript.Pinky.ToString(),myScript.current1.ToString(),myScript.current2.ToString(),myScript.current3.ToString(),myScript.current4.ToString()};

					System.IO.File.WriteAllLines (path, statusfinger);
//					System.IO.File.WriteAllText (path, myScript.Pinky.ToString ());

				}
					

				
			}





			}

		if (!EditorApplication.isPlaying) {

			GUILayout.Label ("Load from path", EditorStyles.boldLabel);
			GUILayout.TextField (myScript.path, EditorStyles.textField);

			if(GUILayout.Button ("Load Gesture", EditorStyles.toolbarButton))
			{	


				myScript.path = EditorUtility.OpenFilePanel ("Load Gesture", "", "gs");


				//PlayerPrefs.SetString ("path", path);
				//				if(myScript.path.Length !=0)
				//				{
				//					
				//					string[] loadfinger = System.IO.File.ReadAllLines (myScript.path);
				//
				//					if (loadfinger [0].ToString () == "Extended") {
				//						myScript.Thumb = DetectTool.PointingState.Extended;
				//						Debug.Log ("Do Extend");
				//					} else if (loadfinger [0].ToString () == "NotExtended") {
				//						myScript.Thumb = DetectTool.PointingState.NotExtended;
				//
				//					}else
				//						myScript.Thumb = DetectTool.PointingState.Either;
				//
				//
				//
				//					if (loadfinger [1].ToString () == "Extended") {
				//						myScript.Index = DetectTool.PointingState.Extended;
				//						Debug.Log ("Do Extend");
				//					} else if (loadfinger [1].ToString () == "NotExtended") {
				//						myScript.Index = DetectTool.PointingState.NotExtended;
				//
				//					}else
				//						myScript.Index = DetectTool.PointingState.Either;
				//
				//
				//					if (loadfinger [2].ToString () == "Extended") {
				//						myScript.Middle = DetectTool.PointingState.Extended;
				//						Debug.Log ("Do Extend");
				//					} else if (loadfinger [2].ToString () == "NotExtended") {
				//						myScript.Middle = DetectTool.PointingState.NotExtended;
				//
				//					}else
				//						myScript.Middle = DetectTool.PointingState.Either;
				//
				//
				//
				//					if (loadfinger [3].ToString () == "Extended") {
				//						myScript.Ring = DetectTool.PointingState.Extended;
				//						Debug.Log ("Do Extend");
				//					} else if (loadfinger [3].ToString () == "NotExtended") {
				//						myScript.Ring = DetectTool.PointingState.NotExtended;
				//
				//					}else
				//						myScript.Ring = DetectTool.PointingState.Either;
				//					
				//
				//
				//					if (loadfinger [4].ToString () == "Extended") {
				//						myScript.Pinky = DetectTool.PointingState.Extended;
				//						Debug.Log ("Do Extend");
				//					} else if (loadfinger [4].ToString () == "NotExtended") {
				//						myScript.Pinky = DetectTool.PointingState.NotExtended;
				//
				//					}else
				//						myScript.Pinky = DetectTool.PointingState.Either;
				//

				//					if(loadfinger[5].ToString() == "")

				//Debug.Log (loadfinger [0].ToString());

				//					string[] temp = new string[loadfinger.Length];

				//Debug.Log ("Temp :" + temp);

				//					for (int i = 0; i < loadfinger.Length; i++) {
				//						temp [i] = loadfinger [i];
				//					}
				//
				//					Debug.Log ("Load Finger " + loadfinger);

				//string[] s = loadfinger.Split ("\\n");

				//					foreach(string fingerstatus in loadfinger)
				//					{
				//						//string fingle = fingerstatus.ToString ();
				//
				//						//Debug.Log (fingerstatus[]);
				//
				//						if (fingerstatus [0].ToString () == "Extended") {
				//							myScript.Thumb = DetectTool.PointingState.Extended;
				//
				//
				//						} else if (fingerstatus [0].ToString () == "NotExtended") {
				//
				//							myScript.Thumb = DetectTool.PointingState.NotExtended;
				//
				//						} else {
				//
				//
				//							myScript.Thumb = DetectTool.PointingState.Either;
				//						}
				//
				//
				//						if (fingerstatus [1].ToString () == "Extended") {
				//							myScript.Index = DetectTool.PointingState.Extended;
				//
				//
				//						} else if (fingerstatus [1].ToString () == "NotExtended") {
				//
				//							myScript.Index = DetectTool.PointingState.NotExtended;
				//
				//						} else {
				//
				//
				//							myScript.Index = DetectTool.PointingState.Either;
				//						}
				//
				//
				//
				//						if (fingerstatus [2].ToString () == "Extended") {
				//							myScript.Middle = DetectTool.PointingState.Extended;
				//
				//
				//						} else if (fingerstatus [2].ToString () == "NotExtended") {
				//
				//							myScript.Middle = DetectTool.PointingState.NotExtended;
				//
				//						} else {
				//
				//
				//							myScript.Middle = DetectTool.PointingState.Either;
				//						}
				//
				//
				//
				//						if (fingerstatus [3].ToString () == "Extended") {
				//							myScript.Ring = DetectTool.PointingState.Extended;
				//
				//
				//						} else if (fingerstatus [3].ToString () == "NotExtended") {
				//
				//							myScript.Ring = DetectTool.PointingState.NotExtended;
				//
				//						} else {
				//
				//
				//							myScript.Ring = DetectTool.PointingState.Either;
				//						}
				//
				//						if (fingerstatus [4].ToString () == "Extended") {
				//							myScript.Pinky = DetectTool.PointingState.Extended;
				//
				//
				//						} else if (fingerstatus [4].ToString () == "NotExtended") {
				//
				//							myScript.Pinky = DetectTool.PointingState.NotExtended;
				//
				//						} else {
				//
				//
				//							myScript.Pinky = DetectTool.PointingState.Either;
				//						}
				//
				//
				//
				//					}



			}// myScript.Usinggesture = true;
		}
		}

		}	
#endif
//	[Serializable]
//	 class GestureSave : DetectTool
//	{ 
//		
//		public string thumps,indexs,middles,rings,pinkys;
//
//
//
//
//
//
//
//
//
//	}


