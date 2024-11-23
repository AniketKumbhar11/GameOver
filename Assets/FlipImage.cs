using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FlipImage : MonoBehaviour
{
    public Image targetImage;       // Assign your Image in the Inspector
    public Sprite startSprite;     // The initial sprite
    public Sprite endSprite;       // The sprite to switch to during the flip
    private float flipDuration = 0.5f; // Duration of the flip effect
    public int CardID;
    public bool isOpen=false;
    public void FlipAndChangeSprite()
    {
        if (targetImage != null && startSprite != null && endSprite != null)
        {
            StartCoroutine(FlipHorizontalAndChangeSprite());
        }
        else
        {
            Debug.LogError("Target Image or sprites are not assigned!");
        }
    }

    private IEnumerator FlipHorizontalAndChangeSprite()
    {
        RectTransform rectTransform = targetImage.rectTransform;
        Vector3 initialScale = rectTransform.localScale;

        float halfDuration = flipDuration / 2f;
        float elapsed = 0f;

        // Step 1: Shrink horizontally
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float scaleX = Mathf.Lerp(1f, 0f, elapsed / halfDuration);
            rectTransform.localScale = new Vector3(scaleX, initialScale.y, initialScale.z);

            yield return null;
        }

        // Step 2: Swap the sprite at the midpoint
        rectTransform.localScale = new Vector3(0f, initialScale.y, initialScale.z);
        targetImage.sprite = endSprite;

        elapsed = 0f;

        // Step 3: Expand back horizontally
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float scaleX = Mathf.Lerp(0f, 1f, elapsed / halfDuration);
            rectTransform.localScale = new Vector3(scaleX, initialScale.y, initialScale.z);

            yield return null;
        }

        // Ensure the final scale is correctly set
        rectTransform.localScale = initialScale;

        // Invoke the callback when the flip is complete
        GameManager.OnFlipComplete?.Invoke(CardID, targetImage);
        
    }
}
