using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnd : MonoBehaviour
   
{
    public int temperature = 0;
    private int collisionCount;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider collision)
    {
        collisionCount = 1;
        ShootableBox objtemp = collision.GetComponent<ShootableBox>();

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Shootable")
        {
            if (objtemp != null)
            {
                objtemp.Temperature(temperature);
            }
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (collisionCount == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
