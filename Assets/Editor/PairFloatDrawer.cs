using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PairFloat))]
public class PairFloatDrawer : PropertyDrawer
{
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label) 
	{
		EditorGUI.BeginProperty(pos, label, prop);

		pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		EditorGUIUtility.labelWidth = 30;

		Rect left = new Rect(pos.x, pos.y, pos.width / 2.0f - 1, pos.height);
		Rect right = new Rect(pos.x + pos.width / 2.0f + 2, pos.y, pos.width / 2.0f - 1, pos.height);

		EditorGUIUtility.labelWidth = 37;
		EditorGUI.PropertyField(left, prop.FindPropertyRelative("first"), new GUIContent("From"));
		EditorGUIUtility.labelWidth = 21;
		EditorGUI.PropertyField(right, prop.FindPropertyRelative("second"), new GUIContent("To"));

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
