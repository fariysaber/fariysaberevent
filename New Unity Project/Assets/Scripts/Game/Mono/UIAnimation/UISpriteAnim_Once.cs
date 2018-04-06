using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpriteAnim_Once : UISpriteAnim
{
    protected override void Update()
    {
        if (Pause)
        {
            return;
        }
        if (sprite == null)
        {
            return;
        }
        if (initImage == null || sprite.Length <= 1 || spriteIndex >= sprite.Length - 1)
        {
            return;
        }
        bool nextSprite = false;
        startTime += Time.deltaTime;
        while (startTime > delayTime)
        {
            spriteIndex++;
            nextSprite = true;
            if (spriteIndex < sprite.Length - 1)
            {
                startTime -= delayTime;
            }
            else
            {
                break;
            }
        }
        if (nextSprite)
        {
            initImage.sprite = sprite[spriteIndex];
        }
    }
}
