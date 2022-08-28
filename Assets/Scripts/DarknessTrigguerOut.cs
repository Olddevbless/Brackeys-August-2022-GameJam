using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessTrigguerOut : MonoBehaviour
{
    [SerializeField] DarknessController darknessController;
    private void Awake()
    {
        darknessController = DarknessController.FindObjectOfType<DarknessController>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && darknessController!=null)
        {
            darknessController.darknessSpeed =0;
            Destroy(darknessController.gameObject, 4);
        }
    }
}
