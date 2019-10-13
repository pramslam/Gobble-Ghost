using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    public Image CooldownImage;
    private int initialCooldownHeight;
    private RectTransform rectTransform;

    public void Start()
    {
        if (CooldownImage == null) Debug.LogError("AbilityUIController: Cooldown Image on " + gameObject.name + " is null! Did you forget to set it?");
        CooldownImage.enabled = false;
        rectTransform = CooldownImage.GetComponent<RectTransform>();
        initialCooldownHeight = (int) rectTransform.sizeDelta.y;
    }

    public void BeginCooldownAnimation(float cooldownTime)
    {
        CooldownImage.enabled = true;
        StartCoroutine(AnimateCooldown(Time.time, cooldownTime));
    }

    IEnumerator AnimateCooldown(float initialTime, float cooldownTime)
    {
        while (Time.time < initialTime + cooldownTime)
        {
            float percentage = 1f - ((Time.time - initialTime) / cooldownTime);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (int)Mathf.Ceil(percentage * initialCooldownHeight)); // can't have half-pixels
            yield return new WaitForEndOfFrame();
        }

        CooldownImage.enabled = false;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, initialCooldownHeight);
    }

    public void SetVisible(bool shouldBeVisible)
    {
        gameObject.SetActive(shouldBeVisible);
    }
}
