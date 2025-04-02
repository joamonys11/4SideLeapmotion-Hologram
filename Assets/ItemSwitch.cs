using UnityEngine;
using System.Collections;

public class ItemSwitch : MonoBehaviour {
	public GameObject[] item;
	public static int checki;
	public bool plusto;
	public bool minute;

	public DetectTool hand1;
	public DetectTool1 hand2;
	// Use this for initialization
	void Start () {
		
		checki = 0;
		Scroll ();


	}
	
	// Update is called once per frame
	void Update () {
	


	}

	public void Scroll()
	{
		//checki = Random.Range (0, item.Length);
	
			for (int i = 0; i < item.Length; i++) {


				if (i == checki) {

					item [i].SetActive (true);


				} else {

					item [i].SetActive (false);
				}
				
				
			}






	}


	public void SwipeLeft()	
	{
		minute = true;
		//checki--;
		//int checkiori = checki;

		if (minute) {

			if (checki > 0) {

				StartCoroutine (minutewait ());
			
				minute = false;

			} else {

			

			}


		}


	}

	public void SwipeRight()
	{
		//plusto = true;
	//	int checkiori = checki;

			plusto = true;

		if (plusto) {

			if (checki < item.Length-1) {
				StartCoroutine (pluswait ());
				//item [checki].SetActive (true);

				plusto = false;
			} else {




			}

		} 
	

	}

	IEnumerator minutewait()
	{
		yield return new WaitForSeconds (0.5f);
		hand1.current1 = DetectTool.StateGesture.Null;
		hand2.current1 = DetectTool1.StateGesture.Null;
		checki--;
		Debug.Log ("I : " + checki);
		Scroll ();
		StopAllCoroutines ();

	}

	IEnumerator pluswait()
	{
		yield return new WaitForSeconds (0.5f);
		hand1.current1 = DetectTool.StateGesture.Null;
		hand2.current1 = DetectTool1.StateGesture.Null;
		checki++;
		Debug.Log ("I : " + checki);
		Scroll ();

		StopAllCoroutines ();
	}

}
