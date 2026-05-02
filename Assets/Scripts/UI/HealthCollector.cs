using UnityEngine;

public class HealthCollector : MonoBehaviour
{
    public Player player;

    public void OnTriggerEnter(Collider other)
    {        
        if(player == null)
            player= other.GetComponentInParent<Player>();

        if (other.gameObject.name == Constants.Coin && player != null)
        {            
            player.UpdateHealth(-5f);//power up health                
            Destroy(other.gameObject);//destroy the coin after collection
        }
        
    }
}
