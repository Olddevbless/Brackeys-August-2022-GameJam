using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingNote : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(2);
    }
}
