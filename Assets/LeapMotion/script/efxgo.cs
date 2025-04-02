using UnityEngine;
using System.Collections;
using Leap;
public class efxgo : MonoBehaviour {
	public GameObject efx;
	public Transform camCenter;
	public Transform objectbase;
//	public static Vector3 startpost;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



	}

	public void Startgo()
	{
		
		GameObject go = Instantiate (efx) as GameObject;
		go.transform.position = objectbase.transform.position;
		Vector3 d = (camCenter.position - objectbase.transform.position).normalized;
		move m = go.GetComponent<move> ();
		m.forward = d;
	}


}
