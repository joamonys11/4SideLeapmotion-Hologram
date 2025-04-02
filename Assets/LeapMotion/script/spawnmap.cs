using UnityEngine;
using System.Collections;

public class spawnmap : MonoBehaviour {
	public GameObject[] item;
	public int itemindex;
	// Use this for initialization
	void Start () {

	
	
	}
	
	// Update is called once per frame
	void Update () {



//		mapar = GameObject.FindGameObjectsWithTag ("map");
		//float distance = Vector3.Distance (transform.position, map.transform.position);	
		//print ("Dis : " + distance);



}
	public void createmap()
	{

	

		Instantiate (item[itemindex], transform.position, transform.rotation);
//			if(mapar.Length < 1 )
//			{
//
//			Instantiate (map, transform.position, Quaternion.identity);
//
//			}

	}

	public void itemset (int num)
	{
		itemindex = num;
		Instantiate (item[itemindex], transform.position, transform.rotation);

	}

}