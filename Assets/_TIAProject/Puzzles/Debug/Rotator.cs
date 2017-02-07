using UnityEngine;

public class Rotator : MonoBehaviour {

    public bool x;
    public bool y;
    public bool z;

    void Update () 
    {
		transform.Rotate(new Vector3(transform.rotation.x + ((x)? 10 : 0), transform.rotation.y + ((y) ? 10 : 0), transform.rotation.z + ((z) ? 10 : 0))) ;
	}
}
