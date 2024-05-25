using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Flunklug
{
    [CustomEditor(typeof(FlunksonaDescriptor))]
    public class FlunksonaDescriptorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FlunksonaDescriptor descriptor = (FlunksonaDescriptor)target;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Author");
            descriptor.Author = EditorGUILayout.TextField(descriptor.Author);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Name");
            descriptor.Name = EditorGUILayout.TextField(descriptor.Name);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Throw Strength");
            descriptor.ThrowStrength = EditorGUILayout.FloatField(descriptor.ThrowStrength);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button($"Export {descriptor.Name}"))
            {
                Export(descriptor);
            }
        }

        public void Export(FlunksonaDescriptor descriptor)
        {
            EditorSceneManager.MarkSceneDirty(descriptor.gameObject.scene);
            EditorSceneManager.SaveScene(descriptor.gameObject.scene);

            PrefabUtility.SaveAsPrefabAsset(descriptor.gameObject, "Assets/TempSona.prefab");

            AssetBundleBuild build = new AssetBundleBuild
            {
                assetNames = new string[] { "Assets/TempSona.prefab" },
                assetBundleName = $"{descriptor.Name}.flunksona"
            };

            string directory = EditorUtility.OpenFolderPanel("Select Export Location", "", "");

            BuildPipeline.BuildAssetBundles(
                Application.temporaryCachePath,
                new AssetBundleBuild[]
                {
                build
                },
                0,
                BuildTarget.StandaloneWindows64
            );

            File.Move(Path.Combine(Application.temporaryCachePath, $"{descriptor.Name}.flunksona"), Path.Combine(directory, $"{descriptor.Name}.flunksona"));

            AssetDatabase.DeleteAsset("Assets/TempSona.prefab");
            AssetDatabase.Refresh();
        }
    }
}