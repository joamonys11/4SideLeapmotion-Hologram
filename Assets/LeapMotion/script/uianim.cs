using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class uianim : MonoBehaviour {
	public  Image rotate_y;	// Use this for initialization
	public  Image rotate_z;	// Use this for initialization
	public  Image rotate_x;	// Use this for initialization
	public GameObject Gamerotate_x,Gamerotate_y,Gamerotate_z;
	public GameObject GamerotateSelf_x,GamerotateSelf_y,GamerotateSelf_z;
	public GameObject Rescale;
	public float speed;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate_y != null) {
			rotate_y.transform.Rotate (0, Time.deltaTime * speed, 0, Space.World);
		}

		if (rotate_x != null) {
			rotate_x.transform.Rotate (Time.deltaTime * speed,0 , 0, Space.World);
		}

		if (rotate_z != null) {
			rotate_z.transform.Rotate (0,0 ,Time.deltaTime * speed, Space.World);
		}

		if (Gamerotate_x != null) {

			Gamerotate_x.transform.Rotate (Time.deltaTime * speed, 0, 0, Space.World);
		}
		if (Gamerotate_y != null) {

			Gamerotate_y.transform.Rotate (0, Time.deltaTime * speed, 0, Space.World);
		}
		if (Gamerotate_z != null) {

			Gamerotate_z.transform.Rotate (0,0 ,Time.deltaTime * speed, Space.World);


		}

		if (GamerotateSelf_x != null) {

			GamerotateSelf_x.transform.Rotate (Time.deltaTime * speed, 0, 0, Space.Self);
		}
		if (GamerotateSelf_y != null) {

			GamerotateSelf_y.transform.Rotate (0, Time.deltaTime * speed, 0, Space.Self);
		}
		if (GamerotateSelf_z != null) {

			GamerotateSelf_z.transform.Rotate (0,0 ,Time.deltaTime * speed, Space.Self);


		}


	}
}
