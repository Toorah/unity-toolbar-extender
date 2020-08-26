using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class CustomToolbar
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("AppCommand")
			{
				fontSize = 14,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	static Texture2D m_icon;

	static CustomToolbar()
	{
		m_icon = Resources.Load<Texture2D>("renew");

		ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		if (EditorPrefs.GetBool("Start-Playing", false))
			DoPlay();
	}

	static void OnToolbarGUI()
	{
		GUILayout.FlexibleSpace();

		GUI.enabled = EditorApplication.isPlaying;

		if (GUILayout.Button(new GUIContent(m_icon, "Restart"), ToolbarStyles.commandButtonStyle))
		{
			EditorApplication.ExitPlaymode();
			EditorPrefs.SetBool("Start-Playing", true);
            EditorApplication.playModeStateChanged += TryPlayAgain;
		}

		GUI.enabled = true;
	}

    private static void TryPlayAgain(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
				DoPlay();
				break;
        }
    }

	private static void DoPlay()
    {
		EditorApplication.EnterPlaymode();
		EditorApplication.playModeStateChanged -= TryPlayAgain;
		EditorPrefs.SetBool("Start-Playing", false);
	}
}