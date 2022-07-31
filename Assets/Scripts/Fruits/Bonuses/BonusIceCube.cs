using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusIceCube : Fruit
{
    private const int BonusIndex = 9;
    [SerializeField] private string textForCutting;

    protected override void SetSprite()
    {
        CurrentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    protected override void CutBehavior()
    {
        playerConfiguration.FreezeTime();
        spawner.SpawnText(RangeX, RangeY, gameObject.transform.position, textForCutting);
        spawner.ExecuteFruit(gameObject, CurrentSprite, BonusIndex, FirstTapPosition, SecondTapPosition);
    }
}
