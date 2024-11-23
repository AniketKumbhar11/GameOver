using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : FlipImage
{

    private void Start()
    {
        if (!isOpen)
            targetImage.sprite = startSprite;
        else
            targetImage.enabled= false;
    }
}
