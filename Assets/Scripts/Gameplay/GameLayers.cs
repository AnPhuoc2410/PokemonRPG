using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask fovLayer;


    public static GameLayers i { get; private set; }
    private void Awake()
    {
        i = this;
    }
    public LayerMask SolidObjectsLayer => solidObjectsLayer;
    public LayerMask GrassLayer => grassLayer;
    public LayerMask InteractableLayer => interactableLayer;
    public LayerMask PlayerLayer => playerLayer;
    public LayerMask FovLayer => fovLayer;

}
