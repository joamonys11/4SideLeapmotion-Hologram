// CarouselPro copyright (c) 2016 Sidéma SPRL

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Sidema.CarouselPro
{
    public class Carousel : MonoBehaviour, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Visibility transition type.
        /// </summary>
        public enum VisibilityTransitionType
        {
            NONE,
            UP,
            DOWN
        }

        [Serializable]
        public struct Slot
        {
            public GameObject slotObject; // game object of the slot
            public GameObject attachedObject; // game object inserted into the slot (attached to the slot object)
            public RotateObject rotator; // component of slot game object used to apply a rotation animation to the slot
            public Vector3 position; // position relative to the pivot
            public float angle; // angle relative to the z-axis of the pivot
        }
        
        /// <summary>
        /// Invoked when selection changed and animation is complete.
        /// </summary>
        public Action<Carousel, int> onSelectionChanged;

        /// <summary>
        /// Invoked when the carousel is shown and animation is complete.
        /// </summary>
        public Action<Carousel> onShow;

        /// <summary>
        /// Invoked when the carousel is hidden and animation is complete.
        /// </summary>
        public Action<Carousel> onHide;

        /// <summary>
        /// The radius of the circle along which slots are distributed.
        /// </summary>
        public float radius
        {
            get { return m_Radius; }
            set
            {
                if (m_CurrentRadius != value)
                {
                    m_Radius = value;
                    m_CurrentRadius = m_Radius;
                    RedistributeSlots();
                }
            }
        }

        /// <summary>
        /// Returns true if the carousel is objects are distributed in counterclockwise fashion.
        /// </summary>
        public bool counterclockwise
        {
            get { return m_Counterclockwise; }
            set {
                if (value != m_CurrentCounterclockwise && m_Slots.Length > 0)
                {
                    m_Counterclockwise = value;
                    m_CurrentCounterclockwise = m_Counterclockwise;
                    RedistributeSlots();
                }
            }
        }

        /// <summary>
        /// True if carousel is rotated when changing selection; false otherwise (setting this to
        /// true will rotate the carousel so that selected object is aligned with Z axis).
        /// </summary>
        public bool rotateCarousel
        {
            get { return m_RotateCarousel; }
            set
            {
                m_Pivot.localRotation = Quaternion.Euler(0f, -m_Slots[m_SelectedSlotIndex].angle, 0f);
                m_RotateCarousel = value;
            }
        }

        /// <summary>
        /// The duration of the carousel's rotation animation.
        /// </summary>
        public float rotationDuration
        {
            get { return m_RotationDuration; }
            set { m_RotationDuration = value; }
        }

        /// <summary>
        /// The easing curve for rotation animation of the carousel.
        /// </summary>
        public AnimationCurve rotationEasing
        {
            get { return m_RotationEasing; }
            set { m_RotationEasing = value; }
        }

        /// <summary>
        /// The scaling factor applied to unselected slots.
        /// </summary>
        public float slotScalingFactorUnselected
        {
            get { return m_SlotScalingFactorUnselected; }
            set { m_SlotScalingFactorUnselected = value; }
        }

        /// <summary>
        /// The scaling factor applied to selected slot.
        /// </summary>
        public float slotScalingFactorSelected
        {
            get { return m_SlotScalingFactorSelected; }
            set { m_SlotScalingFactorSelected = value; }
        }

        /// <summary>
        /// The duration of the slot scaling animation.
        /// </summary>
        public float slotScalingDuration
        {
            get { return m_SlotScalingDuration; }
            set { m_SlotScalingDuration = value; }
        }

        /// <summary>
        /// The easing curve used for slot scaling animation.
        /// </summary>
        public AnimationCurve slotScalingEasing
        {
            get { return m_SlotScalingEasing; }
            set { m_SlotScalingEasing = value; }
        }

        /// <summary>
        /// The offset applied to unselected slots.
        /// </summary>
        public Vector3 slotOffsetUnselected
        {
            get { return m_SlotOffsetUnselected; }
            set
            {
                m_SlotOffsetUnselected = value;
                ApplySlotsEffects();
            }
        }

        /// <summary>
        /// The offset applied to selected slot.
        /// </summary>
        public Vector3 slotOffsetSelected
        {
            get { return m_SlotOffsetSelected; }
            set
            {
                m_SlotOffsetSelected = value;
                ApplySlotsEffects();
            }
        }

        /// <summary>
        /// The duration of the slot offset animation.
        /// </summary>
        public float slotOffsetDuration
        {
            get { return m_SlotOffsetDuration; }
            set { m_SlotOffsetDuration = value; }
        }

        /// <summary>
        /// The curve used to ease the slot offset animation.
        /// </summary>
        public AnimationCurve slotOffsetEasing
        {
            get { return m_SlotOffsetEasing; }
            set { m_SlotOffsetEasing = value; }
        }

        /// <summary>
        /// The current rotation of the carousel.
        /// </summary>
        public float currentRotation
        {
            get { return m_Pivot.eulerAngles.y; }
        }

        /// <summary>
        /// The angle increment between two objects.
        /// </summary>
        public float angleIncrement { get; private set; }

        /// <summary>
        /// The number of slots.
        /// </summary>
        public int count { get { return m_Objects.Count; } }

        /// <summary>
        /// The object inserted in the selected slot. Can be null if the slot is empty.
        /// </summary>
        public GameObject selectedObject { get { return (m_SelectedSlotIndex >= 0) ? m_Objects[m_SelectedSlotIndex] : null; } }

        /// <summary>
        /// The selected slot index. -1 if no slots are present in the carousel.
        /// </summary>
        public int selectedSlotIndex {  get { return m_SelectedSlotIndex; } }

        /// <summary>
        /// The slot object pool size.
        /// </summary>
        public int slotObjectPoolSize { get { return m_SlotObjectPool.Count; } }

        /// <summary>
        /// Gets or sets the object at given index.
        /// </summary>
        /// <param name="index">The index of the object to get or set.</param>
        /// <returns></returns>
        public GameObject this[int index]
        {
            get
            {
                return m_Objects[index];
            }

            set
            {
                m_Objects[index] = value;
                OnObjectsChanged();
            }
        }

        [SerializeField]
        List<GameObject> m_Objects = new List<GameObject>();
        
        [Header("Slot placement")]
        [SerializeField]
        float m_Radius = 1f;

        [SerializeField]
        bool m_Counterclockwise;
        
        [Header("Rotation")]
        [SerializeField]
        bool m_RotateCarousel = true;

        [SerializeField]
        float m_RotationDuration = 0.5f;

        [SerializeField]
        AnimationCurve m_RotationEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Slot selection animations")]
        [SerializeField]
        float m_SlotScalingFactorUnselected = 0.5f;

        [SerializeField]
        float m_SlotScalingFactorSelected = 1f;

        [SerializeField]
        float m_SlotScalingDuration = 0.5f;

        [SerializeField]
        AnimationCurve m_SlotScalingEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        Vector3 m_SlotOffsetUnselected = Vector3.zero;

        [SerializeField]
        Vector3 m_SlotOffsetSelected = Vector3.zero;

        [SerializeField]
        float m_SlotOffsetDuration = 0.5f;

        [SerializeField]
        AnimationCurve m_SlotOffsetEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        Vector3 m_SlotRotationSpeedUnselected = Vector3.zero;

        [SerializeField]
        Vector3 m_SlotRotationSpeedSelected = Vector3.zero;

        [Header("Visibility transition animations")]
        [SerializeField]
        float m_VisibilityTransitionDuration = 0.5f;

        [SerializeField]
        AnimationCurve m_VisibilityTransitionEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        AnimationCurve m_ShowTransitionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField]
        AnimationCurve m_HideTransitionCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
                
        [SerializeField, HideInInspector]
        Slot[] m_Slots = new Slot[0];

        Stack<GameObject> m_SlotObjectPool = new Stack<GameObject>();

        [SerializeField, HideInInspector]
        GameObject[] m_SlotObjectPoolSerialized = null;

        [SerializeField, HideInInspector]
        Transform m_Pivot;

        // Current selected element, Next increments, Previous decrements; [0, #slots)
        [SerializeField, HideInInspector]
		 int m_SelectedSlotIndex = -1;

        private bool m_Rotating;

        private bool m_CurrentCounterclockwise;
        private float m_CurrentRadius;
        private bool m_Rotated;

        /// <summary>
        /// Determines if a given object is in the carousel.
        /// </summary>
        /// <param name="gameObject">The object to locate.</param>
        /// <returns>True if the object is in the carousel.</returns>
        public bool Contains(GameObject gameObject)
        {
            return m_Objects.Contains(gameObject);
        }

        /// <summary>
        /// Returns the slot index of the given object, or -1 if it cannot be located. 
        /// </summary>
        /// <param name="gameObject">The object to locate.</param>
        public int IndexOf(GameObject gameObject)
        {
            return m_Objects.IndexOf(gameObject);
        }

        /// <summary>
        /// Returns the slot index of the given object, or -1 if it cannot be located.
        /// Search starts at given index and stops at the end of carousel's list.
        /// </summary>
        /// <param name="gameObject">The object to locate.</param>
        /// <param name="index">The starting index of the search.</param>
        public int IndexOf(GameObject gameObject, int index)
        {
            return m_Objects.IndexOf(gameObject, index);
        }

        /// <summary>
        /// Returns the slot index of the given object, or -1 if it cannot be located. 
        /// Search starts at given index and stops after count slots.
        /// </summary>
        /// <param name="gameObject">The object to locate.</param>
        /// <param name="index">The starting index of the search.</param>
        /// <param name="count">The number of slots to search into.</param>
        public int IndexOf(GameObject gameObject, int index, int count)
        {
            return m_Objects.IndexOf(gameObject, index, count);
        }

        /// <summary>
        /// Inserts an object in a new slot at given index. If the object is already present in the
        /// carousel, it simply moves to the new slot leaving previous slot empty.
        /// </summary>
        /// <param name="index">The index where the object should be inserted.</param>
        /// <param name="gameObject">The object to insert.</param>
        public void Insert(int index, GameObject gameObject)
        {
            var previousSlotIndex = IndexOf(gameObject);

            if (previousSlotIndex >= 0)
                m_Objects[previousSlotIndex] = null;

            m_Objects.Insert(index, gameObject);
            
            OnObjectsChanged();
        }

        /// <summary>
        /// Inserts a collection of objects in the carousel in new slots from given index. Inserting
        /// objects already present in the carousel will simply move them to the new slots leaving
        /// previous slots empty.
        /// </summary>
        /// <param name="index">The index where to insert the objects.</param>
        /// <param name="gameObjects">The objects to insert to the carousel.</param>
        public void InsertRange(int index, IEnumerable<GameObject> gameObjects)
        {
            if (gameObjects == null)
                throw new NullReferenceException();

            var initialCount = m_Objects.Count;

            foreach(GameObject go in gameObjects)
            {
                var previousSlotIndex = IndexOf(go);

                if (previousSlotIndex >= 0)
                    m_Objects[previousSlotIndex] = null;
            }

            m_Objects.InsertRange(index, gameObjects);

            if (m_Objects.Count != initialCount)
                OnObjectsChanged();
        }

        /// <summary>
        /// Adds an object to the carousel in a new slot. Adding null will create an empty slot.
        /// Adding an object already present in the carousel will simply move it to the new slot
        /// leaving previous slot empty.
        /// </summary>
        /// <param name="gameObject">The object to add.</param>
        /// <returns>The index of the slot into which the object has been added.</returns>
        public int Add(GameObject gameObject)
        {
            m_Objects.Add(gameObject);
           
            OnObjectsChanged();

            return m_Objects.Count - 1;
        }

        /// <summary>
        /// Adds a collection of objects to the carousel in new slots. Adding objects already present
        /// in the carousel will simply move them to the new slots leaving previous slots empty.
        /// </summary>
        /// <param name="gameObjects">The objects to add to the carousel.</param>
        public void AddRange(IEnumerable<GameObject> gameObjects)
        {
            if (gameObjects == null)
                throw new NullReferenceException();

            var initialCount = m_Objects.Count;

            m_Objects.AddRange(gameObjects);

            if (m_Objects.Count != initialCount)
                OnObjectsChanged();
        }

        /// <summary>
        /// Removes the given object from the carousel if present and delete its slot. 
        /// The removed object is un-parented from the carousel.
        /// </summary>
        /// <param name="gameObject">The object to remove.</param>
        public void Remove(GameObject gameObject)
        {
            for (int i = 0; i < m_Slots.Length; i++)
            {
                if (m_Slots[i].attachedObject == gameObject)
                    RemoveAt(i);
            }
        }

        /// <summary>
        /// Removes the object from the slot at given index (if any) and delete its slot. 
        /// The removed object (if any) is un-parented from the carousel.
        /// </summary>
        /// <param name="index">The slot index.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > m_Slots.Length - 1)
                throw new ArgumentOutOfRangeException("index");
            
            m_Objects.RemoveAt(index);

            OnObjectsChanged();
        }

        /// <summary>
        /// Removes all empty slot from the carousel.
        /// </summary>
        public void RemoveAllEmpty()
        {
            var initialCount = m_Objects.Count;

            for (int i = m_Objects.Count-1; i >= 0; i--)
            {
                if (m_Objects[i] == null)
                {
                    m_Objects.RemoveAt(i);
                }
            }
            
            if (initialCount != m_Objects.Count)
                OnObjectsChanged();
        }

        /// <summary>
        /// Removes all the objects from carousel. Objects are un-parented from the carousel.
        /// </summary>
        public void Clear()
        {
            m_Objects.Clear();
            OnObjectsChanged();
        }

        /// <summary>
        /// Returns the object in the slot at given index or null if the slot is empty.
        /// </summary>
        /// <param name="index">The slot index.</param>
        public GameObject Get(int index)
        {
            return m_Objects[index];
        }

        /// <summary>
        /// Sets the object in the slot at given index to the given object.
        /// </summary>
        /// <param name="index">The slot index.</param>
        /// <param name="gameObject">The object to set.</param>
        public void Set(int index, GameObject gameObject)
        {
            m_Objects[index] = gameObject;
            OnObjectsChanged();
        }

        /// <summary>
        /// Copies the objects in the carousel to an array.
        /// </summary>
        /// <param name="array">The array to copy objects to.</param>
        public void CopyTo(GameObject[] array)
        {
            m_Objects.CopyTo(array);
        }

        /// <summary>
        /// Copies the objects in the carousel to an array beginning at given index in the target array.
        /// </summary>
        /// <param name="array">The array to copy objects to.</param>
        /// <param name="index">The index from where to start copying in the array.</param>
        public void CopyTo(GameObject[] array, int index)
        {
            m_Objects.CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the carousel objects.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return m_Objects.GetEnumerator();
        }

        /// <summary>
        /// Selects the previous slot in the carousel (cycle to the last slot if current selection is the first slot).
        /// </summary>
        /// <param name="onSelectionChanged">Callback invoked when selection has changed and animation is complete.</param>
        public void Previous(Action onSelectionChanged = null)
        {
            if (m_SelectedSlotIndex < 0)
                return;

            Slot currentSlot = m_Slots[m_SelectedSlotIndex];

            int previousIndex = pmod(m_SelectedSlotIndex - 1, m_Objects.Count);
            Slot previousSlot = m_Slots[previousIndex];

            if (Rotate(-previousSlot.angle, false))
            {
                ApplyUnselectEffects(currentSlot);
                ApplySelectEffects(previousSlot);
                m_SelectedSlotIndex = previousIndex;
            }

        }

        /// <summary>
        /// Selects the next slot in the carousel (cycle to the first slot if current selection is the last slot).
        /// </summary>
        /// <param name="onSelectionChanged">Callback invoked when selection has changed and animation is complete.</param>
        public void Next(Action onSelectionChanged = null)
        {
            if (m_SelectedSlotIndex < 0)
                return;

            Slot currentSlot = m_Slots[m_SelectedSlotIndex];

            int nextIndex = (m_SelectedSlotIndex + 1) % m_Objects.Count;
            Slot nextSlot = m_Slots[nextIndex];

			if (Rotate(-nextSlot.angle, false))
            {
                ApplyUnselectEffects(currentSlot);
                ApplySelectEffects(nextSlot);
                m_SelectedSlotIndex = nextIndex;
            }

        }

        /// <summary>
        /// Selects the slot at given index.
        /// </summary>
        /// <param name="index">The index of the selection</param>
        /// <param name="onSelectionChanged">Callback invoked when selection has changed and animation is complete.</param>
        public void SetSelection(int index, Action onSelectionChanged = null)
        {
            if (index < 0 || index > m_Slots.Length - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Slot currentSlot = m_Slots[m_SelectedSlotIndex];
            Slot selectedObjectSlot = m_Slots[index];

            if (Rotate(-selectedObjectSlot.angle, false))
            {
                ApplyUnselectEffects(currentSlot);
                ApplySelectEffects(selectedObjectSlot);
                m_SelectedSlotIndex = index;
            }
        }

        /// <summary>
        /// Shows the carousel with an optional transition animation and an optional callback invoked when animation is complete.
        /// </summary>
        /// <param name="transition">The type of visibility transition to use.</param>
        /// <param name="onComplete">Callback invoked when animation is complete.</param>
        public void Show(VisibilityTransitionType transition = VisibilityTransitionType.NONE, Action onComplete = null)
        {
            m_Pivot.localScale = Vector3.one;
            m_Pivot.localPosition = Vector3.zero;

            switch (transition)
            {
                case VisibilityTransitionType.NONE:
                    gameObject.SetActive(true);
                    break;
                case VisibilityTransitionType.UP:
                    m_Pivot.transform.localPosition -= Vector3.up * m_Radius;
                    m_Pivot.transform.localScale = Vector3.zero;

                    gameObject.SetActive(true);

                    m_Pivot
                        .DOLocalMoveY(0f, m_VisibilityTransitionDuration)
                        .SetEase(m_VisibilityTransitionEasing);

                    m_Pivot
                        .DOScale(1f, m_VisibilityTransitionDuration)
                        .SetEase(m_ShowTransitionCurve)
                        .OnComplete(() =>
                        {
                            if (onShow != null)
                                onShow.Invoke(this);

                            if (onComplete != null)
                                onComplete.Invoke();
                        });
                    break;
                case VisibilityTransitionType.DOWN:
                    m_Pivot.transform.localPosition += Vector3.up * m_Radius;
                    m_Pivot.transform.localScale = Vector3.zero;

                    gameObject.SetActive(true);

                    m_Pivot
                        .DOLocalMoveY(0f, m_VisibilityTransitionDuration)
                        .SetEase(m_VisibilityTransitionEasing);

                    m_Pivot
                        .DOScale(1f, m_VisibilityTransitionDuration)
                        .SetEase(m_ShowTransitionCurve)
                        .OnComplete(() =>
                        {
                            if (onShow != null)
                                onShow.Invoke(this);

                            if (onComplete != null)
                                onComplete.Invoke();
                        });

                    break;
            }
        }

        /// <summary>
        /// Hides the carousel with an optional transition animation and an optional callback invoked animation is complete.
        /// </summary>
        /// <param name="transition">The type of visibility transition to use.</param>
        /// <param name="onComplete">Callback invoked when animation is complete.</param>
        public void Hide(VisibilityTransitionType transition = VisibilityTransitionType.NONE, Action onComplete = null)
        {
            m_Pivot.localScale = Vector3.one;
            m_Pivot.localPosition = Vector3.zero;

            switch (transition)
            {
                case VisibilityTransitionType.NONE:
                    gameObject.SetActive(false);
                    break;
                case VisibilityTransitionType.UP:
                    m_Pivot
                        .DOLocalMoveY(m_Radius, m_VisibilityTransitionDuration)
                        .SetEase(m_VisibilityTransitionEasing)
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);

                            if (onHide != null)
                                onHide.Invoke(this);

                            if (onComplete != null)
                                onComplete.Invoke();
                        });

                    m_Pivot
                        .DOScale(0f, m_VisibilityTransitionDuration)
                        .SetEase(m_HideTransitionCurve);
                    break;
                case VisibilityTransitionType.DOWN:
                    m_Pivot
                        .DOLocalMoveY(-m_Radius, m_VisibilityTransitionDuration)
                        .SetEase(m_VisibilityTransitionEasing)
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);

                            if (onHide != null)
                                onHide.Invoke(this);

                            if (onComplete != null)
                                onComplete.Invoke();
                        });

                    m_Pivot
                        .DOScale(0f, m_VisibilityTransitionDuration)
                        .SetEase(m_HideTransitionCurve);
                    break;
            }
        }

        /// <summary>
        /// Clears the slot object pool used to store unused slot objects.
        /// </summary>
        public void ClearSlotObjectPool()
        {
            if (m_SlotObjectPool != null)
            {
                while (m_SlotObjectPool.Count > 0)
                {
                    GameObject go = m_SlotObjectPool.Pop();
                    if (go != null)
                        DestroyImmediate(go);
                }
            }
        }

        void Awake()
        {
            if (m_Pivot == null)
            {
                m_Pivot = new GameObject("Pivot").transform;
                m_Pivot.SetParent(transform, false);
            }

            m_CurrentRadius = m_Radius;
        }

        void OnDestroy()
        {
            ClearSlotObjectPool();
        }

        void OnValidate()
        {
#if UNITY_EDITOR
            bool isPrefabAsset = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) == null && 
                UnityEditor.PrefabUtility.GetPrefabObject(gameObject.transform) != null;

            if (isPrefabAsset)
                return;
