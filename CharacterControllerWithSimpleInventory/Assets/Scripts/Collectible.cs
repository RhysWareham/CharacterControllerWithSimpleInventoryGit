using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Collider2D myCollider;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private bool currentlyBeingHeld = false;
    private bool isPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetCurrentlyBeingHeld(bool isHeld)
    {
        currentlyBeingHeld = isHeld;
        spriteRenderer.enabled = isHeld && isPickedUp;
    }

    public void PickedUp(Transform heldPosition)
    {
        isPickedUp = true;
        rb2d.isKinematic = true;
        transform.position = heldPosition.position;
        transform.SetParent(heldPosition);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2d.gravityScale = 0;
        myCollider.enabled = false;
    }

    public void Dropped()
    {
        isPickedUp = false;
        currentlyBeingHeld = false;
        rb2d.isKinematic = false;
        transform.SetParent(null);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2d.gravityScale = 1;
        myCollider.enabled = true;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>())
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
