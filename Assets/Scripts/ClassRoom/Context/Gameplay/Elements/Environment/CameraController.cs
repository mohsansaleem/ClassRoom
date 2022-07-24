using PG.ClassRoom.Model;
using UnityEngine;
using Zenject;

namespace PG.ClassRoom.Context.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        public Transform CameraHolderTransform;
        public Camera BackgroundCamera;
        public Camera GridCamera;
        public Camera ModulesCamera;

        [Inject] private StaticDataModel _staticDataModel;
        
        // Locals
        private Vector3 _pointerStartPosition;
        // Raycast stuff to handle the Grid Position & Movement.
        public LayerMask layer_mask;

        public float CurrentZoom
        {
            set => BackgroundCamera.orthographicSize =
                GridCamera.orthographicSize = ModulesCamera.orthographicSize = value;
        }

        public Vector3 Position
        {
            get => CameraHolderTransform.position;
            set => CameraHolderTransform.position = value;
        }

        private void Update()
        {
            if (InputUtils.IsPointerOverUI(0))
            {
                return;
            }

            if (InputUtils.GetPointerDown(0))
            {
                Utils.GetGridPosition(out _pointerStartPosition);
            }

            if (InputUtils.GetPointerMove(0))
            {
                if (Utils.GetGridPosition(out var pos))
                {
                    Vector3 direction = _pointerStartPosition - pos;
                    /*Vector3 cameraPos = Vector3.Lerp(CameraHolderTransform.position,
                        (CameraHolderTransform.position + new Vector3(direction.x, CameraHolderTransform.position.y, direction.z)),
                        Time.deltaTime);*/
                    direction.y = 0;
                    Vector3 cameraPos = CameraHolderTransform.position + direction;
                    cameraPos.x = Mathf.Clamp(cameraPos.x, 0, _staticDataModel.MetaData.GridWidth * Constants.GridTileSize);
                    cameraPos.z = Mathf.Clamp(cameraPos.z, 0, _staticDataModel.MetaData.GridHeight * Constants.GridTileSize);

                    CameraHolderTransform.position = cameraPos;
                }
            }
        }
    }
}
