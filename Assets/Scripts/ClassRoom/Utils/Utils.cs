using System.IO;
using PG.Core.Installer;
using UnityEngine;

namespace PG.ClassRoom
{
    public static class Utils
    {
        public static bool VerifyDataFolder()
        {
            if (!Directory.Exists(Constants.DataFolder))
            {
                Directory.CreateDirectory(Constants.DataFolder);
            }
            
            return Directory.Exists(Constants.DataFolder);
        }
        
        public static void Fire<T>(this T signal) where T : Signal
        {
            signal.SignalBus.Fire(signal);
        }
        
        public static void MarkTheGrid(ref bool[,] grid, int wi, int li, int wm, int lm, bool marking = true)
        {
            for (int w = wi; w < wi + wm && w < grid.GetLength(0); w++)
            {
                for (int l = li; l < li + lm && w < grid.GetLength(1); l++)
                {
                    grid[w, l] = marking;
                }
            }
        }
        
        public static bool GetGridPosition(out Vector3 ret)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast (ray, out var hit))
            {
                ret = hit.point;
                return true;
            }

            ret = Vector3.zero;
            return false;
        }
    }
}