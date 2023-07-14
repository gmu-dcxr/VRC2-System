using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CustomEditor(typeof(DumpTruckController))]
    public class DumpTruckControllerInspector : Editor
    {
        private DumpTruckController _dumpTruckController;

        protected SerializedProperty _dumpBed;
        protected SerializedProperty _tailgateJoint;
        protected SerializedProperty _tailgateAnchor;
        protected SerializedProperty _dumpBedLever;
        protected SerializedProperty _partsMovingSFX;
        protected SerializedProperty _partsStartMovingSFX;
        protected SerializedProperty _partsStopMovingSFX;

        private int _selectedMenuIndex = 0;
        private string[] _toolbarMenuOptions = new[] { "Settings", "Mechanical Parts", "SFX" };
        private GUIStyle _menuBoxStyle;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            _dumpTruckController = target as DumpTruckController;

            EditorGUI.BeginChangeCheck();
            _selectedMenuIndex = GUILayout.Toolbar(_selectedMenuIndex, _toolbarMenuOptions);
            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(null);
            }

            //Set up the box style if null
            if (_menuBoxStyle == null)
            {
                _menuBoxStyle = new GUIStyle(GUI.skin.box);
                _menuBoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                _menuBoxStyle.fontStyle = FontStyle.Bold;
                _menuBoxStyle.alignment = TextAnchor.UpperLeft;
            }
            GUILayout.BeginVertical(_menuBoxStyle);

            if (_toolbarMenuOptions[_selectedMenuIndex] == "Settings")
            {
                /*
                 * SETTINGS
                 */
                GUILayout.Label("SETTINGS", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                bool isEngineOn = EditorGUILayout.Toggle("Engine On", _dumpTruckController.IsEngineOn);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_dumpTruckController, "Toggled Engine On");
                    _dumpTruckController.IsEngineOn = isEngineOn;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float dumpBedSpeed = EditorGUILayout.FloatField("Dump Bed Speed", _dumpTruckController.dumpBedSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_dumpTruckController, "Changed Dump Bed Speed");
                    _dumpTruckController.dumpBedSpeed = dumpBedSpeed;
                    MarkSceneAlteration();
                }
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Mechanical Parts")
            {
                /*
                 * MECHANICAL PARTS
                 */
                serializedObject.Update();

                GUILayout.Label("MECHANICAL PARTS", EditorStyles.boldLabel);

                _dumpBed = serializedObject.FindProperty("dumpBed");
                _tailgateJoint = serializedObject.FindProperty("tailgateJoint");
                _tailgateAnchor = serializedObject.FindProperty("tailgateAnchor");
                _dumpBedLever = serializedObject.FindProperty("dumpBedLever");

                EditorGUILayout.PropertyField(_dumpBed, new GUIContent("Dump Bed"));
                EditorGUILayout.PropertyField(_tailgateJoint, new GUIContent("Tailgate Joint"));
                EditorGUILayout.PropertyField(_tailgateAnchor, new GUIContent("Tailgate Anchor"));

                GUILayout.Label("LEVERS", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_dumpBedLever, new GUIContent("Dump Bed Lever"));

                serializedObject.ApplyModifiedProperties();
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "SFX")
            {
                /*
                 * SFX
                 */
                GUILayout.Label("AUDIO SOURCES", EditorStyles.boldLabel);

                serializedObject.Update();

                _partsMovingSFX = serializedObject.FindProperty("partsMovingSFX");
                _partsStartMovingSFX = serializedObject.FindProperty("partsStartMovingSFX");
                _partsStopMovingSFX = serializedObject.FindProperty("partsStopMovingSFX");

                EditorGUILayout.PropertyField(_partsMovingSFX, new GUIContent("Moving SFX"));
                EditorGUILayout.PropertyField(_partsStartMovingSFX, new GUIContent("Starts Moving SFX"));
                EditorGUILayout.PropertyField(_partsStopMovingSFX, new GUIContent("Stops Moving SFX"));

                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.EndVertical();
        }

        private void MarkSceneAlteration()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(_dumpTruckController);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }
    }
}
