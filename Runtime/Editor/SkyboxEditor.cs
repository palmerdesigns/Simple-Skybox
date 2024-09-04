using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VolcanicGarage.Tools
{
    public class SkyboxEditor : EditorWindow
    {
        private List<Material> skyboxMaterials;
        private Vector2 scrollPosition;

        [MenuItem("Window/Simple Skybox Window")]
        public static void ShowWindow()
        {
            GetWindow<SkyboxEditor>("Skybox Editor");
        }

        private void OnEnable()
        {
            LoadSkyboxMaterials();
        }

        private void OnGUI()
        {
            #region Styles

            // Heading Style
            GUIStyle headingStyle = new GUIStyle(GUI.skin.label);
            headingStyle.fontSize = 24;
            headingStyle.fontStyle = FontStyle.Bold;
            headingStyle.normal.textColor = Color.white;

            // Button Style
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fixedHeight = 50;

            #endregion

            GUILayout.Label("All Skyboxes", headingStyle);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            #region Styling the buttons and grid

            int buttonSize = 100;
            int padding = 10;
            int margin = 20;
            int buttonPerRow = Mathf.Max(1, (int)(position.width - 2 * margin) / (buttonSize + padding));
            int rowCounter = 0;

            #endregion

            // Calculate the total width of the buttons and padding
            float totalWidth = buttonPerRow * (buttonSize + padding) - padding;

            // Center the area within the window
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical("box");
            GUILayout.Space(margin); 

            GUILayout.BeginHorizontal();
            GUILayout.Space(margin); 

            if (skyboxMaterials == null || skyboxMaterials.Count == 0)
            {
                EditorGUILayout.HelpBox("No skybox materials found. Please create a material inside a folder called Skyboxes within the Assets folder, and assign it to the skybox shader.", MessageType.Warning);
            }
            else
            {
                foreach (Material material in skyboxMaterials)
                {
                    if (material == null)
                    {
                        continue;
                    }

                    Texture2D thumbnail = AssetPreview.GetAssetPreview(material);
                    if (thumbnail == null)
                    {
                        continue;
                    }

                    if (GUILayout.Button(thumbnail, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        RenderSettings.skybox = material;
                        SceneView.RepaintAll();
                    }

                    rowCounter++;
                    if (rowCounter >= buttonPerRow)
                    {
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(margin); 
                        rowCounter = 0;
                    }
                    else
                    {
                        GUILayout.Space(padding); 
                    }
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            // Refresh Button
            if (GUILayout.Button("Refresh", buttonStyle))
            {
                LoadSkyboxMaterials();
            }

            // Force repaint to ensure the window updates
            Repaint();
        }

        private void LoadSkyboxMaterials()
        {
            #region Grab all skybox materials

            // Load all skybox materials
            skyboxMaterials = new List<Material>();

            string[] materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Skyboxes" });
            foreach (string guid in materialGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                if (material != null)
                {
                    skyboxMaterials.Add(material);
                }
            }
            #endregion
        }
    }






}
