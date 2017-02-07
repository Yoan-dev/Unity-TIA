using UnityEngine;

public class HighlightedObject : MonoBehaviour, IHighlightedObject {

    protected bool highlighted = false;

    public void Highlight()
    {
        highlighted = true;
    }
}
