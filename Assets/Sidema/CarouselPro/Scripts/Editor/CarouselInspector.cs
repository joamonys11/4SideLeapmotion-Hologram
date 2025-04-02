using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Sidema.CarouselPro
{
    [CustomEditor(typeof(Carousel)), CanEditMultipleObjects]
    public class CarouselInspector : Editor
    {
        Carousel carousel;

        UnityEditorInternal.ReorderableList m_ObjectsRL;

        private Carousel.VisibilityTransitionType[] visibilityTransitions = {
            Carousel.VisibilityTransitionType.NONE,
            Carousel.VisibilityTransitionType.UP,
            Carousel.VisibilityTransitionType.DOWN
        };

        private GUIContent k_ClearSlotObjectPoolContent = new GUIContent("Clear slot object pool",
            "A pool is used to store unused slot objects. Each time a slot object is needed it is either created or retrieved from the pool.");

        private int m_SelectionIndex;

        void OnEnable()
        {
            carousel = target as Carousel;
            m_ObjectsRL = new UnityEditorInternal.ReorderableList(serializedObject, serializedObject.FindProperty("m_Objects"));

            m_ObjectsRL.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Slots");
            };

            m_ObjectsRL.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                Rect prefixRect = rect;
                prefixRect.yMin += 2f;
                Rect fieldRect = rect;
                fieldRect.yMin += 2f;
                fieldRect.xMin += 32f;
                fieldRect.height = EditorGUIUtility.singleLineHeight;
                var elementContent = new GUIContent(index.ToString());
                EditorGUI.PrefixLabel(prefixRect, elementContent);
                EditorGUI.PropertyField(fieldRect, m_ObjectsRL.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            GUILayout.Space(4f);

            if (serializedObject.isEditingMultipleObjects)
            {
                DrawPropertiesExcluding(serializedObject, "m_Objects", "m_Script", "m_SelectedSlotIndex");
            }
            else
            {
                m_ObjectsRL.DoLayoutList();
                DrawPropertiesExcluding(serializedObject, "m_Objects", "m_Script", "m_SelectedSlotIndex");
            }

            serializedObject.ApplyModifiedProperties();

            var position = EditorGUILayout.GetControlRect(false, 20f);
            position.y += 8f;

            GUI.Label(position, "Debug", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Selected slot index");
            EditorGUILayout.LabelField(serializedObject.FindProperty("m_SelectedSlotIndex").intValue.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Slot pool size");
            EditorGUILayout.LabelField(carousel.slotObjectPoolSize.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Previous"))
            {
                carousel.Previous();
            }

            if (GUILayout.Button("Next"))
            {
                carousel.Next();
            }

            if (GUILayout.Button("Select"))
            {
                carousel.SetSelection(m_SelectionIndex);
            }

            m_SelectionIndex = EditorGUILayout.IntField(m_SelectionIndex, GUILayout.Width(40f));

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (carousel.gameObject.activeSelf)
            {
                for (int i = 0; i < visibilityTransitions.Length; i++)
                {
                    if (GUILayout.Button("Hide (" + visibilityTransitions[i].ToString() + ")"))
                        carousel.Hide(visibilityTransitions[i]);
                }
            }
            else
            {
                for (int i = 0; i < visibilityTransitions.Length; i++)
                {
                    if (GUILayout.Button("Show (" + visibilityTransitions[i].ToString() + ")"))
                        carousel.Show(visibilityTransitions[i]);
                }
            }

            GUILayout.EndHorizontal();

            if (!AssetDatabase.Contains(target) && GUILayout.Button(k_ClearSlotObjectPoolContent))
            {
                carousel.ClearSlotObjectPool();
            }
        }

        void OnSceneGUI()
        {
            if (carousel != null)
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(carousel.transform.position, carousel.transform.up, carousel.radius);
            }
        }
    }
}
