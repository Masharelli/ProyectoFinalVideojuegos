using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    public Text textOver;
    // Start is called before the first frame update
    void Start()
    {
        textOver.text = "GANADOR Equipo: "  + Winner.winner.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
