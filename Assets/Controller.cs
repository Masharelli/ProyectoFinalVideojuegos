using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public int deatht1 = 0;
    public int deatht2 = 0;
    int currTeamTurn = 0;
    public bool turnchange = false;
    GameObject[] soldiersP1;
    Camera[] camerasP1;
    GameObject[] soldiersP2;
    Camera[] camerasP2;
    GameObject active;
    int currTurn = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        soldiersP1 = GameObject.FindGameObjectsWithTag("soldierP1");

        camerasP1 = new Camera[soldiersP1.Length];
        for (int i = 0; i < soldiersP1.Length; i++)
        {
            camerasP1[i] = soldiersP1[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }
        for (int i = 0; i < camerasP1.Length; i++)
        {
            if (i != 0)
            {
                camerasP1[i].enabled = false;
            }
        }

        soldiersP2 = GameObject.FindGameObjectsWithTag("soldierP2");
        camerasP2 = new Camera[soldiersP2.Length];

        for (int i = 0; i < soldiersP2.Length; i++)
        {
            camerasP2[i] = soldiersP2[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }
        for (int i = 0; i < camerasP2.Length; i++)
        {
            if (i != 0)
            {
                camerasP2[i].enabled = false;
            }
        }



        active = soldiersP1[soldiersP1.Length - 1];
        soldiersP1[soldiersP1.Length - 1].GetComponent<Movement>().activate();
        camerasP1[soldiersP1.Length - 1].enabled = true;
    }

    //Update is called once per frame
    void Update()
    {
        print(soldiersP1.Length);
        if(deatht1 == 2)
        {
            Winner.winner = 2;
            SceneManager.LoadScene("end");
        }
        else if(deatht2 == 2)
        {
            Winner.winner = 1;
            SceneManager.LoadScene("end");
        }
        if(currTeamTurn >= soldiersP2.Length)
        {
            if(soldiersP2.Length!=0)
            turnchange = true;
            currTeamTurn = 0;
            print("Fin de turno npc");
        }
        if (turnchange == true)
        {
            refreshTeams();
            if (currTurn % 2 == 0)
            {
                //active.GetComponent<Movement>().deactivate();
                foreach (Camera cam in camerasP1)
                {
                    cam.enabled = false;
                }
               for(int i = 0; i<soldiersP2.Length; i++)
                {
                    active = soldiersP2[i];
                    active.GetComponent<AI>().activate();
                    camerasP2[i].enabled = true;
                }
                
            }
            else
            {
                //active.GetComponent<AI>().deactivate();
                foreach (Camera cam in camerasP2)
                {
                    cam.enabled = false;
                }
                active = soldiersP1[0];
                active.GetComponent<Movement>().activate();
                camerasP1[0].enabled = true;
            }
            currTurn++;
            turnchange = false;
        }
        if (currTurn % 2 == 0)
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Input.GetKeyDown("" + i))
                    {
                        if (i <= soldiersP1.Length)
                        {
                            
                            active.GetComponent<Movement>().deactivate();
                            
                            active = soldiersP1[i - 1];
                            soldiersP1[i - 1].GetComponent<Movement>().activate();
                            camerasP1[i - 1].enabled = true;
                            for (int j = 0; j < camerasP1.Length; j++)
                            {
                                if (j != i - 1)
                                {
                                    camerasP1[j].enabled = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void refreshTeams()
    {
        soldiersP1 = GameObject.FindGameObjectsWithTag("soldierP1");

        camerasP1 = new Camera[soldiersP1.Length];
        for (int i = 0; i < soldiersP1.Length; i++)
        {
            camerasP1[i] = soldiersP1[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }

        soldiersP2 = GameObject.FindGameObjectsWithTag("soldierP2");
        camerasP2 = new Camera[soldiersP2.Length];

        for (int i = 0; i < soldiersP2.Length; i++)
        {
            camerasP2[i] = soldiersP2[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }
    }
    public void ChangeTeamTurn() { 
        currTeamTurn++;
        print(currTeamTurn);
    }
    public void ChangeTurn()
    {
        this.turnchange = true;
    }
    public void resetT1()
    {
        foreach(GameObject soldier in soldiersP1)
        {
            soldier.GetComponent<Movement>().hasActivated = false;
        }
    }
}
