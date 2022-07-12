using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private GameObject _fruit;
    [SerializeField] private float gravitation;
    [SerializeField] private float lifeTime;
    public Vector3 direction = new Vector3(0, 0, 0);
    private const int FramesPerSecond = 60;
    private void Start()
    {
        _fruit = this.gameObject;
        gravitation /= FramesPerSecond;
        StartCoroutine(Execution(lifeTime));
    }

    private void CheckCollider()
    {
        
    }

    private IEnumerator Execution(float timeToWait)
    {
        while (timeToWait > 0)
        {
            timeToWait -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        Destroy(_fruit);
    }
    

    private void Move()
    {
        _fruit.transform.position += direction;

        direction.y -= gravitation;
    }

    private void FixedUpdate()
    {
        Move();
    }
}
