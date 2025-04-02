using UnityEngine;
using System.Collections;
using Sidema.CarouselPro;
public class Checktext : MonoBehaviour {
	public  GameObject[] obj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var activeCarousel = FindObjectOfType<Carousel>();
		for (int i = 0; i < obj.Length; i++) {

			if (i == activeCarousel.selectedSlotIndex) {

				obj [activeCarousel.selectedSlotIndex].SetActive (true);

			} else {
				obj [i].SetActive (false);


			}


		}
	}
}
