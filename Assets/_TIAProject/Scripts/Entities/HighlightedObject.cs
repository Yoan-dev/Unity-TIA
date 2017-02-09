using UnityEngine;

public class HighlightedObject : MonoBehaviour, IHighlightedObject
{
    protected bool highlighted = false; // protected to be accessed in the implemented class (for a custom behaviour)

    // called when aimed by the ARCamera
    public void Highlight()
    {
        highlighted = true;
    }
}
