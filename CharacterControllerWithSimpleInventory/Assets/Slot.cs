using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private bool isFilled = false;
    private Collectible currentCollectible;
    [SerializeField] private Image slotHolderSR;
    [SerializeField] private Image heldObjectSR;
    [SerializeField] private Sprite emptySprite;

    [SerializeField] private Color32 nonSelectedColour;
    [SerializeField] private Color32 selectedColour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsSelected(bool isSelected)
    {
        slotHolderSR.color = isSelected ? selectedColour : nonSelectedColour;
    }

    private void SetIsFilled(bool hasObject)
    {
        isFilled = hasObject;

        SetSlotImage();
    }

    public bool GetIsFilled()
    {
        return isFilled;
    }

    public void SetCollectible(Collectible newCollectible)
    {
        currentCollectible = newCollectible;

        SetIsFilled(true);
    }

    public void RemoveCollectible()
    {
        currentCollectible = null;

        SetIsFilled(false);
    }

    private void SetSlotImage()
    {
        heldObjectSR.sprite = isFilled ? currentCollectible.GetSprite() : emptySprite;
        heldObjectSR.enabled = isFilled;
    }
}
