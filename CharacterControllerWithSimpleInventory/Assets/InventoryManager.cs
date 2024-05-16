using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Collectible[] inventoryCollectibles = new Collectible[6];
    [SerializeField] private Slot[] inventorySlots = new Slot[6];
    private int selectedIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool TryToStore(Collectible newCollectible)
    {
        if (inventorySlots[selectedIndex].GetIsFilled())
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (!inventorySlots[i].GetIsFilled())
                {
                    inventorySlots[i].SetCollectible(newCollectible);
                    SetSelectedSlot(i);
                    return true;
                }
            }
        }
        else
        {
            inventorySlots[selectedIndex].SetCollectible(newCollectible);
            SetSelectedSlot(selectedIndex);
            return true;
        }

        return false; //Show inventory full message
    }

    public void SetSelectedSlot(int index)
    {
        inventorySlots[selectedIndex].SetIsSelected(false);
        selectedIndex = index;
        inventorySlots[selectedIndex].SetIsSelected(true);
    }

    public bool TryToDrop()
    {
        if (inventorySlots[selectedIndex].GetIsFilled())
        {
            inventorySlots[selectedIndex].RemoveCollectible();
            return true;
        }

        return false;
    }
}
