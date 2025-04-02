using UnityEngine;
using System.Collections;

public class SetPosOri : MonoBehaviour {
	Vector3 oripos;
	Quaternion Orirot;
	Vector3 scale;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{

	

		oripos = new Vector3 (transform.position.x, transform.position.y- ((transform.position.y*2f)/2.5f), transform.position.z);

		transform.position = oripos;

		Orirot = transform.rotation;


	}

	void OnDisable()
	{

		transform.position = oripos;
		transform.rotation = Orirot;

	}
}
