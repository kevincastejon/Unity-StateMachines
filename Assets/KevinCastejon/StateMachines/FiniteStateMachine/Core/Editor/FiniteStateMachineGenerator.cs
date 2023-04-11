using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace KevinCastejon.FiniteStateMachine.Generator
{
    internal class StateMachineData
    {
        private string _name;
        private string _enumName = "MyState";
        private int _defaultState;
        private List<string> _states = new List<string>() { "STATE_A", "STATE_B" };
        private ReorderableList _statesList;

        public StateMachineData(string name)
        {
            _name = name;
            InitializeList();
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        public int DefaultState { get => _defaultState; set => _defaultState = value; }
        public string EnumName { get => _enumName; set => _enumName = value; }
        public List<string> States { get => _states; }
        public ReorderableList StatesList { get => _statesList; }
        public bool IsNameExisting(string name, bool treatRoot = true)
        {
            if (treatRoot && _name == name)
            {
                return true;
            }
            if (_states == null)
            {
                return false;
            }
            foreach (string item in _states)
            {
                if (item == name)
                {
                    return true;
                }
            }
            return false;
        }

        private void InitializeList()
        {
            _statesList = new ReorderableList(_states, typeof(StateMachineData), true, true, true, true);
            _statesList.drawElementCallback = DrawElementCallback;
            _statesList.drawHeaderCallback = DrawHeaderCallback;
            _statesList.onRemoveCallback = OnRemoveCallback;
            _statesList.onCanRemoveCallback = OnCanRemoveCallback;
            _statesList.onAddCallback = OnAddCallback;
        }
        private bool OnCanRemoveCallback(ReorderableList list)
        {
            return list.count > 2;
        }
        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "States", EditorStyles.boldLabel);
        }
        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect position1 = new Rect(rect.x, rect.y, rect.width / 4, rect.height);
            Rect position2 = new Rect(rect.x + (rect.width / 4), rect.y, rect.width / 4, rect.height);
            Rect position3 = new Rect(rect.x + (rect.width / 4) * 2, rect.y, rect.width / 4, rect.height);
            Rect position4 = new Rect(rect.x + (rect.width / 4) * 3, rect.y, rect.width / 4, rect.height);
            _states[index] = EditorGUI.TextField(position1, _states[index]).ToUpper().Replace(" ", "_");
            if (EditorGUI.ToggleLeft(position2, "Is Default", _defaultState == index))
            {
                _defaultState = index;
            }
        }
        private void OnRemoveCallback(ReorderableList list)
        {
            _states.RemoveAt(list.index);
            if (_defaultState == list.index)
            {
                _defaultState--;
                if (_defaultState == -1)
                {
                    _defaultState = 0;
                }
            }
            if (list.count > 0)
            {
                list.index = list.index - 1 >= 0 ? list.index - 1 : 0;
            }
            else
            {
                list.index = -1;
            }
        }
        private void OnAddCallback(ReorderableList list)
        {
            int index = list.count == 0 ? 0 : list.index + 1;
            _states.Insert(index, FiniteStateMachineGenerator.MakeNameUnique("NEW_STATE"));
            list.Select(index);
        }

    }
    internal class FiniteStateMachineGenerator : EditorWindow
    {
        internal static StateMachineData _sm;
        private static bool _useFixedUpdate;
        private static bool _useNamespace;
        private static string _namespace;

        [MenuItem("Window/FSM Generator", false, 211)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(FiniteStateMachineGenerator));
            window.titleContent = new GUIContent("FSM Generator");
        }
        public static string MakeNameUnique(string newName, bool treatRoot = true)
        {
            while (!IsNameUnique(newName, treatRoot))
            {
                newName += "_";
            }
            return newName;
        }
        private static bool IsNameUnique(string newName, bool treatRoot = true)
        {
            return !_sm.IsNameExisting(newName, treatRoot);
        }
        private void OnEnable()
        {
            if (_sm == null)
            {
                _sm = new StateMachineData("MyStateMachine");
            }
        }
        void OnGUI()
        {
            StateMachineData currentSM = _sm;
            EditorGUILayout.LabelField("State machine name");
            currentSM.Name = MakeNameUnique(EditorGUILayout.TextField(currentSM.Name).Replace(" ", "_"), false);
            EditorGUILayout.LabelField("State machine enum name");
            currentSM.EnumName = EditorGUILayout.TextField(currentSM.EnumName);
            currentSM.StatesList.DoLayoutList();
            EditorGUILayout.Space(10f);
            _useFixedUpdate = EditorGUILayout.ToggleLeft("Use FixedUpdate", _useFixedUpdate);
            EditorGUILayout.BeginHorizontal();
            _useNamespace = EditorGUILayout.ToggleLeft("Use Namespace", _useNamespace);
            if (_useNamespace)
            {
                EditorGUILayout.LabelField("Namespace", GUILayout.Width(75f));
                _namespace = EditorGUILayout.TextField(_namespace);
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Generate"))
            {
                string copyPath = EditorUtility.SaveFolderPanel("Save the state machines scripts", Application.dataPath, _sm.Name);
                if (!string.IsNullOrEmpty(copyPath))
                {
                    Generate(copyPath, _sm);
                    AssetDatabase.Refresh();
                }
            }
        }

        void Generate(string path, StateMachineData stateMachine)
        {
            using (StreamWriter outfile = new StreamWriter(path + "/" + stateMachine.Name + ".cs"))
            {
                WriteLine($"using System.Collections;", false, outfile);
                WriteLine($"using System.Collections.Generic;", false, outfile);
                WriteLine($"using UnityEngine;", false, outfile);
                WriteLine($"using KevinCastejon.FiniteStateMachine;", false, outfile);
                if (_useNamespace && !string.IsNullOrEmpty(_namespace))
                {
                    WriteLine($"namespace {_namespace}", false, outfile);
                    WriteLine($"{{", false, outfile);
                }
                WriteLine($"public class {stateMachine.Name} : AbstractFiniteStateMachine", _useNamespace, outfile);
                WriteLine($"{{", _useNamespace, outfile);
                WriteLine($"    public enum {stateMachine.EnumName}", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                for (int i = 0; i < stateMachine.States.Count; i++)
                {
                    WriteLine($"        {stateMachine.States[i]}" + (i == stateMachine.States.Count - 1 ? "" : ","), _useNamespace, outfile);
                }
                WriteLine($"    }}", _useNamespace, outfile);
                WriteLine($"    private void Awake()", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                WriteLine($"        Init({stateMachine.EnumName}.{stateMachine.States[stateMachine.DefaultState]},", _useNamespace, outfile);
                bool isOneSubSm = false;
                for (int i = 0; i < stateMachine.States.Count; i++)
                {
                    WriteLine($"            AbstractState.Create<{GetPascalCase(stateMachine.States[i])}State, {stateMachine.EnumName}>({stateMachine.EnumName}.{stateMachine.States[i]}, this)" + (i == stateMachine.States.Count - 1 ? "" : ","), _useNamespace, outfile);
                }
                WriteLine($"        );", _useNamespace, outfile);
                WriteLine($"    }}", _useNamespace, outfile);
                if (isOneSubSm)
                {
                    WriteLine($"    public override void OnExitFromSubStateMachine(AbstractFiniteStateMachine subStateMachine)", _useNamespace, outfile);
                    WriteLine($"    {{", _useNamespace, outfile);
                    WriteLine($"    }}", _useNamespace, outfile);
                }
                for (int i = 0; i < stateMachine.States.Count; i++)
                {
                    WriteLine($"    public class {GetPascalCase(stateMachine.States[i])}State : AbstractState", _useNamespace, outfile);
                    WriteLine($"    {{", _useNamespace, outfile);
                    WriteLine($"        public override void OnEnter()", _useNamespace, outfile);
                    WriteLine($"        {{", _useNamespace, outfile);
                    WriteLine($"        }}", _useNamespace, outfile);
                    WriteLine($"        public override void OnUpdate()", _useNamespace, outfile);
                    WriteLine($"        {{", _useNamespace, outfile);
                    WriteLine($"        }}", _useNamespace, outfile);
                    if (_useFixedUpdate)
                    {
                        WriteLine($"        public override void OnFixedUpdate()", _useNamespace, outfile);
                        WriteLine($"        {{", _useNamespace, outfile);
                        WriteLine($"        }}", _useNamespace, outfile);
                    }
                    WriteLine($"        public override void OnExit()", _useNamespace, outfile);
                    WriteLine($"        {{", _useNamespace, outfile);
                    WriteLine($"        }}", _useNamespace, outfile);
                    WriteLine($"    }}", _useNamespace, outfile);
                }
                WriteLine($"}}", _useNamespace, outfile);
                if (_useNamespace && !string.IsNullOrEmpty(_namespace))
                {
                    WriteLine($"}}", false, outfile);
                }
            }
        }
        private void WriteLine(string str, bool addExtraSpace, StreamWriter outfile)
        {
            outfile.WriteLine(addExtraSpace ? "    " + str : str);
        }
        private string GetPascalCase(string enumName)
        {
            string o = enumName;
            enumName = enumName.ToLower().Replace("_", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            enumName = info.ToTitleCase(enumName).Replace(" ", string.Empty);
            return enumName;
        }
    }
}