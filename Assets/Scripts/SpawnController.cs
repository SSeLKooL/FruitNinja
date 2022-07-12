using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private Spawner spawner1;
    [SerializeField] private Spawner spawner2;
    [SerializeField] private Spawner spawner3;
    private int _spawnerCount = 3;
    
    [SerializeField] private int packCount;
    [SerializeField] private float throwObjectDelayTime;
    [SerializeField] private float throwPackDelayTime;
    [SerializeField] private float timeDecreaseQuotient;

    private bool _isThrowing;

    private IEnumerator ThrowNewPack(int currentCount)
    {
        var timeToWait = throwObjectDelayTime;
        
        while (timeToWait > 0)
        {
            timeToWait -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        switch (Random.Range(1, _spawnerCount))
        {
            case 1:
                spawner1.SpawnObject();
                break;
            case 2:
                spawner2.SpawnObject();
                break;
            case 3:
                spawner3.SpawnObject();
                break;
        }

        if (--currentCount != 0)
        {
            StartCoroutine(ThrowNewPack(--currentCount));
        }
        else
        {
            packCount++;
            throwPackDelayTime *= timeDecreaseQuotient;
            throwObjectDelayTime *= timeDecreaseQuotient;
            _isThrowing = false;
        }
    }

    private IEnumerator Reload(float timeToWait)
    {
        while (timeToWait > 0)
        {
            timeToWait -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(ThrowNewPack(packCount));
    }

    private void Update()
    {
        if (!_isThrowing)
        {
            _isThrowing = true;
            StartCoroutine(Reload(throwPackDelayTime));
        }
    }
}
