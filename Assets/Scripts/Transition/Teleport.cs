using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SceneName] public string SceneFrom;
    [SceneName] public string SceneToGO;

    public void TeleportToScene()
    {
        transitionManager.Instance.Transition(SceneFrom,SceneToGO);
    }
}
