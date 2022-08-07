using System;
using UnityEngine;

namespace Scripts.Misc
{
    public abstract class EditableObject : ScriptableObject
    {
        [HideInInspector] public string ID = Guid.NewGuid().ToString().ToUpper();
        public virtual string VisualTreeAsset { get; }

        public string FriendlyName;
        public Sprite Icon;
        public string Description;
    }
}