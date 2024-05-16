using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrab : MonoBehaviour
{
    private PlayerInput playerInput;
    private InventoryManager inventoryManager;
    private Collectible heldObject;
    [SerializeField] private Transform holdingPosition;
    private FixedJoint2D fixedJoint;

    private InputAction pickUpAction;

    [SerializeField] private float distanceToPickup = 1f;
    [SerializeField] private LayerMask collectibleMask;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        pickUpAction = playerInput.actions["PickUp"];
    }

    protected virtual void OnEnable()
    {
        pickUpAction.started += PickUpAction_started;
    }

    protected virtual void OnDisable()
    {
        pickUpAction.started -= PickUpAction_started;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        fixedJoint = GetComponent<FixedJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetHeldObject(Collectible collectible)
    {
        if (heldObject != null)
        {
            heldObject.SetCurrentlyBeingHeld(false);
            heldObject = null;
        }

        heldObject = collectible;
        heldObject.SetCurrentlyBeingHeld(true);
    }

    private void PickUpAction_started(InputAction.CallbackContext obj)
    {
        Physics2D.queriesStartInColliders = false;
        Collider2D hit = Physics2D.OverlapBox(holdingPosition.position, new Vector2(distanceToPickup, 0.1f), 0, collectibleMask);

        if (hit != null)
        {
            Collectible collectible = hit.GetComponent<Collectible>();
            if (inventoryManager.TryToStore(collectible))
            {
                collectible.PickedUp(holdingPosition);
                SetHeldObject(collectible);
            }
            else
            {
                //Trigger inventory full message
            }
        }
    }
}
