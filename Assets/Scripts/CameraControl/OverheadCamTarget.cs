using System;
using UnityEngine;

namespace CameraControl
{
    public class OverheadCamTarget : Singleton<OverheadCamTarget>
    {
        [SerializeField] private Transform _playerTransform;

        private void OnEnable()
        {
            EnhancedTouchManager.OnDragMove += HandleDragMove;
        }

        private void OnDisable()
        {
            EnhancedTouchManager.OnDragMove -= HandleDragMove;
        }

        public void SetToPlayerPosition()
        {
            transform.position = _playerTransform.position;
        }

        private void HandleDragMove(Vector2 delta)
        {
            // Invert tap&drag / pan movement
            transform.position += new Vector3(-delta.x, 0f, -delta.y);
        }
    }
}
