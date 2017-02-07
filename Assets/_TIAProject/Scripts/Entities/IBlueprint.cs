using UnityEngine;

public interface IBlueprint {

    void SetGraspable(GameObject graspable);
    void SetInfobulle(GameObject infobulle);
    void SetCamera(ICameraController controller);
    void SetManager(IPuzzleManager manager);
    void SetConsiderRotation(bool considerRotation);
    bool IsCompleted();
}
