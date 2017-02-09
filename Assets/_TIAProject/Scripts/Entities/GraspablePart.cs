using UnityEngine;

public class GraspablePart : MonoBehaviour, IGraspableObject
{
    private IGraspableObject parent; // the graspable object this object is a child of (or grand child, etc.)

    /* For each feature of graspable object
     * we relay the action to the parent */

    public void Grab(Transform newParent)
    {
        parent.Grab(newParent);
    }

    public void UnGrab()
    {
        parent.UnGrab();
    }

    public bool IsCompleted()
    {
        return parent.IsCompleted();
    }

    public void SetParent(IGraspableObject parent)
    {
        this.parent = parent;
    }

    public void SetOriginalParent(Transform transform)
    {
        // not used
    }
}
