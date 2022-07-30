using UnityEngine;

public class BonusHeart : Fruit
{
    private const int BonusIndex = 8;
    [SerializeField] private string textForCutting;
    [SerializeField] private float textScaleMultiplier;

    protected override void SetSprite()
    {
        CurrentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    protected override void CutBehavior()
    {
        playerConfiguration.HealPlayer();
        spawner.ExecuteFruit(gameObject, CurrentSprite, BonusIndex, FirstTapPosition, SecondTapPosition);
        spawner.SpawnText(RangeX, RangeY, gameObject.transform.position, textScaleMultiplier,textForCutting);
    }
}
