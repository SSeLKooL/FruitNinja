using UnityEngine;

public class BonusBomb : Fruit
{
    private const int BonusIndex = 9;

    [SerializeField] private GameObject boomPrefab;
    [SerializeField] private BonusBomb bonusBomb;
    [SerializeField] private ObjectPhysics objectPhysics;
    [SerializeField] private string textFotCutting;

    protected override void SetSprite()
    {
        CurrentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    protected override void CutBehavior()
    {
        playerConfiguration.HitPlayer();
        spawner.SpawnText(RangeX, RangeY, transform.position, textFotCutting);
        spawner.SpawnEffect(boomPrefab, transform.position).GetComponent<BoomAnimation>().bomb = gameObject;
        objectPhysics.enabled = false;
        bonusBomb.enabled = false;
    }
}
