using UnityEngine;

namespace Hexed.Extensions
{
    internal class HighlightHelper
    {
        public static void Init()
        {
            ChangeHighlightColor(Color.grey);
            ChangeCoreColor(Color.grey);
        }

        private static void ChangeHighlightColor(Color highlightcolor)
        {
            foreach (HighlightsFXStandalone HighlightFX in Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>())
            {
                HighlightFX.highlightColor = highlightcolor;
            }
        }

        private static void ChangeCoreColor(Color highlightcolor)
        {
            foreach (MeshRenderer Renderer in Resources.FindObjectsOfTypeAll<MeshRenderer>().Where(m => m.name.Contains("SelectRegion")))
            {
                Renderer.material.color = highlightcolor;
            }
        }

        public static void ToggleHighlightFx(HighlightsFXStandalone highlightFx, MeshFilter filter, bool State)
        {
            if (State) highlightFx.field_Protected_HashSet_1_MeshFilter_0.AddIfNotPresent(filter);
            else highlightFx.field_Protected_HashSet_1_MeshFilter_0.Remove(filter);
        }

        public static bool IsRenderPresent(HighlightsFXStandalone highlightFx, MeshFilter renderer)
        {
            return highlightFx.field_Protected_HashSet_1_MeshFilter_0.Contains(renderer);
        }
    }
}
