using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    private const int SpawnersCount = 3;
    [SerializeField] private Spawner[] spawners = new Spawner[SpawnersCount];
    
    [SerializeField] private int packCount;
    [SerializeField] private float throwObjectDelayTime;
    [SerializeField] private float throwPackDelayTime;
    [SerializeField] private float timeDecreaseQuotient;

    private int _objectsToThrowCount; 
    
    private readonly Timer _throwPackTimer = new Timer();
    private readonly Timer _throwObjectTimer = new Timer();

    private bool _stopSpawn;

    public void Stop()
    {
        _stopSpawn = true;
    }

    private class Timer
    {
        private float _currentTime;
        private float _timeToWait;
        private bool _timerIsSet;

        public void UpdateTimer()
        {
            if (_timerIsSet)
            {
                _currentTime += Time.deltaTime;
            }
        }

        public void SetTimer(float timeToWait)
        {
            _timeToWait = timeToWait;
            _currentTime = 0;
            _timerIsSet = true;
        }
        
        public bool CheckTimer()
        {
            if (_timerIsSet)
            {
                return _timeToWait < _currentTime;
            }

            return true;
        }
    }

    private void Start()
    {
        _throwPackTimer.SetTimer(throwPackDelayTime);
    }

    private void Spawn()
    {
        _throwPackTimer.UpdateTimer();
        _throwObjectTimer.UpdateTimer();
        
        if (_throwPackTimer.CheckTimer() && _throwObjectTimer.CheckTimer())
        {
            _throwObjectTimer.SetTimer(throwObjectDelayTime);
            
            _objectsToThrowCount--;

            spawners[Random.Range(0, SpawnersCount)].SpawnObject();

            if (_objectsToThrowCount == 0)
            {
                _throwPackTimer.SetTimer(throwPackDelayTime);

                packCount++;
                throwPackDelayTime *= timeDecreaseQuotient;
                throwObjectDelayTime *= timeDecreaseQuotient;
                _objectsToThrowCount = packCount;
            }
        }
    }

    private void Update()
    {
        if (!_stopSpawn)
        {
            Spawn();
        }
    }
}
