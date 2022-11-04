using UnityEditor;
using UnityEngine;

namespace GameUtil.Editor
{
    public abstract class PropertyDrawerBase : PropertyDrawer
    {
        protected Rect mPosition;
        protected float mLastHeight;
        protected float mAllHeight;

        protected void Init(Rect position)
        {
            mPosition = position;
            mLastHeight = -2;
        }

        protected Rect GetRect(float height)
        {
            mPosition.y += mLastHeight + 2; //add 2 pixel height
            mLastHeight = height;
            mPosition.height = mLastHeight;
            return mPosition;
        }

        protected Rect GetPropertyRect(SerializedProperty property, bool includeChildren = false)
        {
            return GetRect(EditorGUI.GetPropertyHeight(property, includeChildren));
        }

        protected bool PropertyField(SerializedProperty property, GUIContent label = null, bool includeChildren = false)
        {
            return EditorGUI.PropertyField(GetPropertyRect(property, includeChildren), property, label,
                includeChildren);
        }

        protected bool PropertyField(SerializedProperty property, string name, bool includeChildren = false)
        {
            return PropertyField(property, new GUIContent(name), includeChildren);
        }

        protected void PropertySliderField(SerializedProperty property, float min = 0, float max = 1,
            bool includeChildren = false)
        {
            var position = GetPropertyRect(property, includeChildren);
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    EditorGUI.Slider(position, property, min, max);
                    break;
                case SerializedPropertyType.Integer:
                    EditorGUI.IntSlider(position, property, (int)min, (int)max);
                    break;
                default:
                    EditorGUI.LabelField(position, "Use Range with float or int.");
                    break;
            }
        }

        protected void AddPropertyHeight(SerializedProperty property, bool includeChildren = false)
        {
            mAllHeight += EditorGUI.GetPropertyHeight(property, includeChildren) + 2; //add 2 pixel height
        }
    }
}