using UnityEngine;

public class PlayerTouch : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TrailRenderer trailRenderer;

    private Transform _playerTouchTransform;

    public Vector3 tapPosition = new Vector3(0, 0, 0);
    public bool isTouched;

    private void Start()
    {
        trailRenderer.enabled = false;
        _playerTouchTransform = this.gameObject.transform;
    }

    private void MoveTrail()
    {
        _playerTouchTransform.position = tapPosition;
        
        if (!isTouched)
        {
            trailRenderer.enabled = true;
            isTouched = true;
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];

            if (touch.phase == TouchPhase.Moved)
            {
                tapPosition = (Vector2) mainCamera.ScreenToWorldPoint(touch.position);
                MoveTrail();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            tapPosition = (Vector2) mainCamera.ScreenToWorldPoint(Input.mousePosition);
            MoveTrail();
        }
        else 
        {
            if (isTouched)
            {
                trailRenderer.enabled = false;
                isTouched = false;
            }
        }
    }
}
