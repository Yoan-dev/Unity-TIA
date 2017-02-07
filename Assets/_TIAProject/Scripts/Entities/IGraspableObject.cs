using UnityEngine;

public interface IGraspableObject {

    void Grab(Transform newParent);
    void UnGrab();
    void SetOriginalParent(Transform transform);

}
