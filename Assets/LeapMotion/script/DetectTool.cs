	using UnityEngine;
	using System.Collections.Generic;
	using Leap;
	using Leap.Unity;
	using  UnityEngine.VR;
	using System.Collections;
	using UnityEngine.Events;
	using System;
	using UnityEngine.UI;
	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	public class DetectTool : Detector {

		//public GameObject showhan,thumptx,indextx,middletx,ringtx,pinkytx,gs1tx,gs2tx,gs3tx,gs4tx;
	public GameObject Cap1,Cap2;
		//Controller controller = new Controller();
	//	public Hand hand;
	//	public Frame frame;
			// Use this for initialization
		[Tooltip("The interval in seconds at which to check this detector's conditions.")]

		public float Period = .1f; //seconds
		[HideInInspector]
		public	bool Usinggesture = false;
		[HideInInspector]
		public bool Savegesture = false;
		public efxgo callgo; 
		Controller controller ;
	 	public Frame frame;
			 public string path;
		bool checkshoot = false;
		 public Hand hand;
		List<Finger> fingers;
		private int thump = 0;
		private int index = 0;
		private int middle = 0;
		private int ring = 0; 
		private int pinky = 0;
		private int unknow = 0;
		[Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
		public IHandModel HandModel = null;
		//public IHandModel HandModel;
		/** The required thumb state. */
		public  PointingState Thumb = PointingState.Either;
		/** The required index finger state. */
		public PointingState Index = PointingState.Either;
		/** The required middle finger state. */
		public PointingState Middle = PointingState.Either;
		/** The required ring finger state. */
		public PointingState Ring = PointingState.Either;
		/** The required pinky finger state. */
		public PointingState Pinky = PointingState.Either;

		public StateGesture state1 = StateGesture.Null;
		public StateGesture state2 = StateGesture.Null;
		public StateGesture state3 = StateGesture.Null;
		public StateGesture state4 = StateGesture.Null;

		public StateGesture current1 = StateGesture.Null;
		public StateGesture current2 = StateGesture.Null;
		public StateGesture current3 = StateGesture.Null;
		public StateGesture current4 = StateGesture.Null;

	public Text sen;
		public	bool fingerState = false;
		//public	bool fingerState1 = false;
		public bool gesturestate = false;
		private IEnumerator watcherCoroutine;
		public static PointingState requiredState;
		public float sensitivity;
		public float forwardforce;
	//Vector3 startpos;
		//public GameObject charac;

		float timeset = 10f;
		float timesetori;
		public UnityEvent Up;
		float timeshowtype = 0f;
		float timeorishowtype;
		bool zoom = false;
		Multihandgesture m1 = new Multihandgesture ();
		[Tooltip("Dispatched when condition is detected.")]

		[HideInInspector]
		public UnityEvent Down,Left,Right,Forward,Backward,StopUp,StopDown,StopLeft,StopRight,StopForward,StopBackward;
	public Text datapath;



		void Start () {
			timesetori = 10;
			timeorishowtype = 5f;
		forwardforce =3000f;
			Usinggesture = true;

		}



		void Awake () {
		#if UNITY_EDITOR
				if (EditorApplication.isPlaying)
				{
				Autoload ();
				}
		#endif

	
		if (Application.isPlaying) {
			path = Application.dataPath + "/left.gs";

			string[] loadfinger = System.IO.File.ReadAllLines (path);

				if (sen != null) {

					sen.text = loadfinger [9].ToString () + " : L";

				}
			
			sensitivity = Int32.Parse(loadfinger [9].ToString());

			Debug.Log ("Sensitivity Left : " + sensitivity);

			if (loadfinger [0].ToString () == "Extended") {
				Thumb = DetectTool.PointingState.Extended;
				//Debug.Log ("Do Extend");
			} else if (loadfinger [0].ToString () == "NotExtended") {
				Thumb = DetectTool.PointingState.NotExtended;

			} else
				Thumb = DetectTool.PointingState.Either;



			if (loadfinger [1].ToString () == "Extended") {
				Index = DetectTool.PointingState.Extended;
				//Debug.Log ("Do Extend");
			} else if (loadfinger [1].ToString () == "NotExtended") {
				Index = DetectTool.PointingState.NotExtended;

			} else
				Index = DetectTool.PointingState.Either;


			if (loadfinger [2].ToString () == "Extended") {
				Middle = DetectTool.PointingState.Extended;
				//Debug.Log ("Do Extend");
			} else if (loadfinger [2].ToString () == "NotExtended") {
				Middle = DetectTool.PointingState.NotExtended;

			} else
				Middle = DetectTool.PointingState.Either;



			if (loadfinger [3].ToString () == "Extended") {
				Ring = DetectTool.PointingState.Extended;
				//Debug.Log ("Do Extend");
			} else if (loadfinger [3].ToString () == "NotExtended") {
				Ring = DetectTool.PointingState.NotExtended;

			} else
				Ring = DetectTool.PointingState.Either;



			if (loadfinger [4].ToString () == "Extended") {
				Pinky = DetectTool.PointingState.Extended;
				//Debug.Log ("Do Extend");
			} else if (loadfinger [4].ToString () == "NotExtended") {
				Pinky = DetectTool.PointingState.NotExtended;

			} else
				Pinky = DetectTool.PointingState.Either;


			if (loadfinger [5].ToString () == "Up") {
				state1 = StateGesture.Up;

			} else if (loadfinger [5].ToString () == "Down") {

				state1 = StateGesture.Down;
			} else if (loadfinger [5].ToString () == "Left") {

				state1 = StateGesture.Left;

			} else if (loadfinger [5].ToString () == "Right") {

				state1 = StateGesture.Right;

			} else if (loadfinger [5].ToString () == "Forward") {


				state1 = StateGesture.Forward;
			} else if (loadfinger [5].ToString () == "Backward") {

				state1 = StateGesture.Backward;

			} else if (loadfinger [5].ToString () == "Rotate") {
				state1 = StateGesture.Rotate;

			} else
				state1 = StateGesture.Null;



			if (loadfinger [6].ToString () == "Up") {
				state2 = StateGesture.Up;

			} else if (loadfinger [6].ToString () == "Down") {

				state2 = StateGesture.Down;
			} else if (loadfinger [6].ToString () == "Left") {

				state2 = StateGesture.Left;


			} else if (loadfinger [6].ToString () == "Right") {

				state2 = StateGesture.Right;


			} else if (loadfinger [6].ToString () == "Forward") {


				state2 = StateGesture.Forward;
			} else if (loadfinger [6].ToString () == "Backward") {

				state2 = StateGesture.Backward;

			} else if (loadfinger [6].ToString () == "Rotate") {

				state2 = StateGesture.Rotate;
			} else
				state2 = StateGesture.Null;


			if (loadfinger [7].ToString () == "Up") {
				state3 = StateGesture.Up;

			} else if (loadfinger [7].ToString () == "Down") {

				state3 = StateGesture.Down;
			} else if (loadfinger [7].ToString () == "Left") {

				state3 = StateGesture.Left;


			} else if (loadfinger [7].ToString () == "Right") {

				state3 = StateGesture.Right;


			} else if (loadfinger [7].ToString () == "Forward") {


				state3 = StateGesture.Forward;
			} else if (loadfinger [7].ToString () == "Backward") {

				state3 = StateGesture.Backward;

			} else if (loadfinger [7].ToString () == "Rotate") {
				state3 = StateGesture.Rotate;
			} else
				state3 = StateGesture.Null;






			if (loadfinger [8].ToString () == "Up") {
				state4 = StateGesture.Up;

			} else if (loadfinger [8].ToString () == "Down") {

				state4 = StateGesture.Down;
			} else if (loadfinger [8].ToString () == "Left") {

				state4 = StateGesture.Left;


			} else if (loadfinger [8].ToString () == "Right") {

				state4 = StateGesture.Right;


			} else if (loadfinger [8].ToString () == "Forward") {


				state4 = StateGesture.Forward;
			} else if (loadfinger [8].ToString () == "Backward") {

				state4 = StateGesture.Backward;

			} else if (loadfinger [8].ToString () == "Rotate") {
				state4 = StateGesture.Rotate;
			} else
				state4 = StateGesture.Null;
		}
		
		}


		void OnEnable () {
			
			
			}

		void OnDisable () {
			current1 = StateGesture.Null;
			current2 = StateGesture.Null;
			current3 = StateGesture.Null;
			current4 = StateGesture.Null;

			timeset = timesetori;
			timeshowtype = timeorishowtype;
			//showhan.SetActive (false);
			gesturestate = false;
			Deactivate();
		}
		// Update is called once per frame
		void Update () {

			watcherCoroutine = extendedFingerWatcher();
			if (HandModel == null) {
				HandModel = gameObject.GetComponentInParent<IHandModel> ();

			}

		if (controller == null) {
			controller = new Controller ();
		}
			frame = controller.Frame ();




		if (frame.Hands.Count > 0) {
			if (Cap1.activeInHierarchy) {
				List<Hand> hands = frame.Hands;
				hand = hands [0];
				fingers = hand.Fingers;	
			} else if (Cap2.activeInHierarchy) {

				List<Hand> hands = frame.Hands;
				hand = hands [1];
				fingers = hand.Fingers;	

			}

		}
			

		#region Debug
			if (Input.GetKeyDown (KeyCode.S)) {
				Savegesture =  !Savegesture;
				if (Savegesture == true) {

					Debug.Log ("Save : 0n");
				}else 
					Debug.Log ("Save : off");


			}

			if (Savegesture) {
				//showhan.SetActive (true);
				Usinggesture = false;
				StartCoroutine (GetInfo ());
				StartCoroutine (gesturewatch ());
				timeset -= Time.deltaTime;
				//showhan.GetComponent<Text> ().text = "Recording : " + timeset;
				print ("Time :" + timeset);
				if (timeset <= 0) {
					//showhan.GetComponent<Text> ().text = "Finish And Save";
					Savegesture = false;
				#if UNITY_EDITOR
					string path = EditorUtility.SaveFilePanel ("Save Gesture", "", "gesturename", "gs");
				#endif
					if (path.Length != 0) {
						string[] statusfinger = new string[9] {Thumb.ToString (), Index.ToString (), Middle.ToString (), Ring.ToString ()
							, Pinky.ToString (), current1.ToString (), current2.ToString (), current3.ToString (), current4.ToString ()
						};

						System.IO.File.WriteAllLines (path, statusfinger);
						Savegesture = false;

					}


				} 
					

					
			} else {
				
				timeset = timesetori;
				StopCoroutine (GetInfo ());
				StopCoroutine (gesturewatch ());

			}

			if (Input.GetKeyDown(KeyCode.U)){
				Usinggesture = !Usinggesture;
				if (Usinggesture == true) {

					Debug.Log ("Using : on");
				} else
					Debug.Log ("Using : Off");

			}
			//Debug.Log ("UsingGesture :"+Usinggesture);
			if (Usinggesture) {
				//showhan.SetActive (true);
				Savegesture = false;
				//showhan.GetComponent<Text>().text = "Use Gesture Mode";
				StartCoroutine (watcherCoroutine);
			} else
				StopCoroutine (watcherCoroutine);

		#endregion

		}
		#region Debughand
		void Debughand()

		{
			
			 
			for (int i = 0; i < frame.Hands.Count; i++) {
				Hand leaphand = frame.Hands [i];
			
				Vector handXbasic = leaphand.PalmNormal.Cross (leaphand.Direction).Normalized;
				Vector handYbasic = -leaphand.PalmNormal;
				Vector handZbasic = -leaphand.Direction;
				Vector handOrigin = leaphand.PalmPosition;


				Debug.Log ("handx :" + handXbasic +"\n handy :"+handYbasic+" \n handz :"+handZbasic  );//			Matrix4x4 handTransform = new Matrix4x4 (handXbasic, handYbasic, handZbasic, handOrigin);

	//			UnityMatrixExtension handTransform = new UnityMatrixExtension (handXbasic,handYbasic,handZbasic,handOrigin);
			}

		}
		#endregion


		IEnumerator extendedFingerWatcher() {
			Hand hand;
			while(true){
				
				if(HandModel != null && HandModel.IsTracked){
					hand = HandModel.GetLeapHand();
					if (hand != null) {
						fingerState = matchFingerState (hand.Fingers [0], 0)
						&& matchFingerState (hand.Fingers [1], 1)
						&& matchFingerState (hand.Fingers [2], 2)
						&& matchFingerState (hand.Fingers [3], 3)
						&& matchFingerState (hand.Fingers [4], 4);
						
						

					if (fingers != null) {
						StartCoroutine (gesturewatch ());
					}
						//setuipanel ();	

						if (!Multihandgesture.multigesture)
						{
							if (HandModel.IsTracked && fingerState) {
								if ((current1 == state1 && current2 == state2 && current3
									== state3 && current4 == state4) == true) {
									gesturestate = true;
								if (gesturestate) {
									//zoom = true;
									Activate ();


								} else {
									Deactivate ();
									StopDown.Invoke ();
									StopBackward.Invoke ();
									StopForward.Invoke ();
									StopLeft.Invoke ();
									StopRight.Invoke ();

								}


								//TODO 
								//for zoom  
							} else if(current1 == state1 || current2 == state1 || current3 == state1 || current4 == state1 ) {

								gesturestate = true;
								if (gesturestate) {
									//zoom = true;
									Activate ();


								} else {
									Deactivate ();
									StopDown.Invoke ();
									StopBackward.Invoke ();
									StopForward.Invoke ();
									StopLeft.Invoke ();
									StopRight.Invoke ();

								}


								}
									
							} else if (!HandModel.IsTracked || !fingerState) {
								Deactivate ();
								StopDown.Invoke ();
								StopBackward.Invoke ();
								StopForward.Invoke ();
								StopLeft.Invoke ();
								StopRight.Invoke ();
								//zoom = false;
						}
					}


					}
				} else if(IsActive){
					Deactivate();
					//zoom = false;
				}

				yield return new WaitForSeconds(Period);
			}
		}



		IEnumerator gesturewatch()
		{
			
			Vector avgPos = Vector.Zero;
			Vector avgVelocity = Vector.Zero;
			Vector handmotion = Vector.Zero;
		//Vector rotating = Vector.Zero;
			float speedmotionup = 700f;

			foreach (Finger finger in fingers)
			{

				avgPos += finger.TipPosition;
				avgVelocity += finger.TipVelocity;


			}

			avgPos /= fingers.Count;
			avgVelocity /= fingers.Count;


			if (fingerState == false) {

				gesturestate = false;
				current1 = StateGesture.Null;
				current2 = StateGesture.Null;
				current3 = StateGesture.Null;
				current4 = StateGesture.Null;
			//TODO RESET Gesture With outhand off

			}
		
		if (!Multihandgesture.LeapAxis) {

			if (gesturestate) {
				if (avgVelocity.y > speedmotionup) {
					//forward
					//					timeshowtype -= Time.deltaTime;
					//					if (timeshowtype <= 0) {
					//						callgo.Startgo ();	
					//						timeshowtype = 1f;
					//					} 



				} else if (avgVelocity.y < -sensitivity) {

					//backward
					Backward.Invoke ();



				} else if (hand.Rotation.x > 0.2) {


					//rotate

				} else if (avgVelocity.x > sensitivity) {

					Left.Invoke ();
					//left

				} else if (avgVelocity.x < -sensitivity) {

					Right.Invoke ();
					//right

				} else if (avgVelocity.z > sensitivity) {
					Down.Invoke ();
					//down

				} else if (avgVelocity.z < -sensitivity) {
					Up.Invoke ();

					//up


				}

			} else if (!gesturestate) {


				if (avgVelocity.y < -sensitivity) 
				{

					//backward
					Backward.Invoke();



				} else if (hand.Rotation.x > 0.2) {


					//rotate

				} else if (avgVelocity.x > sensitivity) {

					Left.Invoke ();
					//left

				} else if (avgVelocity.x < -sensitivity)
				{

					Right.Invoke ();
					//right

				} else if (avgVelocity.z > sensitivity) 
				{
					Down.Invoke ();
					//down

				}else if(avgVelocity.z < -sensitivity)
				{
					Up.Invoke ();

					//up


				}if (avgVelocity.y > speedmotionup) {


					Forward.Invoke ();
					//forward

				}

			}

			if (hand.Rotation.x > 0.2) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Rotate) {
					//current1 = StateGesture.Rotate;
					//					if(gs1tx != null)
					//						
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Rotate";
				} else if (current2 == StateGesture.Null || current2 == StateGesture.Rotate) {
					//current2 = StateGesture.Rotate;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Rotate";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Rotate) {
					
					//current3 = StateGesture.Rotate;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Rotate";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Rotate) {

					//current4 = StateGesture.Rotate;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Rotate";
				}

			}



			if (avgVelocity.y > sensitivity) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Forward) {
					current1 = StateGesture.Forward;

					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Forward";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Forward) {
					current2 = StateGesture.Forward;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Forward";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Forward) {

					current3 = StateGesture.Forward;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Forward";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Forward) {
					current4 = StateGesture.Forward;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Forward";
				}

				//isUp = true;



			} else if (avgVelocity.y < -sensitivity) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Backward) {
					current1 = StateGesture.Backward;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Backward";
				} else if (current2 == StateGesture.Null || current2 == StateGesture.Backward) {
					current2 = StateGesture.Backward;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Backward";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Backward) {

					current3 = StateGesture.Backward;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Backward";
				} else if (current4 == StateGesture.Down || current4 == StateGesture.Backward) {

					current4 = StateGesture.Backward;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Backward";
				}


			}

			if (avgVelocity.x > sensitivity) {
				//isRight = true;
				if (current1 == StateGesture.Null || current1 == StateGesture.Left) {
					current1 = StateGesture.Left;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Left";
				} else if (current2 == StateGesture.Null || current2 == StateGesture.Left) {
					current2 = StateGesture.Left;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Left";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Left) {

					current3 = StateGesture.Left;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Left";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Left) {

					current4 = StateGesture.Left;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Left";
				}


			} else if (avgVelocity.x < -sensitivity) {

				//isLeft = true;
				if (current1 == StateGesture.Null || current1 == StateGesture.Right) {
					current1 = StateGesture.Right;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Right";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Right) {
					current2 = StateGesture.Right;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Right";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Right) {

					current3 = StateGesture.Right;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Right";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Right) {

					current4 = StateGesture.Right;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Right";
				}
			}

			if (avgVelocity.z > sensitivity) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Down) {
					current1 = StateGesture.Down;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Down";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Down) {
					current2 = StateGesture.Down;

					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Down";
				} else if (current3 == StateGesture.Null || current3 == StateGesture.Down) {
					current3 = StateGesture.Down;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Down";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Down) {

					current4 = StateGesture.Down;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Down";
				}
				//isBackward = true;

			} else if (avgVelocity.z < -sensitivity) {

				//isForward = true;
				if (current1 == StateGesture.Null || current1 == StateGesture.Up) {
					current1 = StateGesture.Up;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Up";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Up) {
					current2 = StateGesture.Up;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Up";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Up) {

					current3 = StateGesture.Up;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Up";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Up) {

					current4 = StateGesture.Up;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Up";
				}


			}

		} else if (Multihandgesture.LeapAxis) {

			if (gesturestate) {
				if (avgVelocity.z < -sensitivity) {
					//forward
					timeshowtype -= Time.deltaTime;
					if (timeshowtype <= 0) {
						//callgo.Startgo ();
						if (Up != null) {

						}
						timeshowtype = 1f;
					} 



				} else if (avgVelocity.y < -sensitivity) {

					//down
					Down.Invoke ();



				} else if (hand.Rotation.x > 0.2) {


					//rotate

				} else if (avgVelocity.x > sensitivity) {

					Right.Invoke ();
					//right

				} else if (avgVelocity.x < -sensitivity) {
					Left.Invoke ();

					//left

				} else if (avgVelocity.z > sensitivity) {

					Backward.Invoke ();
					//backward

				} else if (avgVelocity.y > sensitivity) {

					//up
					Up.Invoke();

				}


			} else if (!gesturestate) {


				if (avgVelocity.z < -sensitivity) {
					//forward
					timeshowtype -= Time.deltaTime;
					if (timeshowtype <= 0) {
						///callgo.Startgo ();
						if (Up != null) {

						}
						timeshowtype = 1f;
					} 



				} else if (avgVelocity.y < -sensitivity) {

					//down
					Down.Invoke ();



				} else if (hand.Rotation.x > 0.2) {


					//rotate

				} else if (avgVelocity.x > sensitivity) {

					Right.Invoke ();
					//right

				} else if (avgVelocity.x < -sensitivity) {
					Left.Invoke ();

					//left

				} else if (avgVelocity.z > sensitivity) {

					Backward.Invoke ();
					//backward

				} else if (avgVelocity.y > sensitivity) {

					//up
					Up.Invoke();
				}





			}

			if (hand.Rotation.x > 0) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Rotate) {
					//current1 = StateGesture.Rotate;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Rotate";
				} else if (current2 == StateGesture.Null || current2 == StateGesture.Rotate) {
					//current2 = StateGesture.Rotate;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Rotate";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Rotate) {

					//current3 = StateGesture.Rotate;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Rotate";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Rotate) {

					//current4 = StateGesture.Rotate;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Rotate";
				}

			} 





			if (avgVelocity.y > sensitivity) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Up) {
					current1 = StateGesture.Up;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Up";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Up) {
					current2 = StateGesture.Up;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Up";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Up) {

					current3 = StateGesture.Up;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Up";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Up) {
					current4 = StateGesture.Up;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Up";
				}

				//isUp = true;



			} else if (avgVelocity.y < -sensitivity) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Down) {
					current1 = StateGesture.Down;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Down";
				} else if (current2 == StateGesture.Null || current2 == StateGesture.Down) {
					current2 = StateGesture.Down;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Down";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Down) {

					current3 = StateGesture.Down;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Down";
				} else if (current4 == StateGesture.Down || current4 == StateGesture.Down) {

					current4 = StateGesture.Down;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Down";
				}
				//isDown = true;

			}

			if (avgVelocity.x > sensitivity) {
				//isRight = true;
				if (current1 == StateGesture.Null || current1 == StateGesture.Right) {
					current1 = StateGesture.Right;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Right";
				} else if (current2 == StateGesture.Null || current2 == StateGesture.Right) {
					current2 = StateGesture.Right;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Right";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Right) {

					current3 = StateGesture.Right;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Right";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Right) {

					current4 = StateGesture.Right;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Right";
				}


			} else if (avgVelocity.x < -sensitivity) {

				//isLeft = true;
				if (current1 == StateGesture.Null || current1 == StateGesture.Left) {
					current1 = StateGesture.Left;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Left";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Left) {
					current2 = StateGesture.Left;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Left";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Left) {

					current3 = StateGesture.Left;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Left";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Left) {

					current4 = StateGesture.Left;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Left";
				}
			}

			if (avgVelocity.z > sensitivity) {

				if (current1 == StateGesture.Null || current1 == StateGesture.Backward) {
					current1 = StateGesture.Backward;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Backward";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Backward) {
					current2 = StateGesture.Backward;

					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Backward";
				} else if (current3 == StateGesture.Null || current3 == StateGesture.Backward) {
					current3 = StateGesture.Backward;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Backward";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Backward) {

					current4 = StateGesture.Backward;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Backward";
				}
				//isBackward = true;

			} else if (avgVelocity.z < -sensitivity) {

				//isForward = true;
				if (current1 == StateGesture.Null || current1 == StateGesture.Forward) {
					current1 = StateGesture.Forward;
					//					gs1tx.GetComponent<Text> ().text = "Gesture1 : Forward";

				} else if (current2 == StateGesture.Null || current2 == StateGesture.Forward) {
					current2 = StateGesture.Forward;
					//					gs2tx.GetComponent<Text> ().text = "Gesture2 : Forward";

				} else if (current3 == StateGesture.Null || current3 == StateGesture.Forward) {

					current3 = StateGesture.Forward;
					//					gs3tx.GetComponent<Text> ().text = "Gesture3 : Forward";
				} else if (current4 == StateGesture.Null || current4 == StateGesture.Forward) {

					current4 = StateGesture.Forward;
					//					gs4tx.GetComponent<Text> ().text = "Gesture4 : Forward";
				}



			}

		}


			yield return new WaitForSeconds (Period);

		}

		public bool matchFingerState (Finger finger, int ordinal) {
	//		PointingState requiredState;
			switch (ordinal) {
			case 0:
				requiredState = Thumb;
				break;
			case 1:
				requiredState = Index;
				break;
			case 2:
				requiredState = Middle;
				break;
			case 3:
				requiredState = Ring;
				break;
			case 4:
				requiredState = Pinky;
				break;
			default:
				return false;
			}
			return (requiredState == PointingState.Either) ||
				(requiredState == PointingState.Extended && finger.IsExtended) ||
				(requiredState == PointingState.NotExtended && !finger.IsExtended);
		}



		IEnumerator GetInfo()
		{
			yield return new WaitForSeconds (0);


			for(int i =0 ;i<hand.Fingers.Count ;i++)
			{
				

					Finger digit = hand.Fingers[i];
						if (digit.Type == Finger.FingerType.TYPE_THUMB) {



					if (hand.Fingers [0].IsExtended) {
						thump = 1;
						Thumb = PointingState.Extended;
	//					thumptx.GetComponent<Text>().text = "Thump : Extend";
						//Debug.Log ("Finger Type is : " + Finger.FingerType.TYPE_THUMB + " IsExtend: " + hand.Fingers [0].IsExtended);
					} else {
						Thumb = PointingState.NotExtended;
	//					thumptx.GetComponent<Text>().text = "Thump : NotExtend";
					}
						
			
						} else if (digit.Type == Finger.FingerType.TYPE_INDEX) {
					if (hand.Fingers [1].IsExtended) {
						index = 1;
						Index = PointingState.Extended;
	//					indextx.GetComponent<Text>().text = "Index : Extend";
						//Debug.Log ("Finger Type is : " + Finger.FingerType.TYPE_INDEX + " IsExtend: " + hand.Fingers [1].IsExtended);
					} else {
						Index = PointingState.NotExtended;
	//					indextx.GetComponent<Text>().text = "Index : NotExtend";
					}

						} else if (digit.Type == Finger.FingerType.TYPE_MIDDLE) {
					if (hand.Fingers [2].IsExtended) {
						middle = 1;
						Middle = PointingState.Extended;
	//					middletx.GetComponent<Text>().text = "Middle : Extend";
						//Debug.Log ("Finger Type is : " + Finger.FingerType.TYPE_MIDDLE + " IsExtend: " + hand.Fingers [2].IsExtended);
					} else {

						Middle = PointingState.NotExtended;
	//					middletx.GetComponent<Text>().text = "Middle : NotExtend";
					}

						} else if (digit.Type == Finger.FingerType.TYPE_RING) {
			
					if (hand.Fingers [3].IsExtended) {
						ring = 1;
						Ring = PointingState.Extended;
	//					ringtx.GetComponent<Text>().text = "Ring : Extend";
						//Debug.Log ("Finger Type is : " + Finger.FingerType.TYPE_RING + " IsExtend: " + hand.Fingers [3].IsExtended);
					} else {

						Ring = PointingState.NotExtended;
	//					ringtx.GetComponent<Text>().text = "Ring : NotExtend";
					}

						} else if (digit.Type == Finger.FingerType.TYPE_PINKY) {
					if (hand.Fingers [4].IsExtended) {
						pinky = 1;
						Pinky = PointingState.Extended;
	//					pinkytx.GetComponent<Text>().text = "Pinky : Extend";
						//Debug.Log ("Finger Type is : " + Finger.FingerType.TYPE_PINKY + " IsExtend: " + hand.Fingers [4].IsExtended);
					} else {

						Pinky = PointingState.NotExtended;
	//					pinkytx.GetComponent<Text>().text = "Pinky : NotExtend";
					}

						}

			}

	}


		void Autoload()
		{

			string[] loadfinger = System.IO.File.ReadAllLines (path);
			
		sensitivity = Int32.Parse(loadfinger [9].ToString());

		Debug.Log ("Sensitivity Right : " + sensitivity);

			if (loadfinger [0].ToString () == "Extended") {
				Thumb = DetectTool.PointingState.Extended;
				Debug.Log ("Do Extend");
			} else if (loadfinger [0].ToString () == "NotExtended") {
				Thumb = DetectTool.PointingState.NotExtended;

			}else
				Thumb = DetectTool.PointingState.Either;



			if (loadfinger [1].ToString () == "Extended") {
				Index = DetectTool.PointingState.Extended;
				Debug.Log ("Do Extend");
			} else if (loadfinger [1].ToString () == "NotExtended") {
				Index = DetectTool.PointingState.NotExtended;

			}else
				Index = DetectTool.PointingState.Either;


			if (loadfinger [2].ToString () == "Extended") {
				Middle = DetectTool.PointingState.Extended;
				Debug.Log ("Do Extend");
			} else if (loadfinger [2].ToString () == "NotExtended") {
				Middle = DetectTool.PointingState.NotExtended;

			}else
				Middle = DetectTool.PointingState.Either;



			if (loadfinger [3].ToString () == "Extended") {
				Ring = DetectTool.PointingState.Extended;
				Debug.Log ("Do Extend");
			} else if (loadfinger [3].ToString () == "NotExtended") {
				Ring = DetectTool.PointingState.NotExtended;

			}else
				Ring = DetectTool.PointingState.Either;



			if (loadfinger [4].ToString () == "Extended") {
				Pinky = DetectTool.PointingState.Extended;
				Debug.Log ("Do Extend");
			} else if (loadfinger [4].ToString () == "NotExtended") {
				Pinky = DetectTool.PointingState.NotExtended;

			}else
				Pinky = DetectTool.PointingState.Either;

		
			if (loadfinger [5].ToString () == "Up") {
				state1 = StateGesture.Up;

			} else if (loadfinger [5].ToString () == "Down") {

				state1 = StateGesture.Down;
			} else if (loadfinger [5].ToString () == "Left") {

				state1 = StateGesture.Left;

			} else if (loadfinger [5].ToString () == "Right") {

				state1 = StateGesture.Right;

			} else if (loadfinger [5].ToString () == "Forward") {


				state1 = StateGesture.Forward;
			} else if (loadfinger [5].ToString () == "Backward") {

				state1 = StateGesture.Backward;

			} else if (loadfinger [5].ToString () == "Rotate") {
				state1 = StateGesture.Rotate;

			} else
				state1 = StateGesture.Null;



			if (loadfinger [6].ToString () == "Up") {
				state2 = StateGesture.Up;

			} else if (loadfinger [6].ToString () == "Down") {

				state2 = StateGesture.Down;
			} else if (loadfinger [6].ToString () == "Left") {

				state2 = StateGesture.Left;


			} else if (loadfinger [6].ToString () == "Right") {

				state2 = StateGesture.Right;


			} else if (loadfinger [6].ToString () == "Forward") {


				state2 = StateGesture.Forward;
			}else if(loadfinger[6].ToString() == "Backward" ){

				state2 = StateGesture.Backward;

			}else if(loadfinger[6].ToString() == "Rotate"){
						
						state2 = StateGesture.Rotate;
			}else 
						state2 = StateGesture.Null;


			if (loadfinger [7].ToString () == "Up") {
				state3 = StateGesture.Up;

			} else if (loadfinger [7].ToString () == "Down") {

				state3 = StateGesture.Down;
			} else if (loadfinger [7].ToString () == "Left") {

				state3 = StateGesture.Left;


			} else if (loadfinger [7].ToString () == "Right") {

				state3 = StateGesture.Right;


			} else if (loadfinger [7].ToString () == "Forward") {


				state3 = StateGesture.Forward;
			} else if (loadfinger [7].ToString () == "Backward") {

				state3 = StateGesture.Backward;

			} else if(loadfinger [7].ToString () == "Rotate"){
						state3 = StateGesture.Rotate;
			}else
						state3 = StateGesture.Null;






			if (loadfinger [8].ToString () == "Up") {
				state4 = StateGesture.Up;

			} else if (loadfinger [8].ToString () == "Down") {

				state4 = StateGesture.Down;
			} else if (loadfinger [8].ToString () == "Left") {

				state4 = StateGesture.Left;


			} else if (loadfinger [8].ToString () == "Right") {

				state4 = StateGesture.Right;


			} else if (loadfinger [8].ToString () == "Forward") {


				state4 = StateGesture.Forward;
			} else if (loadfinger [8].ToString () == "Backward") {

				state4 = StateGesture.Backward;

					} else if(loadfinger [8].ToString () == "Rotate"){
						state4 = StateGesture.Rotate;
					}else 
						state4 = StateGesture.Null;

		
		}



		IEnumerator usegesture()
		{
			yield return new WaitForSeconds (1f);


			Usinggesture = true;


		}
		
		IEnumerator savesystem()
		{


			yield return new WaitForSeconds (2f);

			//showhan.SetActive (false);

	//		StartCoroutine()

			Savegesture = true;


		}


		public enum PointingState{Extended, NotExtended, Either}
		public enum StateGesture{ Up,Down,Left,Right,Forward,Backward,Rotate,Null }
		public enum StateRotate{

		IsBegin,
		IsStart,
		Null


	}
	}

		