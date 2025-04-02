using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

public class Multihandgesture : Detector
{
	//public GameObject zoombut;
	public CharacterSelectMenu charselect;
	public static bool multigesture = false;
	bool handone = false;
	bool handtwo = false;
	public bool gesturestate1 = false;
	public bool gesturestate2 = false;
	float speed;
	public GameObject character;
	public GameObject cam;
	public PinchDetector p1, p2;
	float timeforreset = 5f;
	bool zoomin = false;
	bool zoomout = false;
	//public GameObject zoomdist;
	public  bool zoom;
	//	public PinchDetector pina;
	//	public PinchDetector pinb;
	public Transform pin1, pin2;
	[SerializeField]
	public DetectTool hand1;
	public DetectTool1 hand2;
	public string state1;
	public string state2;
	public ItemSwitch switchs;
	public bool SwipeGesture = true;
	public GameObject[] capsulhand;
	public GameObject[] carouselobj;
	public StateDO input = StateDO.Null;
	public float cooldown =  1f;
	float timeStamp;
	bool isPinching;
	public Text uielement;
	public Text InputText;
	public bool UIshow;	
	public Text LeapCheckAxis;
	public static bool LeapAxis = true;
	public GameObject uisen;


	public bool IsLeapAxis;

	// Use this for initialization
	void Start ()
	{
		input = StateDO.Null;
		Cursor.visible = false;
		UIshow = false;
	}

	//	void OnDisable()
	//	{
	//
	//		current1 = Zoominout.Null;
	//		current2 = Zoominout.Null;
	//
	//
	//
	//	}
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log ("Is Pinch: "+p1.IsPinching );
		LeapAxis = IsLeapAxis;
//		if (p1.IsPinching && p1.DidStartHold ) {
//
//			Debug.Log ("start");
//		}
		//Debug.Log ("Angle : " + hand2.hand.GrabAngle);
		state1 = hand1.current1.ToString ();
		state2 = hand2.current1.ToString ();

		if (Input.GetKeyDown (KeyCode.G)) {

			//SwipeGesture = !SwipeGesture;
			UIshow = !UIshow;


		}

//		Debug.Log ("Grab in pinch1:" + hand1.hand.GrabStrength);
//		Debug.Log ("Grab in pinch2 :" + hand2.hand.GrabStrength);

		//Debug.Log("Swipe")


		if (UIshow) {
	
			if (uielement != null) {
				uielement.gameObject.SetActive (true);
				uielement.text = "SwipeGesture : " + SwipeGesture;

			} 
			if (InputText != null) {
				InputText.gameObject.SetActive (true);
				InputText.text = "Input : " + input.ToString ();

			}

			if (LeapCheckAxis != null) {

				if (Input.GetKeyDown (KeyCode.L)) {

					LeapAxis = !LeapAxis;
					IsLeapAxis = !IsLeapAxis;
				}


				LeapCheckAxis.gameObject.SetActive (true);
				LeapCheckAxis.text = "AxisLeap  : " + LeapAxis;

			}


			if (uisen != null) {

				uisen.gameObject.SetActive (true);

			}

		} else {
			uielement.gameObject.SetActive (false);
			InputText.gameObject.SetActive (false);
			LeapCheckAxis.gameObject.SetActive (false);
			uisen.gameObject.SetActive (false);



		}



