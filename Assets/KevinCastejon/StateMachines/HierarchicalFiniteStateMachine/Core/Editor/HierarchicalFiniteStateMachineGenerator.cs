using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace KevinCastejon.HierarchicalFiniteStateMachine.Generator
{
    internal class StateMachineData
    {
        private string _name;
        private bool _isRoot;
        private string _enumName = "MyState";
        private bool _isSubStateMachine;
        private int _defaultState;
        private List<StateMachineData> _states;
        private ReorderableList _statesList;

        public StateMachineData(string name, bool isRoot = false)
        {
            _name = name;
            IsRoot = isRoot;
            if (IsRoot)
            {
                IsSubStateMachine = true;
            }
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
        public bool IsRoot
        {
            get
            {
                return _isRoot;
            }

            set
            {
                _isRoot = value;
            }
        }
        public int DefaultState { get => _defaultState; set => _defaultState = value; }
        public string EnumName { get => _enumName; set => _enumName = value; }
        public bool IsSubStateMachine
        {
            get
            {
                return _isSubStateMachine;
            }

            set
            {
                bool hasChanged = _isSubStateMachine != value;
                if (hasChanged)
                {
                    _isSubStateMachine = value;
                    if (_isSubStateMachine)
                    {
                        _states = new List<StateMachineData>() { new StateMachineData("STATE_A"), new StateMachineData("STATE_B") }; 
                        InitializeList();
                    }
                    else
                    {
                        _states.Clear();
                    }
                }
            }
        }
        public List<StateMachineData> States { get => _states; }
        public ReorderableList StatesList { get => _statesList; }
        public bool IsNameExisting(string name, bool treatRoot = true)
        {
            if (treatRoot && _isRoot && _name == name)
            {
                return true;
            }
            if (_states == null)
            {
                return false;
            }
            foreach (StateMachineData item in _states)
            {
                if (item.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        public string GetName(List<int> navigation)
        {
            if (navigation.Count == 0)
            {
                return Name;
            }
            else
            {
                int nextIndex = navigation[0];
                navigation.RemoveAt(0);
                return _states[nextIndex].GetName(navigation);
            }
        }
        public StateMachineData GetStateMachine(List<int> navigation)
        {
            if (navigation.Count == 0)
            {
                return this;
            }
            else
            {
                int nextIndex = navigation[0];
                navigation.RemoveAt(0);
                return _states[nextIndex].GetStateMachine(navigation);
            }
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
            _states[index].Name = EditorGUI.TextField(position1, _states[index].Name).ToUpper().Replace(" ", "_");
            if (EditorGUI.ToggleLeft(position2, "Is Default", _defaultState == index))
            {
                _defaultState = index;
            }
            _states[index].IsSubStateMachine = EditorGUI.ToggleLeft(position3, "SubStateMachine", _states[index].IsSubStateMachine);
            if (_states[index].IsSubStateMachine)
            {
                if (GUI.Button(position4, "Open"))
                {
                    HierarchicalFiniteStateMachineGenerator.OnStateOpen(index);
                }
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
            _states.Insert(index, new StateMachineData(HierarchicalFiniteStateMachineGenerator.MakeNameUnique("NEW_STATE", this)));
            list.Select(index);
        }

    }
    internal class HierarchicalFiniteStateMachineGenerator : EditorWindow
    {
        internal static StateMachineData _root;
        internal static List<int> _navigation;
        private static bool _useNamespace;
        private static bool _useFixedUpdate;
        private static bool _generateWrapper;
        private static string _namespace;

        [MenuItem("Window/HFSM Generator", false, 211)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(HierarchicalFiniteStateMachineGenerator));
            window.titleContent = new GUIContent("HFSM Generator");
        }
        public static string MakeNameUnique(string newName, StateMachineData sm, bool treatRoot = true)
        {
            while (!IsNameUnique(newName, sm, treatRoot))
            {
                newName += "_";
            }
            return newName;
        }
        private static bool IsNameUnique(string newName, StateMachineData sm, bool treatRoot = true)
        {
            return !sm.IsNameExisting(newName, treatRoot);
        }
        private void OnEnable()
        {
            if (_root == null)
            {
                _root = new StateMachineData("MyStateMachine", true);
                _navigation = new List<int>();
            }
        }
        internal static void OnStateOpen(int i)
        {
            _navigation.Add(i);
        }
        void OnGUI()
        {
            StateMachineData currentSM = _root.GetStateMachine(new List<int>(_navigation));
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(_navigation.Count == 0);
            if (GUILayout.Button(_root.GetName(new List<int>())))
            {
                _navigation.Clear();
            }
            EditorGUI.EndDisabledGroup();
            for (int i = 1; i < _navigation.Count + 1; i++)
            {
                EditorGUILayout.LabelField(">", GUILayout.Width(10f));
                EditorGUI.BeginDisabledGroup(i == _navigation.Count);
                if (GUILayout.Button(_root.GetName(_navigation.GetRange(0, i))))
                {
                    _navigation = _navigation.GetRange(0, i);
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField(currentSM.IsRoot ? "Root state machine name" : "Sub state machine name");
            if (currentSM.IsRoot)
            {
                currentSM.Name = MakeNameUnique(EditorGUILayout.TextField(currentSM.Name).Replace(" ", "_"), currentSM, false);
            }
            else
            {
                EditorGUILayout.LabelField(GetPascalCase(currentSM.Name) + "StateMachine");
            }
            EditorGUILayout.LabelField(currentSM.IsRoot ? "Root state machine enum name" : "Sub state machine enum name");
            currentSM.EnumName = EditorGUILayout.TextField(currentSM.EnumName);
            currentSM.StatesList.DoLayoutList();
            EditorGUILayout.Space(10f);
            _generateWrapper = EditorGUILayout.ToggleLeft("Generate Wrapper Component", _generateWrapper);
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
                string copyPath = EditorUtility.SaveFolderPanel("Save the state machines scripts", Application.dataPath, _root.Name);
                if (!string.IsNullOrEmpty(copyPath))
                {
                    Generate(copyPath, _root);
                    GenerateWrapper(copyPath, _root);
                    AssetDatabase.Refresh();
                }
            }
        }

        private void GenerateWrapper(string path, StateMachineData root)
        {
            using (StreamWriter outfile = new StreamWriter(path + "/" + root.Name + "Component.cs"))
            {
                WriteLine($"using System.Collections;", false, outfile);
                WriteLine($"using System.Collections.Generic;", false, outfile);
                WriteLine($"using UnityEngine;", false, outfile);
                WriteLine($"using KevinCastejon.HierarchicalFiniteStateMachine;", false, outfile);
                if (_useNamespace && !string.IsNullOrEmpty(_namespace))
                {
                    WriteLine($"namespace {_namespace}", false, outfile);
                    WriteLine($"{{", false, outfile);
                }
                WriteLine($"public class {root.Name}Component : MonoBehaviour", _useNamespace, outfile);
                WriteLine($"{{", _useNamespace, outfile);
                WriteLine($"    private {root.Name} _stateMachine;", _useNamespace, outfile);
                WriteLine($"    private void Awake()", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                WriteLine($"        _stateMachine = AbstractHierarchicalFiniteStateMachine.CreateRootStateMachine<{root.Name}>(\"{root.Name}\", this);", _useNamespace, outfile);
                WriteLine($"    }}", _useNamespace, outfile);
                WriteLine($"    private void Start()", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                WriteLine($"        _stateMachine.OnEnter();", _useNamespace, outfile);
                WriteLine($"    }}", _useNamespace, outfile);
                WriteLine($"    private void Update()", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                WriteLine($"        _stateMachine.OnUpdate();", _useNamespace, outfile);
                WriteLine($"    }}", _useNamespace, outfile);
                if (_useFixedUpdate)
                {
                    WriteLine($"    private void FixedUpdate()", _useNamespace, outfile);
                    WriteLine($"    {{", _useNamespace, outfile);
                    WriteLine($"        _stateMachine.OnFixedUpdate();", _useNamespace, outfile);
                    WriteLine($"    }}", _useNamespace, outfile);
                }
                WriteLine($"}}", _useNamespace, outfile);

                if (_useNamespace && !string.IsNullOrEmpty(_namespace))
                {
                    WriteLine($"}}", false, outfile);
                }
            }
        }

        void Generate(string path, StateMachineData stateMachine)
        {
            using (StreamWriter outfile = new StreamWriter(path + "/" + (stateMachine.IsRoot ? stateMachine.Name : GetPascalCase(stateMachine.Name) + "StateMachine") + ".cs"))
            {
                WriteLine($"using KevinCastejon.HierarchicalFiniteStateMachine;", false, outfile);
                if (_useNamespace && !string.IsNullOrEmpty(_namespace))
                {
                    WriteLine($"namespace {_namespace}", false, outfile);
                    WriteLine($"{{", false, outfile);
                }
                WriteLine($"public class {(stateMachine.IsRoot ? stateMachine.Name : GetPascalCase(stateMachine.Name) + "StateMachine")} : AbstractHierarchicalFiniteStateMachine", _useNamespace, outfile);
                WriteLine($"{{", _useNamespace, outfile);
                WriteLine($"    public enum {stateMachine.EnumName}", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                for (int i = 0; i < stateMachine.States.Count; i++)
                {
                    WriteLine($"        {stateMachine.States[i].Name}" + (i == stateMachine.States.Count - 1 ? "" : ","), _useNamespace, outfile);
                }
                WriteLine($"    }}", _useNamespace, outfile);
                WriteLine($"    public {(stateMachine.IsRoot ? stateMachine.Name : GetPascalCase(stateMachine.Name) + "StateMachine")}()", _useNamespace, outfile);
                WriteLine($"    {{", _useNamespace, outfile);
                WriteLine($"        Init({stateMachine.EnumName}.{stateMachine.States[stateMachine.DefaultState].Name},", _useNamespace, outfile);
                bool isOneSubSm = false;
                for (int i = 0; i < stateMachine.States.Count; i++)
                {
                    if (stateMachine.States[i].IsSubStateMachine)
                    {
                        isOneSubSm = true;
                        WriteLine($"            Create<{GetPascalCase(stateMachine.States[i].Name)}StateMachine, {stateMachine.EnumName}>({stateMachine.EnumName}.{stateMachine.States[i].Name}, this)" + (i == stateMachine.States.Count - 1 ? "" : ","), _useNamespace, outfile);
                    }
                    else
                    {
                        WriteLine($"            Create<{GetPascalCase(stateMachine.States[i].Name)}State, {stateMachine.EnumName}>({stateMachine.EnumName}.{stateMachine.States[i].Name}, this)" + (i == stateMachine.States.Count - 1 ? "" : ","), _useNamespace, outfile);
                    }
                }
                WriteLine($"        );", _useNamespace, outfile);
                WriteLine($"    }}", _useNamespace, outfile);
                if (isOneSubSm)
                {
                    WriteLine($"    public override void OnExitFromSubStateMachine(AbstractHierarchicalFiniteStateMachine subStateMachine)", _useNamespace, outfile);
                    WriteLine($"    {{", _useNamespace, outfile);
                    WriteLine($"    }}", _useNamespace, outfile);
                }
                if (stateMachine.IsSubStateMachine)
                {
                    WriteLine($"    public override void OnStateMachineEntry()", _useNamespace, outfile);
                    WriteLine($"    {{", _useNamespace, outfile);
                    WriteLine($"    }}", _useNamespace, outfile);
                    WriteLine($"    public override void OnStateMachineExit()", _useNamespace, outfile);
                    WriteLine($"    {{", _useNamespace, outfile);
                    WriteLine($"    }}", _useNamespace, outfile);
                }
                for (int i = 0; i < stateMachine.States.Count; i++)
                {
                    if (stateMachine.States[i].IsSubStateMachine)
                    {
                        continue;
                    }
                    WriteLine($"    public class {GetPascalCase(stateMachine.States[i].Name)}State : AbstractState", _useNamespace, outfile);
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
            for (int i = 0; i < stateMachine.States.Count; i++)
            {
                if (stateMachine.States[i].IsSubStateMachine)
                {
                    Generate(path, stateMachine.States[i]);
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