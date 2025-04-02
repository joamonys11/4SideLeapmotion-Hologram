using UnityEngine;
using Sidema.CarouselPro;
using System;
using System.Collections;
using Leap;
using Leap.Unity;
public class MultiCarouselsController : MonoBehaviour {
	public GameObject[] model3d;
    public Carousel[] carousels = new Carousel[0];
	bool menu;
	bool SwipeLefts =false;
	bool SwipeRights = false;
	bool SwipeDowns = false;
	bool Selects =false;
	bool SwipeUps = false;
	public static bool ObjectScene = false;
	int currentslot1;
	int currentslot2;
	public PinchDetector p1, p2;

	//public GameObject[] obj;

	//public MultiCarouselsController m1;
    int currentCarouselIndex = 0;
    bool changingCarousel = false;
   
    void Awake()
    {
        carousels = GetComponentsInChildren<Carousel>(true);
    }

	void Start ()
    {
        for (int i = 0; i < carousels.Length; i++)
        {
            carousels[i].Hide();
        }

        carousels[0].Show();
		menu = true;
	}

    void OnChangedCarousel()
    {
        changingCarousel = false;
    }
	
	void Update ()
    {
		#region Prevent Carousel lost
		if (menu) {

			GameObject[] go = GameObject.FindGameObjectsWithTag ("carousel");
			if (go.Length == 0) {

				currentCarouselIndex = 0;
				carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.DOWN, OnChangedCarousel);
				changingCarousel = true;

			}


		}

        float horizontalAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalAxis) > 0f)
        {
            if (horizontalAxis < 0f)
            {
                carousels[currentCarouselIndex].Previous(OnChangedCarousel);
            }
            else
            {
                carousels[currentCarouselIndex].Next(OnChangedCarousel);
            }
        }

        float verticalAxis = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalAxis) > 0f && !changingCarousel)
        {
            if (verticalAxis < 0f && currentCarouselIndex < carousels.Length - 1)
            {
                carousels[currentCarouselIndex].Hide(Carousel.VisibilityTransitionType.UP);
                currentCarouselIndex += 1;
                carousels[currentCarouselIndex].Show(Carousel.VisibilityTransitionType.UP, OnChangedCarousel);
                changingCarousel = true;
            }
            else if (verticalAxis > 0f && currentCarouselIndex > 0)
            {
                carousels[currentCarouselIndex].Hide(Carousel.VisibilityTransitionType.DOWN);
                currentCarouselIndex -= 1;
                carousels[currentCarouselIndex].Show(Carousel.VisibilityTransitionType.DOWN, OnChangedCarousel);
                changingCarousel = true;
            }
        }
		#endregion
		//Debug.Log ("Carousel Index :" + currentCarouselIndex);



		#region STATE
		if (currentCarouselIndex == 0) {


			SwipeLefts = false;
			SwipeRights = false;
			SwipeDowns = false;
			Selects = false;
			SwipeUps = true;
			currentslot1 = currentCarouselIndex;



		} else if (currentCarouselIndex == 1) {

			SwipeLefts = true;
			SwipeRights = true;
			SwipeDowns = true; //back
			Selects = true;
			SwipeUps = true;


			currentslot2 = currentCarouselIndex;
		}else if (ObjectScene)
			{
		if ((p1.IsPinching && p1.IsHolding) || (p2.IsPinching && p2.IsHolding)) {



			SwipeDowns = false;

		} else {

			SwipeDowns = true;



		}
				}
		#endregion

    }

	public void SwipeRight()
	{
		if (SwipeRights) {
			//StartCoroutine (waitforgesture (1.0f));
			carousels [currentCarouselIndex].Previous (OnChangedCarousel);
		}
	}

	public void SwipeLeft()
	{
		if (SwipeLefts) {
			//StartCoroutine (waitforgesture (1.0f));
			carousels [currentCarouselIndex].Next (OnChangedCarousel);
		}
	}

	public void SwipeUp()
	{
		if (SwipeUps) 
		{
			
//			if (currentCarouselIndex < carousels.Length - 1) {
//				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.UP);
//				currentCarouselIndex += 1;
//				carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.UP, OnChangedCarousel);
//				changingCarousel = true;
//
//
//			}


						if (currentCarouselIndex == 0) {
							carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.UP);
							currentCarouselIndex += 1;
							carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.UP, OnChangedCarousel);
							changingCarousel = true;
			
			
						}


		}
