using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController {

    public bool target;
    public bool changeLight = false;
    public new GameObject light;
    private IGraspableObject current;

    void Update()
    {
        if (changeLight) ChangeLight();

        target = false;
        if (current != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                current.UnGrab();
                Ungrab();
            }
        }
        else
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0F))
            {
                if (hit.collider.GetComponent<IHighlightedObject>() != null)
                    hit.collider.GetComponent<IHighlightedObject>().Highlight();
                if (hit.collider.GetComponent<IGraspableObject>() != null)
                {
                    target = true;
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("hit");
                        current = hit.collider.GetComponent<IGraspableObject>();
                        current.Grab(transform);
                    }
                }
            }
        }
    }

    private void ChangeLight()
    {
        light.transform.eulerAngles = transform.eulerAngles;
    }

    public void LightOn()
    {
        changeLight = true;
    }

    public void LightOff()
    {
        changeLight = false;
    }

    public void Ungrab()
    {
        current = null;
    }
}
