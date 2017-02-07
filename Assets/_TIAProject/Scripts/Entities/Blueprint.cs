using UnityEngine;

public class Blueprint : HighlightedObject, IBlueprint {

    private ICameraController controller;
    private IPuzzleManager manager;
    private GameObject graspable;
    private GameObject infobulle;
    private bool considerRotation;
    private bool completed;

    void Start()
    {
        //foreach (MeshRenderer current in GetComponentsInChildren<MeshRenderer>())
            //current.material.color = Color.blue;
    }
	
	void LateUpdate () 
    {
        if (graspable != null && Vector3.Distance(transform.position, graspable.transform.position) < 0.020f && (!considerRotation || Vector3.Angle(transform.up, graspable.transform.up) < 5.0f))
            AutoComplete();

        if (completed)
        {
            if (highlighted) Debug.Log(name + " highlighted");
            if (highlighted) infobulle.SetActive(true);
            else infobulle.SetActive(false);
            highlighted = false;
            manager.VerifyPuzzle();
        }
	}

    public void SetGraspable(GameObject graspable)
    {
        this.graspable = graspable;
    }

    public void SetInfobulle(GameObject infobulle)
    {
        this.infobulle = infobulle;
    }

    public void SetCamera(ICameraController camera)
    {
        this.controller = camera;
    }

    public void SetManager(IPuzzleManager manager)
    {
        this.manager = manager;
    }

    public void SetConsiderRotation(bool considerRotation)
    {
        this.considerRotation = considerRotation;
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public void AutoComplete()
    {
        foreach (MeshRenderer current in GetComponentsInChildren<MeshRenderer>())
            current.material.color = Color.green;
        controller.Ungrab();
        Destroy(graspable);
        completed = true;
    }
}
