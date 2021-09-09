using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRod_FXController : FXController
{
    public ChargerFXController[] crystalChargers;
    //public ChargerFXController[] crystals;

    public Transform LightningEndL;
    public Transform LightningEndR;

   /* public Transform LightningEndL_origin;
    public Transform LightningEndR_origin;

    public Transform LightningEndL_target;
    public Transform LightningEndR_target;*/

    public float minTriggerTime;
    public float maxTriggerTime;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        StartCoroutine(RandomLightningHit());


    }

    // Update is called once per frame
    void Update()
    {
        base.PerformFX();
    }


    IEnumerator RandomLightningHit()
    {
        while (true)
        {
            int crystalToHit = Random.Range(0, 2);
            crystalChargers[crystalToHit].isEnabled = true;

            if (crystalToHit == 0)
                LightningEndL.gameObject.SetActive(true);
            else
                LightningEndR.gameObject.SetActive(true);

            Invoke(nameof(StopLightningHit), 0.2f);
            // Do the thing
            yield return new WaitForSeconds(Random.Range(minTriggerTime, maxTriggerTime));
        }
    }

    void StopLightningHit()
    {
        foreach (var item in crystalChargers)
        {
            item.isEnabled = false;
            LightningEndL.gameObject.SetActive(false);
            LightningEndR.gameObject.SetActive(false);
        }
    }
}
