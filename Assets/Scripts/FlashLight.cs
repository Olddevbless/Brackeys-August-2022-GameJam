using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] float maxbatteryLife;
    [Range(-3,7)] public float currentBatteryLife;
    [SerializeField] bool lightOn;
    Light lightEmission;
    [SerializeField] GameObject lightCollider;
    [SerializeField] float lightColliderX;
    [SerializeField] float lightColliderScaleZ;
    void Start()
    {
        lightColliderX = lightCollider.transform.localPosition.z;
        lightColliderScaleZ = lightCollider.transform.localScale.z;
        lightEmission = gameObject.GetComponent<Light>();
        currentBatteryLife = maxbatteryLife;
    }

    // Update is called once per frame
    void Update()
    {
        lightCollider.transform.localPosition = new Vector3(lightCollider.transform.localPosition.x, lightCollider.transform.localPosition.y, lightColliderX);
        lightCollider.transform.localScale = new Vector3(lightCollider.transform.localScale.x, lightCollider.transform.localScale.y, lightColliderScaleZ);
        if (lightOn)
        {
            if(currentBatteryLife>0)
            {
                currentBatteryLife = currentBatteryLife - Time.deltaTime;
                if (lightColliderX>0)
                {
                    lightColliderScaleZ -= Time.deltaTime * 500;
                    lightColliderX -= Time.deltaTime *4f ;
                }
                
            }
            lightEmission.intensity = currentBatteryLife;
            if (currentBatteryLife == 0)
            {
                ToggleLight();
                
            }

        }
        else
        {
            if (currentBatteryLife < 7)
            {
                lightColliderScaleZ += Time.deltaTime * 500;
                lightColliderX += Time.deltaTime * 4f;
                currentBatteryLife = currentBatteryLife + Time.deltaTime;

            }
            lightEmission.intensity = 0;
        }
    }
    public void ToggleLight()
    {
        
        lightOn = !lightOn;
        lightCollider.SetActive(lightOn);
    }
   
}
