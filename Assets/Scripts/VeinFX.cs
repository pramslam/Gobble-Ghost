using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinFX : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("3"))
        {
            Show();
        }

        // Flip the image effect when flipping player
        if (spriteRenderer.enabled == true)
        {
            //Get the mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);

            //Rotate the sprite to the mouse point
            Vector3 diff = mousePos - transform.position;
            diff.Normalize();

            // Flip sprite
            if (diff.x > 0.0f)
                spriteRenderer.flipX = false;
            else if (diff.x < 0.0f)
                spriteRenderer.flipX = true;
        }
    }

    // Public Functions
    #region
    public void Show()
    {
        spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }
    #endregion
}
