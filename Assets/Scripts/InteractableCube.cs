using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.Rendering;

public class InteractableCube : Interactable
{
    public override void Interact()
    {
        Debug.Log("Cube interaction");
    }
}
