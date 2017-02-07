using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	void Update () 
    {
		transform.Rotate(new Vector3(transform.rotation.x + 10, transform.rotation.y, transform.rotation.z)) ;
	}
}
