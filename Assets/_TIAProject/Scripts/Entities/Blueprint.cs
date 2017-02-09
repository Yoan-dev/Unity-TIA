using UnityEngine;

public class Blueprint : HighlightedObject, IBlueprint
{
    private GameObject graspable; // the graspable puzzle piece this blueprint correspond to
    private bool considerRotation; // do we consider rotation for this blueprint? (example: false for spherical objects)

    void Update()
    {
        // if the graspable object is close to the blueprint (position and rotation)
        // we consider that the blueprint is compelted
        if (graspable != null && Vector3.Distance(transform.position, graspable.transform.position) < 0.040f && (!considerRotation || Vector3.Angle(transform.up, graspable.transform.up) < 10.0f))
            AutoComplete();
    }

    // initialization
    public void SetGraspable(GameObject graspable)
    {
        this.graspable = graspable;
    }

    // initialization
    public void SetConsiderRotation(bool considerRotation)
    {
        this.considerRotation = considerRotation;
    }

    // once the blueprint is completed
    // we modify the graspable then destroy the blueprint
    public void AutoComplete()
    {
        graspable.GetComponent<IPuzzleObject>().Complete();
        graspable.transform.position = transform.position;
        graspable.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }
}
