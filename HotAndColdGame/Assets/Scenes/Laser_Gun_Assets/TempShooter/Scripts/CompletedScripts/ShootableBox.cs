using UnityEngine;
using System.Collections;

public class ShootableBox : MonoBehaviour {

	public float temperature = 0;

    void Update()
    {
        print(temperature + " obj:" + this.name);

        if (temperature > 100)
        {
            temperature = 100;
        }

        if (temperature < -100)
        {
            temperature = -100;
        }

        if (temperature < -1)
        {
            temperature += 0.1f;
        }
        if (temperature > 1)
        {
            temperature -= 0.1f;
        }

        if (temperature <= 49 && temperature >= -49)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    public void Temperature(int change)
	{
        temperature += change;

        if (temperature <= -50) 
		{
			gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        if (temperature >= 50)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
