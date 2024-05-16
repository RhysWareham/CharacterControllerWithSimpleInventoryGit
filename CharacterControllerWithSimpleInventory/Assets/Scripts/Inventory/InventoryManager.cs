using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Slot[] inventorySlots = new Slot[6];
    private int selectedIndex = 0;
    [SerializeField] private PopupText inventoryFullPopup;

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

        inventoryFullPopup.ShowPopup();
        return false;
    }

    public void SetSelectedSlot(int index)
    {
        inventorySlots[selectedIndex].SetIsSelected(false);
        selectedIndex = index;
        inventorySlots[selectedIndex].SetIsSelected(true);
    }

    public Collectible GetSelectedCollectible()
    {
        return inventorySlots[selectedIndex].GetCollectible();
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
