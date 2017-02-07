﻿using UnityEngine;

public interface IPuzzleObject
{
    void SetManager(IPuzzleManager manager);
    void SetController(ICameraController controller);
    void SetInfobulle(GameObject infobulle);
    void Complete();
}
