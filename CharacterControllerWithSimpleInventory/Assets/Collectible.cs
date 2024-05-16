using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Collider2D collider;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private bool currentlyBeingHeld = false;
    private bool isPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetCurrentlyBeingHeld(bool isHeld)
    {
        currentlyBeingHeld = isHeld;
        spriteRenderer.enabled = isHeld && isPickedUp;
    }

    public void PickedUp(Transform heldPosition)
    {
        isPickedUp = true;
        rigidbody.isKinematic = true;
        transform.position = heldPosition.position;
        transform.SetParent(heldPosition);
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.gravityScale = 0;
        collider.enabled = false;
    }

    public void Dropped()
    {
        isPickedUp = false;
        rigidbody.isKinematic = false;
        SetCurrentlyBeingHeld(false);
        transform.SetParent(null);
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.gravityScale = 1;
        collider.enabled = true;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
