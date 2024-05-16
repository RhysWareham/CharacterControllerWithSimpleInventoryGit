using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabAndDrop : MonoBehaviour
{
    private PlayerInput playerInput;
    private InventoryManager inventoryManager;
    private Collectible heldObject;

    private InputAction pickUpAction;
    private InputAction dropAction;
    private InputAction selectItemSlotAction;

    [SerializeField] private Transform holdingPosition;
    [SerializeField] private float distanceToPickup = 1f;
    [SerializeField] private LayerMask collectibleMask;

    [SerializeField] private float timeToDrop = 0.5f;
    private bool isTryingToDrop = false;
    private Coroutine tryToDropCoroutine;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        pickUpAction = playerInput.actions["PickUp"];
        dropAction = playerInput.actions["Drop"];
        selectItemSlotAction = playerInput.actions["SelectItem"];
    }

    protected virtual void OnEnable()
    {
        pickUpAction.started += PickUpAction_started;
        selectItemSlotAction.started += SelectItemSlotAction_started;
        dropAction.started += DropAction_started;
        dropAction.canceled += DropAction_canceled;
    }

    protected virtual void OnDisable()
    {
        pickUpAction.started -= PickUpAction_started;
        selectItemSlotAction.started -= SelectItemSlotAction_started;
        dropAction.started -= DropAction_started;
        dropAction.canceled -= DropAction_canceled;
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    private void SetHeldObject(Collectible collectible)
    {
        if (heldObject != null)
        {
            heldObject.SetCurrentlyBeingHeld(false);
            heldObject = null;
        }

        if (collectible != null)
        {
            heldObject = collectible;
            heldObject.SetCurrentlyBeingHeld(true);
        }
    }

    private void DropObject()
    {
        if (inventoryManager.TryToDrop())
        {
            heldObject.Dropped();
            heldObject = null;
        }
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
        }
    }

    private void DropAction_started(InputAction.CallbackContext obj)
    {
        if (heldObject != null)
        {
            isTryingToDrop = true;
            tryToDropCoroutine = StartCoroutine(DropItem());
        }
    }

    private void DropAction_canceled(InputAction.CallbackContext obj)
    {
        isTryingToDrop = false;

        if (tryToDropCoroutine != null)
        {
            StopCoroutine(tryToDropCoroutine);
            tryToDropCoroutine = null;
        }
    }

    private void SelectItemSlotAction_started(InputAction.CallbackContext obj)
    {
        if (!isTryingToDrop)
        {
            float keyNumber = obj.ReadValue<float>();
            inventoryManager.SetSelectedSlot((int)keyNumber - 1);
            SetHeldObject(inventoryManager.GetSelectedCollectible());
        }
    }

    private IEnumerator DropItem()
    {
        yield return new WaitForSeconds(timeToDrop);

        if (isTryingToDrop)
        {
            isTryingToDrop = false;
            DropObject();
        }
        
        tryToDropCoroutine = null;

        yield return null;
    }
}
