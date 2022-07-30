using UnityEngine;

public class BonusHeart : Fruit
{
    private const int BonusIndex = 8;

    protected override void SetSprite()
    {
        CurrentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    protected override void CutBehavior()
    {
        playerConfiguration.HealPlayer();
        spawner.ExecuteBonus(gameObject, CurrentSprite, BonusIndex, FirstTapPosition, SecondTapPosition);
    }
}