#endif
            if (m_Pivot == null)
            {
                m_Pivot = new GameObject("Pivot").transform;
                m_Pivot.SetParent(transform, false);
            }

            // Sync slots and list of objects
            if (m_Slots.Length != m_Objects.Count)
            {
                OnObjectsChanged();
            }
            else
            {
                // Check attached objects
                for (int i = 0; i < m_Objects.Count; i++)
                {
                    if (m_Objects[i] != m_Slots[i].attachedObject)
                    {
                        OnObjectsChanged();
                        break;
                    }
                }
            }

            radius = m_Radius;
            
            if (m_SlotScalingDuration > m_RotationDuration)
                m_SlotScalingDuration = m_RotationDuration;

            if (m_SlotOffsetDuration > m_RotationDuration)
                m_SlotOffsetDuration = m_RotationDuration;

            rotateCarousel = m_RotateCarousel;
            counterclockwise = m_Counterclockwise;

            ApplySlotsEffects();
        }

        private void OnObjectsChanged()
        {
            UpdateSlots();
            RedistributeSlots();

            // Restore a valid index if objects changed and our index falls outside of the range of slots
            if ((m_SelectedSlotIndex < 0 || m_SelectedSlotIndex >= m_Slots.Length))
            {
                if (m_Slots.Length > 0)
                    m_SelectedSlotIndex = 0;
                else
                    m_SelectedSlotIndex = -1;

                if (m_Rotated)
                {
                    m_Pivot.localRotation = Quaternion.Euler(Vector3.zero);
                    m_Rotated = false;
                }
            }

            if (m_SelectedSlotIndex > 0)
                m_Pivot.transform.localRotation = Quaternion.Euler(0f, -m_Slots[m_SelectedSlotIndex].angle, 0f);
            else
                m_Pivot.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            ApplySlotsEffects();
        }

        private void OnSelectionChanged()
        {
            m_Rotating = false;

            if (onSelectionChanged != null)
                onSelectionChanged.Invoke(this, m_SelectedSlotIndex);
        }

        public void OnBeforeSerialize()
        {
            if (m_SlotObjectPool != null)
            {
                m_SlotObjectPoolSerialized = new GameObject[m_SlotObjectPool.Count];
                m_SlotObjectPool.CopyTo(m_SlotObjectPoolSerialized, 0);
            }
        }

        public void OnAfterDeserialize()
        {
            if (m_SlotObjectPoolSerialized == null)
                return;

            m_SlotObjectPool.Clear();

            for (int i = 0; i < m_SlotObjectPoolSerialized.Length; i++)
            {
                m_SlotObjectPool.Push(m_SlotObjectPoolSerialized[i]);
            }
        }

        private void ApplySlotsEffects()
        {
            for (int i = 0; i < m_Slots.Length; i++)
            {
                Slot slot = m_Slots[i];

                if (m_SelectedSlotIndex == i)
                {
                    slot.slotObject.transform.localScale = Vector3.one * m_SlotScalingFactorSelected;
                    slot.slotObject.transform.localPosition = slot.position + m_SlotOffsetSelected;
                    slot.rotator.m_speedPerAxis = m_SlotRotationSpeedSelected;
                }
                else
                {
                    slot.slotObject.transform.localScale = Vector3.one * m_SlotScalingFactorUnselected;
                    slot.slotObject.transform.localPosition = slot.position + m_SlotOffsetUnselected;
                    slot.rotator.m_speedPerAxis = m_SlotRotationSpeedUnselected;
                }
            }
        }

        private void RecycleSlotObjects()
        {
            for (int i = 0; i < m_Slots.Length; i++)
            {
                var slotObject = m_Slots[i].slotObject;
                if (slotObject != null)
                {
                    while (slotObject.transform.childCount > 0)
                        slotObject.transform.GetChild(0).SetParent(null, false);

                    m_SlotObjectPool.Push(slotObject);
                    slotObject.SetActive(false);
                    slotObject.hideFlags = HideFlags.HideInHierarchy;
                }
            }
        }

        private Slot CreateSlotForObject(GameObject gameObject)
        {
            Slot s = new Slot();
            s.attachedObject = gameObject;

            if (m_SlotObjectPool.Count > 0)
            {
                s.slotObject = m_SlotObjectPool.Pop();
                s.slotObject.SetActive(true);
                s.slotObject.hideFlags = HideFlags.None;
                s.rotator = s.slotObject.GetComponent<RotateObject>();
                if (s.rotator == null)
                    s.rotator = s.slotObject.AddComponent<RotateObject>();
            }
            else
            {
                s.slotObject = new GameObject("Slot");
                s.slotObject.transform.SetParent(m_Pivot, false);
                s.rotator = s.slotObject.AddComponent<RotateObject>();
            }

            s.rotator.m_speedPerAxis = m_SlotRotationSpeedUnselected;

            if (gameObject != null)
            {
                gameObject.transform.SetParent(s.slotObject.transform, false);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
            }

            return s;
        }

        private void UpdateSlots()
        {
            RecycleSlotObjects();

            m_Slots = new Slot[m_Objects.Count];

            for (int i = 0; i < m_Objects.Count; i++)
            {
                // Already in the carousel? Make sure we don't have duplicates
                int lastIndex = m_Objects.IndexOf(m_Objects[i]);

                if (lastIndex >= 0 && lastIndex < i)
                {
                    m_Slots[lastIndex].attachedObject = null;
                    m_Objects[lastIndex] = null;
                }

                var es = CreateSlotForObject(m_Objects[i]);
                es.slotObject.transform.SetSiblingIndex(i);
                m_Slots[i] = es;
            }
        }

        private void RedistributeSlots()
        {
            float currentAngle = 0;
            angleIncrement = 360f / m_Slots.Length;

            for (int i = 0; i < m_Slots.Length; i++)
            {
                Slot slot = m_Slots[i];

                slot.slotObject.transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
                slot.slotObject.transform.localPosition = Quaternion.Euler(0f, currentAngle, 0f) * (Vector3.forward * m_Radius);
                slot.position = slot.slotObject.transform.localPosition;
                slot.angle = currentAngle;

                m_Slots[i] = slot;

                currentAngle += (m_Counterclockwise ? -1f : 1f) * angleIncrement;
            }
        }

        // Computes "positive modulo" m of x (let m = 5, if x = 5 --> 0, x = -1 --> 4)
        private static int pmod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        private bool Rotate(float angle, bool relative = true)
        {
            m_Rotated = true;

            if (!m_Rotating)
            {
                if (m_RotateCarousel)
                {
                    m_Pivot.DOLocalRotate(new Vector3(0f, angle, 0f),
                        m_RotationDuration,
						relative ? RotateMode.WorldAxisAdd : RotateMode.Fast).SetEase(m_RotationEasing).OnComplete(OnSelectionChanged);
                }
                else
                {
                    StartCoroutine(Animating());
                }

                m_Rotating = true;
                return true;
            }

            return false;
        }

        private IEnumerator Animating()
        {
            yield return new WaitForSeconds(m_RotationDuration);
            OnSelectionChanged();
            m_Rotating = false;
        }

        private void ApplyUnselectEffects(Slot slot)
        {
            slot.rotator.m_speedPerAxis = m_SlotRotationSpeedUnselected;
            slot.slotObject.transform.DOLocalMove(slot.position + m_SlotOffsetUnselected, m_SlotScalingDuration).SetEase(m_SlotOffsetEasing);
            slot.slotObject.transform.DOScale(m_SlotScalingFactorUnselected, m_SlotOffsetDuration).SetEase(m_SlotScalingEasing);
        }

        private void ApplySelectEffects(Slot slot)
        {
            slot.rotator.m_speedPerAxis = m_SlotRotationSpeedSelected;
            slot.slotObject.transform.DOLocalMove(slot.position + m_SlotOffsetSelected, m_SlotScalingDuration).SetEase(m_SlotOffsetEasing);
            slot.slotObject.transform.DOScale(m_SlotScalingFactorSelected, m_SlotScalingDuration).SetEase(m_SlotScalingEasing);
        }
        
    }
}


