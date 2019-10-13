using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFX : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Public Functions
    #region
    public void DashShow()
    {
        spriteRenderer.enabled = true;
    }

    public void DashHide()
    {
        spriteRenderer.enabled = false;
    }
    #endregion
}
