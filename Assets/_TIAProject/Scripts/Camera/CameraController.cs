using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController
{
    public bool target; // does the camera aim at something (for debug)
    public bool changeLight = false; // does the light button is pressed
    public new GameObject light; // the directional light
    private IGraspableObject current; // the current grabbed puzzle piece

    void Update()
    {
        if (changeLight) ChangeLight(); // light button pressed

        target = false;

        // ungrab the current puzzle piece by clicking
        if (current != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                current.UnGrab();
            }
        }

        // else we can grab a puzzle piece by aiming at it and clicking
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
                        // if we got a target, we can grab it if it's a non-completed piece
                        IGraspableObject temp = hit.collider.GetComponent<IGraspableObject>();
                        if (!temp.IsCompleted())
                        {
                            current = temp;
                            current.Grab(transform);
                        }
                    }
                }
            }
        }
    }

    // pressing the light button allow to change the light
    // regarding the orientation of the ARCamera
    private void ChangeLight()
    {
        light.transform.eulerAngles = transform.eulerAngles;
    }

    // called by the light button (EventTrigger)
    public void LightOn()
    {
        changeLight = true;
    }

    // called by the light button (EventTrigger)
    public void LightOff()
    {
        changeLight = false;
    }

    // ungrab the current object
    // either called in update or by the grabbed object
    public void Ungrab()
    {
        current = null;
    }
}
