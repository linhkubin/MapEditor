using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleMap : EditorWindow
{
	public GUIContent _content = new GUIContent();

	[MenuItem("Tools/SimpleMapTool")]
	public static void ShowWindow()
	{
		EditorWindow win = EditorWindow.GetWindow(typeof(SimpleMap));
		win.minSize = new Vector2(250.0f, 430.0f);

	}

	public void OnEnable()
	{
		_content.text = "SimpleMap";
		titleContent = _content;
	}

    private void OnProjectChange()
    {
		Debug.Log(Selection.gameObjects);
		Debug.Log("OnProjectChange");
	}

	private void OnFocus()
    {
		Debug.Log(Selection.gameObjects);
		Debug.Log("OnFocus");

	}

	private void OnGUI()
    {
		Debug.Log("ongui");


	}

}
