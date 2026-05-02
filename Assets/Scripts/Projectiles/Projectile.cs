using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Weapons weapon;

    public virtual void Init (Weapons weapon)
    {
        this.weapon = weapon;
    }

    public virtual void Launch()
    {

    }
}
