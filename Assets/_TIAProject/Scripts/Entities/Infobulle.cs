using UnityEngine;

public class Infobulle : MonoBehaviour
{
    private Transform arcamera; // the arcamera transform for orientation

	void Start ()
    {
        arcamera = GameObject.Find("CustomARCamera").transform;
	}
	
	void Update ()
    {
        // when activated, the infobulle will "look at" the camera
        if (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - arcamera.position);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, arcamera.eulerAngles.z);
        }
    }
}
