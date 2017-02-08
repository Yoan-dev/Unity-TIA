using UnityEngine;

public class GraspableObject : HighlightedObject, IGraspableObject, IPuzzleObject
{
    public Transform originalParent;
    private GameObject infobulle;
    private IPuzzleManager manager;
    private ICameraController controller;
    private ParticleSystem highlight;
    private bool grabbed;
    private bool completed;

    void Update()
    {
        if (highlighted && !completed)
        {
            highlight.Play();
        }
        else
        {
            highlight.Stop();
        }
        if (completed)
        {
            if (highlighted) infobulle.SetActive(true);
            else infobulle.SetActive(false);
            highlighted = false;
        }
        highlighted = false;
    }

    public void Grab(Transform newParent)
    {
        grabbed = true;
        transform.parent = newParent;
        transform.localPosition = new Vector3(0, 0, 1.2f);
    }

    public void UnGrab()
    {
        grabbed = false;
        transform.parent = originalParent;
        controller.Ungrab();
    }

    public void SetOriginalParent(Transform transform)
    {
        originalParent = transform;
    }

    public void SetManager(IPuzzleManager manager)
    {
        this.manager = manager;
    }

    public void SetInfobulle(GameObject infobulle)
    {
        this.infobulle = infobulle;
    }

    public void Complete()
    {
        completed = true;
        UnGrab();
        manager.VerifyPuzzle();
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public void SetController(ICameraController controller)
    {
        this.controller = controller;
    }

    public void SetHighlight(ParticleSystem highlight)
    {
        this.highlight = highlight;
    }
}
