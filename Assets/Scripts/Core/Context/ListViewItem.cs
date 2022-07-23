using UnityEngine;

namespace PG.Core.Context
{
    public abstract class ListViewItem<M> : MonoBehaviour
    {
        [HideInInspector]
        public M Data;

        public virtual void RefreshData(M model)
        {
            Data = model;
        }
    }
}