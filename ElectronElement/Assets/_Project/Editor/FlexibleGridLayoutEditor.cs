using UnityEditor;

namespace UnityTools.UI
{
    [CustomEditor(typeof(FlexibleGridLayout))]
    public class FlexibleGridLayoutEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FlexibleGridLayout target = (FlexibleGridLayout)base.target;

            base.OnInspectorGUI();

            target.fitType = (FlexibleGridLayout.FitType)EditorGUILayout.EnumPopup("Fit Type", target.fitType);

            target.spacing = EditorGUILayout.Vector2Field("Spacing", target.spacing);

            if (target.fitType == FlexibleGridLayout.FitType.FixedRows)
            {
                target.rows = EditorGUILayout.IntField("Rows", target.rows);
            }
            if(target.fitType == FlexibleGridLayout.FitType.FixedColumns)
            {
                target.columns = EditorGUILayout.IntField("Columns", target.columns);
            }

            if(target.fitType == FlexibleGridLayout.FitType.FixedColumns || target.fitType == FlexibleGridLayout.FitType.FixedRows)
            {
                target.cellSize = EditorGUILayout.Vector2Field("Cell Size", target.cellSize);

                target.fitX = EditorGUILayout.Toggle("Fit X", target.fitX);
                target.fitY = EditorGUILayout.Toggle("Fit Y", target.fitY);
            }
        }
    }
}