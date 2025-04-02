using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {
	//public Transform reference;
	// Use this for initialization
	public Vector3 forward;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//Vector3 forward = transform.forward;

		transform.Translate (forward * Time.deltaTime * 20);




//		if(distance)

	Destroy (gameObject,2f);

	}
}
