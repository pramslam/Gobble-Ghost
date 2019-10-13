using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaniFX : MonoBehaviour
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
        if (Input.GetKeyDown("1"))
        {
            Show();
        }
        gameObject.transform.rotation = Quaternion.identity;
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
