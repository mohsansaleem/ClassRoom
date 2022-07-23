using System;
using UnityEngine;

namespace PG.CastleBuilder.Context.Gameplay
{
    public class GridController : MonoBehaviour
    {
        public GameObject ThisGameObject;
        public Transform GridTransform;
        public MeshRenderer GridMeshRenderer;

        // Events
        public event Action OnMouseDownAtGrid;
        public event Action OnMouseUpAtGrid;
        
        public bool Visible
        {
            get => ThisGameObject.activeInHierarchy;
            set => ThisGameObject.SetActive(value);
        }

        private Vector2 _gridSize;

        public Vector2 GridSize
        {
            get => _gridSize;
            set
            {
                _gridSize = value;
                Vector2 sizeInUnits = _gridSize * Constants.GridTileSize;
                GridTransform.localPosition = new Vector3(sizeInUnits.x / 2, 0, sizeInUnits.y / 2);
                GridTransform.localScale = new Vector3(_gridSize.x / 2, 1, _gridSize.y / 2);

                GridMeshRenderer.material.SetFloat("_GridSizeX", _gridSize.x);
                GridMeshRenderer.material.SetFloat("_GridSizeY", _gridSize.y);
            }
        }

        private void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
            
            if (!InputUtils.IsPointerOverUI(0))
            {
                OnMouseDownAtGrid?.Invoke();
            }
            else
            {
                Debug.Log("PointerOverUI");
            }
        }

        private void OnMouseUp()
        {
            Debug.Log("OnMouseUp");
            
            if (!InputUtils.IsPointerOverUI(0))
            {
                OnMouseUpAtGrid?.Invoke();
            }
            else
            {
                Debug.Log("PointerOverUI");
            }
        }
    }
}