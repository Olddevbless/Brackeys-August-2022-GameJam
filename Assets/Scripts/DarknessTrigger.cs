using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessTrigger : MonoBehaviour
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
            StopCoroutine(GameManager.current.Notice(""));
            StartCoroutine( GameManager.current.Notice("Darkness comming, be redy to Bark (B)"));
            darknessController.MoveTowardsPlayer();
        }
    }
}
