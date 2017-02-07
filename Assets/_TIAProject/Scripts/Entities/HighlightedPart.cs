using UnityEngine;

public class HighlightedPart : MonoBehaviour, IHighlightedObject
{
    private IHighlightedObject parent;

    public void Highlight()
    {
        parent.Highlight();
    }

    public void SetParent(IHighlightedObject parent)
    {
        this.parent = parent;
    }
}
