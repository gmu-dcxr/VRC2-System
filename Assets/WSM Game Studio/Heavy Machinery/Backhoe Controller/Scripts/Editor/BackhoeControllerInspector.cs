using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CustomEditor(typeof(BackhoeController))]
    public class BackhoeControllerInspector : Editor
    {
        private BackhoeController _backhoeController;

        protected SerializedProperty _loaderFrame;
        protected SerializedProperty _frontBucket;
        protected SerializedProperty _swingFrame;
        protected SerializedProperty _boom;
        protected SerializedProperty _arm;
        protected SerializedProperty _rearBucket;
        protected SerializedProperty _leftStabilizerLeg;
        protected SerializedProperty _rightStabilizerLeg;
        protected SerializedProperty _loaderFrameLever;
        protected SerializedProperty _frontBucketLever;
        protected SerializedProperty _swingFrameLever;
        protected SerializedProperty _boomLever;
        protected SerializedProperty _armLever;
        protected SerializedProperty _rearBucketLever;
        protected SerializedProperty _partsMovingSFX;
        protected SerializedProperty _partsStartMovingSFX;
        protected SerializedProperty _partsStopMovingSFX;

        private int _selectedMenuIndex = 0;
        private string[] _toolbarMenuOptions = new[] { "Settings", "Mechanical Parts", "SFX" };
        private GUIStyle _menuBoxStyle;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            _backhoeController = target as BackhoeController;

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
                #region SETTINGS
                GUILayout.Label("SETTINGS", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                bool isEngineOn = EditorGUILayout.Toggle("Engine On", _backhoeController.IsEngineOn);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Toggled Engine On");
                    _backhoeController.IsEngineOn = isEngineOn;
                    MarkSceneAlteration();
                }
                #endregion

                #region LOADER SETTINGS
                GUILayout.Label("LOADER SETTINGS", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                float loaderFrameSpeed = EditorGUILayout.FloatField("Loader Frame Speed", _backhoeController.loaderFrameSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Loader Frame Speed");
                    _backhoeController.loaderFrameSpeed = loaderFrameSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float bucketSpeed = EditorGUILayout.FloatField("Front Bucket Speed", _backhoeController.frontBucketSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Front Bucket Speed");
                    _backhoeController.frontBucketSpeed = bucketSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                LevelingMode levelingMode = (LevelingMode)EditorGUILayout.EnumPopup("Leveling Mode", _backhoeController.levelingMode);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Leveling Mode");
                    _backhoeController.levelingMode = levelingMode;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float selfLevelingSpeed = EditorGUILayout.FloatField("Self Leveling Speed", _backhoeController.loaderSelfLevelingSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Self Leveling Speed");
                    _backhoeController.loaderSelfLevelingSpeed = selfLevelingSpeed;
                    MarkSceneAlteration();
                }
                #endregion

                #region BACKHOE SETTINGS
                GUILayout.Label("BACKHOE SETTINGS", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                float swingFrameSpeed = EditorGUILayout.FloatField("Swing Frame Speed", _backhoeController.swingFrameSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Swing Frame Speed");
                    _backhoeController.swingFrameSpeed = swingFrameSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float boomSpeed = EditorGUILayout.FloatField("Boom Speed", _backhoeController.boomSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Boom Speed");
                    _backhoeController.boomSpeed = boomSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float armSpeed = EditorGUILayout.FloatField("Arm Speed", _backhoeController.armSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Arm Speed");
                    _backhoeController.armSpeed = armSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float rearBucketSpeed = EditorGUILayout.FloatField("Rear Bucket Speed", _backhoeController.rearBucketSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Rear Bucket Speed");
                    _backhoeController.rearBucketSpeed = rearBucketSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float stabilizerLegSpeed = EditorGUILayout.FloatField("Stabilizer Leg Speed", _backhoeController.stabilizerLegSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_backhoeController, "Changed Stabilizer Leg Speed");
                    _backhoeController.stabilizerLegSpeed = stabilizerLegSpeed;
                    MarkSceneAlteration();
                }
                #endregion
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Mechanical Parts")
            {
                serializedObject.Update();

                #region LOADER MECHANICAL PARTS
                GUILayout.Label("LOADER MECHANICAL PARTS", EditorStyles.boldLabel);

                _loaderFrame = serializedObject.FindProperty("loaderFrame");
                _frontBucket = serializedObject.FindProperty("frontBucket");
                EditorGUILayout.PropertyField(_loaderFrame, new GUIContent("Loader Frame"));
                EditorGUILayout.PropertyField(_frontBucket, new GUIContent("Front Bucket"));
                #endregion

                #region BACKHOE MECHANICAL PARTS
                GUILayout.Label("BACKHOE MECHANICAL PARTS", EditorStyles.boldLabel);

                _swingFrame = serializedObject.FindProperty("swingFrame");
                _boom = serializedObject.FindProperty("boom");
                _arm = serializedObject.FindProperty("arm");
                _rearBucket = serializedObject.FindProperty("rearBucket");
                _leftStabilizerLeg = serializedObject.FindProperty("leftStabilizerLeg");
                _rightStabilizerLeg = serializedObject.FindProperty("rightStabilizerLeg");

                EditorGUILayout.PropertyField(_swingFrame, new GUIContent("Swing Frame"));
                EditorGUILayout.PropertyField(_boom, new GUIContent("Boom"));
                EditorGUILayout.PropertyField(_arm, new GUIContent("Arm"));
                EditorGUILayout.PropertyField(_rearBucket, new GUIContent("Rear Bucket"));
                EditorGUILayout.PropertyField(_leftStabilizerLeg, new GUIContent("Left Stabilizer Leg"));
                EditorGUILayout.PropertyField(_rightStabilizerLeg, new GUIContent("Right Stabilizer Leg"));
                #endregion

                #region LOADER LEVERS
                GUILayout.Label("LOADER LEVERS", EditorStyles.boldLabel);

                _loaderFrameLever = serializedObject.FindProperty("loaderFrameLever");
                _frontBucketLever = serializedObject.FindProperty("frontBucketLever");
                EditorGUILayout.PropertyField(_loaderFrameLever, new GUIContent("Loader Frame Lever"));
                EditorGUILayout.PropertyField(_frontBucketLever, new GUIContent("Front Bucket Lever"));
                #endregion

                #region BACKHOE LEVERS
                GUILayout.Label("BACKHOE LEVERS", EditorStyles.boldLabel);

                _swingFrameLever = serializedObject.FindProperty("swingFrameLever");
                _boomLever = serializedObject.FindProperty("boomLever");
                _armLever = serializedObject.FindProperty("armLever");
                _rearBucketLever = serializedObject.FindProperty("rearBucketLever");

                EditorGUILayout.PropertyField(_swingFrameLever, new GUIContent("Swing Frame Lever"));
                EditorGUILayout.PropertyField(_boomLever, new GUIContent("Boom Lever"));
                EditorGUILayout.PropertyField(_armLever, new GUIContent("Arm Lever"));
                EditorGUILayout.PropertyField(_rearBucketLever, new GUIContent("Rear Bucket Lever"));
                #endregion

                serializedObject.ApplyModifiedProperties();
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "SFX")
            {
                #region AUDIO SOURCES
                GUILayout.Label("AUDIO SOURCES", EditorStyles.boldLabel);

                serializedObject.Update();

                _partsMovingSFX = serializedObject.FindProperty("partsMovingSFX");
                _partsStartMovingSFX = serializedObject.FindProperty("partsStartMovingSFX");
                _partsStopMovingSFX = serializedObject.FindProperty("partsStopMovingSFX");

                EditorGUILayout.PropertyField(_partsMovingSFX, new GUIContent("Moving SFX"));
                EditorGUILayout.PropertyField(_partsStartMovingSFX, new GUIContent("Starts Moving SFX"));
                EditorGUILayout.PropertyField(_partsStopMovingSFX, new GUIContent("Stops Moving SFX"));

                serializedObject.ApplyModifiedProperties(); 
                #endregion
            }

            GUILayout.EndVertical();
        }

        private void MarkSceneAlteration()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(_backhoeController);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }
    }
}
