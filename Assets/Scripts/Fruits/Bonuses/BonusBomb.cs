using UnityEngine;

public class BonusBomb : Fruit
{
    private const int BonusIndex = 9;

    [SerializeField] private GameObject boomPrefab;
    [SerializeField] private BonusBomb bonusBomb;
    [SerializeField] private ObjectPhysics objectPhysics;

    protected override void SetSprite()
    {
        CurrentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    protected override void CutBehavior()
    {
        playerConfiguration.HitPlayer();
        spawner.SpawnEffect(boomPrefab, gameObject.transform.position).GetComponent<BoomAnimation>().bomb = gameObject;
        objectPhysics.enabled = false;
        bonusBomb.enabled = false;
    }
}
