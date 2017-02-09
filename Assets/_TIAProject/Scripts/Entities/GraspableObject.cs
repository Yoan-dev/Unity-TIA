using UnityEngine;

public class GraspableObject : HighlightedObject, IGraspableObject, IPuzzleObject
{
    public Transform originalParent; // the image target
    private GameObject infobulle; // the infobulle corresponding to this puzzle piece
    private IPuzzleManager manager; // the puzzle manager
    private ICameraController controller; // the ARCamera
    private ParticleSystem highlight; // for when we aim at this puzzle piece
    private bool grabbed; // is this piece grabbed or not by the ARCamera
    private bool completed; // is this puzzle piece completed (putted on the blueprint)

    void Update()
    {
        if (highlighted && !completed)
        {
            highlight.Play(); // particles on
        }
        else
        {
            highlight.Stop(); // particles off
        }
        if (completed)
        {
            if (highlighted) infobulle.SetActive(true); // infobulle on
            else infobulle.SetActive(false); // infobulle off
        }
        highlighted = false; // reboot
    }

    // called by the camera
    // allow to grab this puzzle piece and replace it consequently
    public void Grab(Transform newParent)
    {
        grabbed = true;
        transform.parent = newParent;
        transform.localPosition = new Vector3(0, 0, 1.2f);
    }

    // allow to ungrab this puzzle piece
    public void UnGrab()
    {
        grabbed = false;
        transform.parent = originalParent;
        controller.Ungrab();
    }

    // initialization
    public void SetOriginalParent(Transform transform)
    {
        originalParent = transform;
    }

    // initialization
    public void SetManager(IPuzzleManager manager)
    {
        this.manager = manager;
    }

    // initialization
    public void SetInfobulle(GameObject infobulle)
    {
        this.infobulle = infobulle;
    }

    // called when this puzzle piece is placed
    // on the corresponding blueprint
    public void Complete()
    {
        completed = true;
        UnGrab();
        manager.VerifyPuzzle();
    }

    // is this puzzle piece completed or not ?
    public bool IsCompleted()
    {
        return completed;
    }

    // initialization
    public void SetController(ICameraController controller)
    {
        this.controller = controller;
    }

    // initialization
    public void SetHighlight(ParticleSystem highlight)
    {
        this.highlight = highlight;
    }
}
