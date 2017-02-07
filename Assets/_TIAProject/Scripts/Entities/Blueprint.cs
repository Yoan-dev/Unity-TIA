using UnityEngine;

public class Blueprint : HighlightedObject, IBlueprint
{
    private GameObject graspable;
    private bool considerRotation;

    void Update()
    {
        if (graspable != null && Vector3.Distance(transform.position, graspable.transform.position) < 0.040f && (!considerRotation || Vector3.Angle(transform.up, graspable.transform.up) < 10.0f))
            AutoComplete();
    }

    public void SetGraspable(GameObject graspable)
    {
        this.graspable = graspable;
    }

    public void SetConsiderRotation(bool considerRotation)
    {
        this.considerRotation = considerRotation;
    }

    public void AutoComplete()
    {
        graspable.GetComponent<IPuzzleObject>().Complete();
        graspable.transform.position = transform.position;
        graspable.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }
}
