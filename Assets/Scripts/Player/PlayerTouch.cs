using TMPro.EditorUtilities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerTouch : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TrailRenderer trailRenderer;

    private Transform _playerTouchTransform;

    private Vector3 _tapPosition = new Vector3(0, 0, 0);

    private void Start()
    {
        _playerTouchTransform = this.gameObject.transform;
    }

    private void MoveTrail()
    {
        _playerTouchTransform.position = _tapPosition;
        trailRenderer.enabled = true;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];

            if (touch.phase == TouchPhase.Moved)
            {
                _tapPosition = (Vector2) mainCamera.ScreenToWorldPoint(touch.position);
                MoveTrail();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            _tapPosition = (Vector2) mainCamera.ScreenToWorldPoint(Input.mousePosition);
            MoveTrail();
        }
        else 
        {
            trailRenderer.enabled = false;
        }
    }
}
