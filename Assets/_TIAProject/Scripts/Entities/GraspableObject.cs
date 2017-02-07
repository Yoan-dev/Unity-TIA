using UnityEngine;

public class GraspableObject : HighlightedObject, IGraspableObject {

    public Transform originalParent;
    private bool grabbed;
	
	void Update ()
    {
        Color color;
        if (highlighted) color = Color.yellow;
        else if (grabbed) color = Color.red;
        else color = Color.white;
        foreach (MeshRenderer current in GetComponentsInChildren<MeshRenderer>())
            current.material.color = color;
        highlighted = false;
	}

    public void Grab(Transform newParent)
    {
        Debug.Log(name + " grabbed");
        grabbed = true;
        transform.parent = newParent;
        transform.localPosition = new Vector3(0, 0, 0.35f);
    }

    public void UnGrab()
    {
        Debug.Log(name + " ungrabbed");
        grabbed = false;
        transform.parent = originalParent;
    }

    public void SetOriginalParent(Transform transform)
    {
        originalParent = transform;
    }

}
