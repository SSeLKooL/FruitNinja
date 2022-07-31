using UnityEngine;

public class BonusHeart : Fruit
{
    private const int BonusIndex = 8;
    [SerializeField] private string textForCutting;

    protected override void SetSprite()
    {
        CurrentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    protected override void CutBehavior()
    {
        playerConfiguration.HealPlayer();
        spawner.SpawnText(RangeX, RangeY, gameObject.transform.position,textForCutting);
        spawner.ExecuteFruit(gameObject, CurrentSprite, BonusIndex, FirstTapPosition, SecondTapPosition);
    }
}