//		if (carousels [0].gameObject.activeInHierarchy) {
//
//			var activeCarousel = FindObjectOfType<Carousel>();
//			if (currentCarouselIndex == 0) {
//
//
//
//			}
//
//		}

	}

	public void SwipeDown()
	{

		if (SwipeDowns) {
			
			StartCoroutine (SwipeState (0.8f));
		}
	}


	public void SwipeDownState()
	{



		var activeCarousel = FindObjectOfType<Carousel>();
		//StartCoroutine (waitforgesture (1.0f));

		#region Check Object Still in scene

		if (menu) {
			
			if (currentCarouselIndex == 0) {
				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.DOWN);
				currentCarouselIndex = 0;
				carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.DOWN, OnChangedCarousel);
				changingCarousel = true;
			}else if (currentCarouselIndex == 1) {
				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.DOWN);
				currentCarouselIndex = 0;
				carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.DOWN, OnChangedCarousel);
				changingCarousel = true;
			}else if (currentCarouselIndex == 3) {
				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.DOWN);
				currentCarouselIndex = 1;
				carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.DOWN, OnChangedCarousel);
				changingCarousel = true;
			}




		} else {



			GameObject[] obj = GameObject.FindGameObjectsWithTag ("object");
			if (obj != null || obj.Length > 0) {

				foreach (var item in obj) {

					item.SetActive (false);
					if (!item.activeInHierarchy) {

						menu = true;
						ObjectScene = false;
					}

				}

			}

			#endregion
			//currentCarouselIndex = 1;
			carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.DOWN, OnChangedCarousel);
			changingCarousel = true;

			//menu = true;


		}

		//StopAllCoroutines ();


	}

	IEnumerator SwipeSelect(float time)
	{


		yield return new WaitForSeconds (time);
			Select();


	}

	public void SelectButton()
	{
			SwipeUp ();

		if (Selects) {

			StartCoroutine (SwipeSelect (0.8f));
		}
	}

	public void Select()
	{
		

		if (carousels [1].gameObject.activeInHierarchy) {

			var activeCarousel = FindObjectOfType<Carousel>();

			if (activeCarousel.selectedSlotIndex == 1) {

				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.UP);
				currentCarouselIndex = 3 ;
				carousels [currentCarouselIndex].Show (Carousel.VisibilityTransitionType.UP, OnChangedCarousel);
				changingCarousel = true;

		//TODO change atricle

//				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.UP);
//				//carousels [1].SetSelection (-1);
//				model3d [0].SetActive (true);
//				ObjectScene = true;
//				//handcontrol = false;
//				menu = false;	
//				//menu = false;

			} else if (activeCarousel.selectedSlotIndex == 2) {


				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.UP);
				//carousels [1].SetSelection (-1);
				ObjectScene = true;
				model3d [1].SetActive (true);
				//handcontrol = false;
				menu = false;

			}else if (activeCarousel.selectedSlotIndex == 0) {


				carousels [currentCarouselIndex].Hide (Carousel.VisibilityTransitionType.UP);
				//carousels [1].SetSelection (-1);
				ObjectScene = true;
				model3d [1].SetActive (true);
				//handcontrol = false;
				menu = false;

			}


		}



	}


	IEnumerator SwipeState(float time)
	{
		yield return new WaitForSeconds (time);
		SwipeDownState ();


	}




}

