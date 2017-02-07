using UnityEngine;

public class GraspablePart : MonoBehaviour, IGraspableObject
{
    private IGraspableObject parent;

    public void Grab(Transform newParent)
    {
        parent.Grab(newParent);
    }

    public void UnGrab()
    {
        parent.UnGrab();
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