		if (SwipeGesture) {

			switch(input)
			{
			case StateDO.Select:
				SelectMenu ();
				break;
			case StateDO.SwipeUp:
				SwipeUpState ();
				break;
			case StateDO.Back:	
				SwipeDownState();
				break;
			case StateDO.SwipeLeft:
				SwipeLeftState ();
				break;
			case StateDO.SwipeRight:
				SwipeRightState ();
				break;
			case StateDO.Null:
				break;
			default :
				break;

			}





			if( ((hand1.hand.GrabStrength == 1)  && (p1.DidStartHold)) ||( (hand2.hand.GrabStrength ==1) && (p2.DidStartHold) )){



				if (input == StateDO.Null || input == StateDO.Select) {
					if (!MultiCarouselsController.ObjectScene) {
						input = StateDO.Select;
					}

				}


			} 




			if ((hand1.hand.Fingers [0].IsExtended && hand1.hand.Fingers [4].IsExtended &&  (!hand1.hand.Fingers [1].IsExtended && !hand1.hand.Fingers [2].IsExtended && !hand1.hand.Fingers [3].IsExtended )) ||
				(hand2.hand.Fingers [0].IsExtended && hand2.hand.Fingers[4].IsExtended && (!hand2.hand.Fingers [1].IsExtended && !hand2.hand.Fingers [2].IsExtended && !hand2.hand.Fingers [3].IsExtended ))  ) 
			{
				Debug.Log ("ComeBack");


					if (input == StateDO.Null || input == StateDO.Back) {
					if (!p1.IsHolding || !p2.IsHolding) {
						input = StateDO.Back;
					}

					}


			}



			if ((hand1.current1 == DetectTool.StateGesture.Left && hand1.current2 == DetectTool.StateGesture.Null && hand1.current3 == DetectTool.StateGesture.Null && hand1.current4 == DetectTool.StateGesture.Null) ||
			    (hand2.current1 == DetectTool1.StateGesture.Left && hand2.current2 == DetectTool1.StateGesture.Null && hand2.current3 == DetectTool1.StateGesture.Null && hand2.current4 == DetectTool1.StateGesture.Null)) {

//				if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
//					carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeLeft ();
//					SwipeGesture = false;
//					
//					StartCoroutine (reset ());
//					//return;
//				}
				if (input == StateDO.Null || input == StateDO.SwipeLeft) {


					if (!MultiCarouselsController.ObjectScene) {
						input = StateDO.SwipeLeft;
					}
				}


			} else if ((hand1.current1 == DetectTool.StateGesture.Right && hand1.current2 == DetectTool.StateGesture.Null && hand1.current3 == DetectTool.StateGesture.Null && hand1.current4 == DetectTool.StateGesture.Null) ||
			           (hand2.current1 == DetectTool1.StateGesture.Right && hand2.current2 == DetectTool1.StateGesture.Null && hand2.current3 == DetectTool1.StateGesture.Null && hand2.current4 == DetectTool1.StateGesture.Null)) {

//				if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
//					carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeRight ();
//					SwipeGesture = false;
//					StartCoroutine (reset ());
//				}

				if (input == StateDO.Null || input == StateDO.SwipeRight) {

					if (!MultiCarouselsController.ObjectScene) {
						input = StateDO.SwipeRight;
					}
				}

			} else if ((hand1.current1 == DetectTool.StateGesture.Down && hand1.current2 == DetectTool.StateGesture.Null && hand1.current3 == DetectTool.StateGesture.Null && hand1.current4 == DetectTool.StateGesture.Null) ||
			           (hand2.current1 == DetectTool1.StateGesture.Down && hand2.current2 == DetectTool1.StateGesture.Null && hand2.current3 == DetectTool1.StateGesture.Null && hand2.current4 == DetectTool1.StateGesture.Null)) {

				if (input == StateDO.Null || input == StateDO.SwipeDown) {

					//input = StateDO.SwipeDown;
				}


			} else if ((hand1.current1 == DetectTool.StateGesture.Forward && hand1.current2 == DetectTool.StateGesture.Null && hand1.current3 == DetectTool.StateGesture.Null && hand1.current4 == DetectTool.StateGesture.Null) ||
			           (hand2.current1 == DetectTool1.StateGesture.Forward && hand2.current2 == DetectTool1.StateGesture.Null && hand2.current3 == DetectTool1.StateGesture.Null && hand2.current4 == DetectTool1.StateGesture.Null)) {

//				if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
//					carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeUp ();
//				}


			}else if ((hand1.current1 == DetectTool.StateGesture.Up && hand1.current2 == DetectTool.StateGesture.Null && hand1.current3 == DetectTool.StateGesture.Null && hand1.current4 == DetectTool.StateGesture.Null) ||
				(hand2.current1 == DetectTool1.StateGesture.Up && hand2.current2 == DetectTool1.StateGesture.Null && hand2.current3 == DetectTool1.StateGesture.Null && hand2.current4 == DetectTool1.StateGesture.Null)) {

				//				if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
				//					carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeUp ();
				//				}

				if (input == StateDO.Null || input == StateDO.SwipeUp) {

					//input = StateDO.SwipeUp;
				}

			}








			//Swipe End
		} else {
			
			hand1.current1 = DetectTool.StateGesture.Null;
			hand1.current2 = DetectTool.StateGesture.Null;
			hand1.current3 = DetectTool.StateGesture.Null;
			hand1.current4 = DetectTool.StateGesture.Null;

			hand2.current1 = DetectTool1.StateGesture.Null;
			hand2.current2 = DetectTool1.StateGesture.Null;
			hand2.current3 = DetectTool1.StateGesture.Null;
			hand2.current4 = DetectTool1.StateGesture.Null;
		}





	
	}

	public void ZoomGesture (float speed)
	{
		Vector3 forward = cam.transform.forward;
		character.transform.Translate (forward * Time.deltaTime * speed);
	

	}



	IEnumerator SwipeUp ()
	{
		yield return new WaitForSeconds (1f);

		carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeUp ();


	}


	IEnumerator reset()
	{
		yield return new WaitForSeconds (cooldown);
		SwipeGesture = true;
		input = StateDO.Null;
	}
	//


	public enum StateDO {SwipeUp,SwipeDown,SwipeLeft,SwipeRight,Select,Back,Exit, Null }



	public void SwipeUpState()
	{

		if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {

			StartCoroutine (SwipeUp ());
			SwipeGesture = false;
			StartCoroutine (reset ());
		}



	}

	public void SwipeLeftState()
	{


		if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
			carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeLeft ();
			SwipeGesture = false;

			StartCoroutine (reset ());
			//return;
		}


	}

	public void SwipeRightState()

	{

		if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
			carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeRight ();
			SwipeGesture = false;
			StartCoroutine (reset ());
		}


	}

	public void SelectMenu()
	{
		
		if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
			carouselobj [0].GetComponent<MultiCarouselsController> ().SelectButton ();
			SwipeGesture = false;
			StartCoroutine (reset ());
		}



	}


	public void SwipeDownState()

	{

		if (carouselobj [0] != null || carouselobj [0].activeInHierarchy) {
			carouselobj [0].GetComponent<MultiCarouselsController> ().SwipeDown ();
			SwipeGesture = false;
			StartCoroutine (reset ());
		}


	}


	IEnumerator exit()
	{

		yield return new WaitForSeconds (5f);

		Application.Quit ();
		Debug.Log ("Exit");

	}

	public void QuitApp()
	{

		StartCoroutine (exit ());

	}
}



