using UnityEngine;

public enum CollectibleType
{
    Curry,
    Wing,
    Fishballs,
    Sotong
}

public class CollectibleIdentity : MonoBehaviour
{
    public CollectibleType type;
}