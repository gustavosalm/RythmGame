using UnityEngine;

namespace RhythmGameStarter
{
    [ExecuteInEditMode]
    public class MatchWidth : MonoBehaviour
    {
        [Comment("When using perspective camera, this component can handles the camera's fov and make sure all the track is being shown on screen")]
        public float horizontalFoV = 90.0f;

        public Camera _camera;

        private void Update()
        {
            if (!_camera) return;

            float halfWidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);

            float halfHeight = halfWidth * Screen.height / Screen.width;

            float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;

            _camera.fieldOfView = verticalFoV;
        }
    }
}