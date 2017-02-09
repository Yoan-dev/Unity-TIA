using UnityEngine;

public class HighlightedPart : MonoBehaviour, IHighlightedObject
{
    private IHighlightedObject parent; // the highlighted object this object is a child of (or grand child, etc.)

    /* For each feature of highlighted object
     * we relay the action to the parent */

    public void Highlight()
    {
        parent.Highlight();
    }

    public void SetParent(IHighlightedObject parent)
    {
        this.parent = parent;
    }
}
