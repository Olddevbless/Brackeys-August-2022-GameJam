using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    [SerializeField]GameObject noteCanvas;
    
    [SerializeField] bool canvasBool;

    private void Start()
    {
        canvasBool = false;
        noteCanvas.SetActive(canvasBool);
    }
    public void ReadNote()
    {
        canvasBool = !canvasBool;
        noteCanvas.SetActive(canvasBool);
    }
}
