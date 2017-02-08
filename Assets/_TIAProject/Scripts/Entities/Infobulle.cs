using UnityEngine;

public class Infobulle : MonoBehaviour
{
    private Transform arcamera;

	void Start ()
    {
        arcamera = GameObject.Find("CustomARCamera").transform;
	}
	
	void Update ()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - arcamera.position);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, arcamera.eulerAngles.z);
    }
}
