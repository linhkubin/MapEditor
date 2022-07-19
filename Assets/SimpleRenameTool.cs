/****************************************
	SimpleRenameTool
	Copyright 2014 Unluck Software	
 	www.chemicalbliss.com																			
*****************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class SimpleRenameTool: EditorWindow {
	public string _replace = "";
	public string _replaceWith = "";
	public int _counter;
	public string _addString = "";
	public string _rename = "GameObject";
	public string _addToNumerate;
	public int _numerateStep = 1;
	public bool _alphabetSort;
	public bool _hierarchySort;
	public GUIContent _content = new GUIContent();

	[MenuItem("Window/SimpleRenameTool")]
	public static void ShowWindow() {
			EditorWindow win = EditorWindow.GetWindow(typeof(SimpleRenameTool));
			win.minSize = new Vector2(250.0f, 430.0f);
   			
	}
	
	public void OnEnable(){
			_content.text = "SimpleRenameTool";
			titleContent = _content;
	}
	
	public void Rename() {
		Transform[] _selection = Selection.transforms; //Add selection to array
		for(int i = 0; i < _selection.Length; i++) {
			Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "Rename");
			float p = (float)i;
			EditorUtility.DisplayProgressBar("Replacing String in GameObject Name", "", p / _selection.Length);
			_selection[i].name = _rename;
		}
	}

	public void ReplaceName() {
		Transform[] _selection = Selection.transforms; //Add selection to array
		for(int i = 0; i < _selection.Length; i++) {
			Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "Rename");
			float p = (float)i;
			EditorUtility.DisplayProgressBar("Replacing String in GameObject Name", "", p / _selection.Length);
			string n = _selection[i].gameObject.name;
			n = n.Replace(_replace, _replaceWith);
			_selection[i].name = n;
		}
	}

	public void AddString(string type) {
		Transform[] _selection = Selection.transforms; //Add selection to array
		for(int i = 0; i < _selection.Length; i++) {
			Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "Rename");
			float p = (float)i;
			EditorUtility.DisplayProgressBar("Replacing String in GameObject Name", "", p / _selection.Length);
			string n = _selection[i].gameObject.name;
			if (type == "suffix") n = n + _addString;
			else if (type == "prefix") n = _addString + n;
			_selection[i].name = n;
		}
	}
	
	public static int Compare(Transform g1,Transform g2) {
    	return g1.name.CompareTo (g2.name);
    }
    
	public static int Compare(int first, int second){
		int result = 0;
		if(first < second) result = -1;
		else if(first > second) result = 1;
		return result;
	}
	
	
	public void Numerate(string type) {
		Transform[] _selection = Selection.transforms; //Add selection to array
		if(_hierarchySort)
			System.Array.Sort(_selection, (g1, g2) => Compare(g1.GetSiblingIndex(), g2.GetSiblingIndex()));
		//_selection.Sort(_selection,function(g1,g2) Compare(g1.GetSiblingIndex(), g2.GetSiblingIndex()));
			if(_alphabetSort)
			System.Array.Sort(_selection, (g1, g2) => Compare(g1, g2));
		//	_selection.Sort(_selection,function(g1,g2) Compare(g1, g2));
		for (int i = 0; i < _selection.Length; i++) {
			Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "Rename");
			float p = (float)i;
			EditorUtility.DisplayProgressBar("Replacing String in GameObject Name", "", p / _selection.Length);
			string n = _selection[i].gameObject.name;
			if (type == "suffix") n = n + _addToNumerate + (_counter + (i * _numerateStep)).ToString("000");
			else if (type == "prefix") n = (_counter + (i * _numerateStep)).ToString("000") + _addToNumerate + n;
			_selection[i].name = n;
		}
	}
	
	public void RemoveChar(string type) {
		Transform[] _selection = Selection.transforms; //Add selection to array
		for(int i = 0; i < _selection.Length; i++) {
			Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "Rename");
			float p = (float)i;
			EditorUtility.DisplayProgressBar("Replacing String in GameObject Name", "", p / _selection.Length);
			string n = _selection[i].gameObject.name;
			if (n.Length > 1 && type == "last") {
				n = n.Remove(n.Length - 1);
				_selection[i].name = n;
			}
			else if (n.Length > 1 && type == "first") {
				n = n.Remove(0, 1);
				_selection[i].name = n;
			}
		}
	}
	
	 public void SavePrefabs() {
        if (Selection.gameObjects.Length > 0) {
            string path = EditorUtility.SaveFolderPanel("Select Folder ", "Assets/", "");
            if (path.Length > 0) {
                //Debug.Log(Application.dataPath);
                if (path.Contains("" + Application.dataPath)) {
                    string s = "" + path + "/";
                    string d = "" + Application.dataPath + "/";
                    string p = "Assets/" + s.Remove(0, d.Length);
                    Transform[] _selection = Selection.transforms;
                    bool cancel = false;
                    for(int i = 0; i < _selection.Length; i++) {
                    	float x = (float)i;
						EditorUtility.DisplayProgressBar("Creating Prefabs", "", x / _selection.Length);
                        if (!cancel) {
                            if (AssetDatabase.LoadAssetAtPath(p + _selection[i].gameObject.name + ".prefab", typeof(GameObject)) != null) {
                                //			var goName:String = go.name;

                                //					var i:int = go.name String. go.name.Length-1];
                                //				Debug.Log(i);
                                int option = EditorUtility.DisplayDialogComplex("Are you sure?", "" + _selection[i].gameObject.name + ".prefab" + " already exists. Do you want to overwrite it?", "Yes", "No", "Cancel");

                                switch (option) {
                                case 0:
                                    CreateNew(_selection[i].gameObject, p + _selection[i].gameObject.name + ".prefab");
                                    goto case 1;
                                case 1:
                                    break;
                                case 2:
                                    cancel = true;
                                    break;
                                default:
                                    Debug.LogError("Unrecognized option.");
                                    break;
                                }

                            } else
                                CreateNew(_selection[i].gameObject, p + _selection[i].gameObject.name + ".prefab");
                        }
                    }
                } else {
                    Debug.LogError("Prefab Save Failed: Can't save outside project: " + path);
                }
            }
        } else {
            Debug.LogWarning("No GameObjects Selected");
        }
    }
    
	public static void CreateNew(GameObject obj,string localPath) {
        GameObject prefab = PrefabUtility.CreateEmptyPrefab(localPath) as GameObject;
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }
    
	public void OnGUI() {
		////////////////////////////////////////////////////////////////////////////// R
		GUILayout.Space(20.0f);
		_rename = EditorGUILayout.TextField("", _rename);
		if (GUILayout.Button("Rename")) {
			this.Rename();
		}
		
		////////////////////////////////////////////////////////////////////////////// REPLACE
		GUILayout.Space(10.0f);
		_replace = EditorGUILayout.TextField("Replace in Name", _replace);
		_replaceWith = EditorGUILayout.TextField("Replace with", _replaceWith);
		if (GUILayout.Button("Replace In Name")) {
			this.ReplaceName();
		}
		GUILayout.Space(10.0f);
		
		////////////////////////////////////////////////////////////////////////////// NUMBERS
		_counter = EditorGUILayout.IntField("Start Number", _counter);
		if (_counter < 0) _counter = 0;
		_numerateStep = EditorGUILayout.IntField("Numerate Step", _numerateStep);
		if (_numerateStep < 1) _numerateStep = 1;
		_addToNumerate = EditorGUILayout.TextField("Numerate Character", _addToNumerate);
		_alphabetSort = EditorGUILayout.Toggle("Sort Alphabetically", _alphabetSort);
#if UNITY_4_5 || UNITY_4_6 || UNITY_5
		_hierarchySort = EditorGUILayout.Toggle("Sort Like Hierarchy", _hierarchySort);
#endif

		if (GUILayout.Button("Numerate Suffix")) {
			this.Numerate("suffix");
		}
		if (GUILayout.Button("Numerate Prefix")) {
			this.Numerate("prefix");
		}
		GUILayout.Space(10.0f);
		
		////////////////////////////////////////////////////////////////////////////// REMOVE
		if (GUILayout.Button("Remove First Character")) {
			RemoveChar("first");
		}
		if (GUILayout.Button("Remove Last Character")) {
			RemoveChar("last");
		}
		GUILayout.Space(10.0f);
		
		////////////////////////////////////////////////////////////////////////////// ADD
		_addString = EditorGUILayout.TextField("Add to Name", _addString);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Add Prefix")) {
			this.AddString("prefix");
		}
		if (GUILayout.Button("Add Suffix")) {
			this.AddString("suffix");
		}

		EditorGUILayout.EndHorizontal();

		GUILayout.Space(10.0f);
		
		////////////////////////////////////////////////////////////////////////////// SAVE
		 if (GUILayout.Button("Save Prefabs")) {
        	SavePrefabs();
        }
		
	}

	public void OnInspectorUpdate() {
		//   Repaint();
		EditorUtility.ClearProgressBar();
	}
}