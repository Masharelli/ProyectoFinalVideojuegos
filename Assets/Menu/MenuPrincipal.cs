using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Empezar(){
        SceneManager.LoadScene("SampleScene");
    }
    public void Cerrar(){
        Application.Quit();
        Debug.Log("Salir");
    }
}
