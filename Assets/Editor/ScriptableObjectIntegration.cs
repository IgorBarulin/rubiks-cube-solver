using UnityEngine;
using UnityEditor;
using Assets.Scripts.CubeConstruct;

namespace Assets.Editor
{
    static class ScriptableObjectIntegration
    {
        [MenuItem("Assets/Create/PaletteColors")]
        public static void CreatePaletteColors()
        {
            ScriptableObjectUtility.CreateAsset<PaletteColors>();
        }

        [MenuItem("Assets/Create/CubeColorSheme")]
        public static void CreateCubeColorSheme()
        {
            ScriptableObjectUtility.CreateAsset<CubeColorSheme>();
        }
    }
}