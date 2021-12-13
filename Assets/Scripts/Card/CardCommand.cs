using UnityEngine;
// Base Class
public abstract class CardCommand : ScriptableObject
{
    [HideInInspector] public virtual CardEffect Effect { get; }
}
