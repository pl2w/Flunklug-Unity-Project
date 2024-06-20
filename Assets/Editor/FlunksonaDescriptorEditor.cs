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

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Throw Strength");
            descriptor.ThrowStrength = EditorGUILayout.FloatField(descriptor.ThrowStrength);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Fast Movement Speed");
            descriptor.FastMoveSpeed = EditorGUILayout.FloatField(descriptor.FastMoveSpeed);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Grab Audio");
            descriptor.GrabAudio = (AudioSource)EditorGUILayout.ObjectField(descriptor.GrabAudio, typeof(AudioSource));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Drop Audio");
            descriptor.DropAudio = (AudioSource)EditorGUILayout.ObjectField(descriptor.DropAudio, typeof(AudioSource));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Collision Audio");
            descriptor.CollisionAudio = (AudioSource)EditorGUILayout.ObjectField(descriptor.CollisionAudio, typeof(AudioSource));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Fast Movement Audio");
            descriptor.FastMoveAudio = (AudioSource)EditorGUILayout.ObjectField(descriptor.FastMoveAudio, typeof(AudioSource));
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

            string file = EditorUtility.SaveFilePanel("Select Export Location", "", descriptor.Name, "flunksona");

            if (File.Exists(file))
                File.Delete(file);

            string directory = Path.GetDirectoryName(file);

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