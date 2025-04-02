using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap.Unity;

	public class togglezoom : MonoBehaviour {
		public Text text;
		public UnityEngine.UI.Image image;
		public Color OnColor;
		public Color OffColor;
	public GameObject map;
		//public Multihandgesture m1;
	public Toggle toggle;


	void Update()
	{
		if (toggle.isOn) {
			text.text = "Earth : On";
			text.color = new Color (105f, 218f, 255f);
			map.SetActive (true);
//			m1.zoom = true;

			//print ("Zoom :" + m1.zoom);
		} else {
			text.text = "Earth :Off";
			text.color = new Color(255f, 0f, 37f);
			map.SetActive (false);
//			m1.zoom = false;
			//print ("Zoom :" + m1.zoom);
		}



	}



//		public void SetToggle(Toggle toggle) {
//			
//		}
	}
