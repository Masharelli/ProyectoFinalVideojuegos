using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    GameObject[] soldiers;
    Camera[] cameras;
    GameObject active;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        soldiers = GameObject.FindGameObjectsWithTag("soldier");
        cameras = new Camera[soldiers.Length];
        for(int i = 0; i<soldiers.Length; i++){
            cameras[i] = soldiers[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }
        for(int i = 0; i<cameras.Length; i++)
        {
            if(i != 0)
            {
                cameras[i].enabled = false;
            }
        }
        active = soldiers[soldiers.Length-1];
        soldiers[soldiers.Length - 1].GetComponent<Movement>().activate();
        cameras[soldiers.Length - 1].enabled = true;
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            for(int i = 0; i<10; i++)
            {
                if (Input.GetKeyDown("" + i))
                {
                    if(i <= soldiers.Length) {
                        active.GetComponent<Movement>().deactivate();
                        active = soldiers[i - 1];
                        soldiers[i - 1].GetComponent<Movement>().activate();
                        cameras[i - 1].enabled = true;
                        for (int j = 0; j < cameras.Length; j++)
                        {
                            if (j != i - 1)
                            {
                                cameras[j].enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
