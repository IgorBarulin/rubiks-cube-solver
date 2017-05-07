using UnityEngine;
using UnityEditor;
using Assets.Scripts.CubeConstruct;

namespace Assets.Editor
{
    static class YourUnityIntegration
    {
        [MenuItem("Assets/Create/PaletteColors")]
        public static void CreateYourScriptableObject()
        {
            ScriptableObjectUtility.CreateAsset<PaletteColors>();
        }
    }
}