using UnityEngine;

public enum HitType
{
    Body,
    Head
}

public class EnemyHitbox : MonoBehaviour
{
    public HitType hitType;
}
