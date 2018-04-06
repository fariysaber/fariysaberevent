using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class UISpriteAnim_Loop : UISpriteAnim
{
	protected override void Update () 
    {
        if (Pause)
        {
            return;
        }
        if (sprite == null)
        {
            return;
        }
        if (initImage == null || sprite.Length <= 1)
        {
            return;
        }
        startTime += Time.deltaTime;
        bool nextSprite = false;
        while (startTime > delayTime)
        {
            spriteIndex++;
            spriteIndex = spriteIndex % sprite.Length;
            startTime -= delayTime;
            nextSprite = true;
        }
        if (nextSprite)
        {
            initImage.sprite = sprite[spriteIndex];
        }
	}
}
