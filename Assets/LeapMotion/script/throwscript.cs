using UnityEngine;
using System.Collections;
using Leap.Unity;
public class throwscript : MonoBehaviour {
	Rigidbody rigid;
	public PinchDetector p1, p2;
	private PinchDetector l,r;
	private GameObject pinchleft, pinchright;


	// Use this for initialization


	void Awake()
	{
		
		pinchleft = GameObject.FindGameObjectWithTag("pinchleft");
		pinchright = GameObject.FindGameObjectWithTag ("pinchright");

		l = pinchleft.GetComponent<PinchDetector>();
		r = pinchright.GetComponent<PinchDetector>();


		p1 = l;
		p2 = r;


	}
	void Start () {
	
		rigid = GetComponent<Rigidbody> ();
		rigid.isKinematic = true;

	}
	
	// Update is called once per frame
	void Update () {

		float p1dis = Vector3.Distance (transform.position, p1.Position);
		float p2dis = Vector3.Distance (transform.position, p2.Position);


//		print (p1dis + ": P1dis");
//		print (p2dis + ": P2dis");

		if (p1.IsPinching || p2.IsPinching) {


			if (p1dis < 0.3 || p2dis < 0.3) {
				rigid.isKinematic = true;
			}
		} else
			rigid.isKinematic = false;
		

		//transform.Rotate(0,9f*Time.deltaTime,0);
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("junk")) 
		{
			Destroy (gameObject);



		}



	}



//	void OnTriggerStay(Collider other)
//	{
//
////		if (other.gameObject.name)
//		if (other.gameObject.tag == "palm_l" || other.gameObject.tag == "palm_r"  ) {
//
//			rigid.isKinematic = true;
//
//			print ("Keep");
//
//
//		}
//
//	}
//
//	void OnTriggerExit(Collider other)
//	{
//
//		//		if (other.gameObject.name)
//		if (other.gameObject.tag == "palm_l" || other.gameObject.tag == "palm_r"  ) {
//
//			rigid.isKinematic = false;
//			print ("Throw");
//
//		}
//
//	}
}
