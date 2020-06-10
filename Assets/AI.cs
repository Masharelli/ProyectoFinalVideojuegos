using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3[] vectors;
    public bool medic;
    public bool soldado;
    public Text TextoProbabilidad;
    float probabilidadDeDsiparo;
    public GameObject turnController;
    float v, h;
    float vel;
    public GameObject enemigo;
    public Vector3 goal;
    public bool active;
    bool enemyDetected = false;
    bool move;
    bool hasActivated = false;
    int MaxMov = 20;
    bool shot = false;
    public Image Mov;
    Vector3 initgoal;
    public Image hpbar;

    public AudioSource SonidoDisparar;

    public float maxhp;
    public float hp;

    private float initX;
    private float initZ;

    private float currX;
    private float currZ;

    private float distX;
    private float distZ;

    private float currDist;
    private float distPerc;

    AudioSource sonidoDisparar;
    GameObject gun;
    UnityEngine.AI.NavMeshAgent agent;

    Animator anim;

    void Start()
    {

        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("soldierP1");
        float dist = 1000;
        GameObject closest = null;
        foreach(GameObject soldier in enemigos)
        {
            if (Vector3.Distance(soldier.transform.position, transform.position) <  dist)
            {
                dist = Vector3.Distance(soldier.transform.position, transform.position);
                closest = soldier;
            }
        }
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Vector3 pos = closest.GetComponent<Movement>().getTransform();
        goal = pos;
        sonidoDisparar = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        active = false;
        move = true;
        this.initX = this.gameObject.transform.position.x;
        this.initZ = this.gameObject.transform.position.z;
        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;

        gun = this.transform.Find("Bip001").gameObject.transform.Find("Bip001 Pelvis").gameObject.transform.Find("Bip001 Spine").gameObject.transform.Find("Bip001 R Clavicle").gameObject.transform.Find("Bip001 R UpperArm").gameObject.transform.Find("Bip001 R Forearm").gameObject.transform.Find("Bip001 R Hand").gameObject.transform.Find("R_hand_container").gameObject.transform.Find("w_shotgun").gameObject;
        
        this.vectors = new Vector3[8];


    }

    // Update is called once per frame
    void Update()
    {
        if (this.hp <= 0)
        {
            anim.SetTrigger("dead");
            turnController.GetComponent<Controller>().deatht2++;
            Destroy(this.gameObject);
            turnController.GetComponent<Controller>().refreshTeams();
        }
        else if (this.hp > 0 && this.hp <= 75)
        {
            GameObject[] healthpacks = GameObject.FindGameObjectsWithTag("healthpack");
            float dist = 1000;
            GameObject closest = null;
            foreach (GameObject soldier in healthpacks)
            {
                if (Vector3.Distance(soldier.transform.position, transform.position) < dist)
                {
                    dist = Vector3.Distance(soldier.transform.position, transform.position);
                    closest = soldier;
                }
            }
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (closest != null)
            {
                Vector3 pos = closest.transform.position;
                goal = pos;
            }
            

            Debug.DrawRay(this.transform.position + new Vector3(0, 1f, 0), transform.forward * 60, Color.green);
            this.currX = this.gameObject.transform.position.x;
            this.currZ = this.gameObject.transform.position.z;

            this.distX = Mathf.Abs(currX - initX);
            this.distZ = Mathf.Abs(currZ - initZ);

            this.currDist = Mathf.Sqrt((distX * distX) + (distZ * distZ));
            if (active && move)
            {
                agent.destination = goal;
                anim.SetFloat("v", 3);
                hpbar.fillAmount = ((this.hp * 100) / maxhp) / 100;
                if (MaxMov - currDist <= 0)
                {
                    move = false;
                    agent.isStopped = true;
                    anim.SetFloat("v", 0);
                    this.deactivate();

                }
                else
                {
                    distPerc = (float)currDist / MaxMov;
                    Mov.fillAmount = distPerc;
                    agent.isStopped = false;
                }
            }
        }
        else
        {
            Debug.DrawRay(this.transform.position + new Vector3(0, 1f, 0), transform.forward * 60, Color.green);
            this.currX = this.gameObject.transform.position.x;
            this.currZ = this.gameObject.transform.position.z;

            this.distX = Mathf.Abs(currX - initX);
            this.distZ = Mathf.Abs(currZ - initZ);

            this.currDist = Mathf.Sqrt((distX * distX) + (distZ * distZ));
            if (active && move)
            {
                agent.destination = goal;
                anim.SetFloat("v", 3);
                hpbar.fillAmount = ((this.hp * 100) / maxhp) / 100;
                if (MaxMov - currDist <= 0)
                {
                    move = false;
                    agent.isStopped = true;
                    anim.SetFloat("v", 0);

                    this.deactivate();

                }
                else
                {
                    distPerc = (float)currDist / MaxMov;
                    Mov.fillAmount = distPerc;
                    agent.isStopped = false;
                }
                //print(Vector3.Distance(agent.destination, transform.position));
                if ((Vector3.Distance(agent.destination, transform.position) < 10f))
                {
                    anim.SetTrigger("gunO");
                    Buscar();
                    transform.rotation = Quaternion.LookRotation(goal - transform.position, Vector3.up);
                    agent.isStopped = true;
                    print("Ahi ta prro");
                    
                    anim.SetBool("gunOut",true);
                    //SonidoDisparar.Play();
                    RaycastHit hit;

                    if (!shot)
                    {
                        print("Disparo");
                        if (Physics.Raycast(this.transform.position + new Vector3(0, 1f, 0), this.transform.forward, out hit, 15))
                        {
                            if (hit.transform.gameObject.tag.Equals("soldierP1"))
                            {
                                probabilidadDeDsiparo = UnityEngine.Random.Range(0, 5) + hit.distance;

                                float disparoImpreso = 100 - probabilidadDeDsiparo * 10;
                                SonidoDisparar.Play();
                                if (disparoImpreso >= 0)
                                {
                                    TextoProbabilidad.text = "Probabilidad de impacto: " + disparoImpreso;
                                }
                                else
                                {
                                    TextoProbabilidad.text = "Probabilidad de impacto: " + 0;

                                }
                                if (probabilidadDeDsiparo < 10)
                                {
                                    Debug.Log("Disparo acertado a " + hit.transform.gameObject.tag);
                                    Animator hitAnim = hit.transform.GetComponent<Animator>();
                                    if (hit.transform.gameObject.GetComponent<Movement>().hp > 0)
                                        hit.transform.gameObject.GetComponent<Movement>().hp -= 50;
                                    hitAnim.SetTrigger("damage");


                                }
                                else
                                {
                                    Debug.Log("Disparo fallado");
                                }
                            }
                        }
                        shot = true;
                        anim.SetBool("gunOut", false);
                    }
                    

                    this.deactivate();
                }
                if ((this.transform.position == agent.destination) && !enemyDetected)
                {
                    print("On ta?");
                }
            }
        }


    }
    public void activate()
    {

        active = true;
        move = true;
        initX = this.gameObject.transform.position.x;
        initZ = this.gameObject.transform.position.z;
        currX = this.gameObject.transform.position.x;
        currZ = this.gameObject.transform.position.z;
        if (!enemyDetected)
        {
            StartCoroutine(Buscar());
        }
    }
    public void deactivate()
    {
        turnController.GetComponent<Controller>().ChangeTeamTurn();
        print("Desactivado");
        shot = false;
        active = false;
        move = false;
        agent.isStopped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "healthpack")
        {
            this.hp = this.hp + 50;
            if (this.hp > maxhp)
            {
                this.hp = maxhp;
            }
            Destroy(other.gameObject);
        }
    }

    public IEnumerator Buscar()
    {
        float startRot = transform.eulerAngles.y;
        float endRotation = startRot + 360.0f;
        float t = 0.0f;

        while (t < 3)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRot, endRotation, t / 3) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            Vector3 fwd = this.transform.TransformDirection(Vector3.forward);
            Quaternion viewl = Quaternion.AngleAxis(-15, new Vector3(0, 1, 0));
            Quaternion viewr = Quaternion.AngleAxis(15, new Vector3(0, 1, 0));
            Quaternion viewl2 = Quaternion.AngleAxis(-10, new Vector3(0, 1, 0));
            Quaternion viewr2 = Quaternion.AngleAxis(10, new Vector3(0, 1, 0));
            Quaternion viewt = Quaternion.AngleAxis(5, new Vector3(0, 0, 1));
            Quaternion viewb = Quaternion.AngleAxis(-5, new Vector3(0, 0, 1));

            vectors[0] = viewl * transform.forward;
            vectors[1] = viewl * transform.forward;
            vectors[2] = viewr * transform.forward;
            vectors[3] = viewl2 * transform.forward;
            vectors[4] = viewr2 * transform.forward;
            vectors[5] = viewt * transform.forward;
            vectors[6] = viewb * transform.forward;
            vectors[7] = fwd;

            foreach (Vector3 vec in this.vectors)
            {
                Debug.DrawRay(this.transform.position + new Vector3(0, 1f, 0), vec * 60, Color.green);
                RaycastHit hit;

                if (Physics.Raycast(this.transform.position + new Vector3(0, 1f, 0), vec, out hit, 60))
                {

                    if (hit.collider.tag.Equals("soldierP1"))
                    {
                        print("Enemy Detected");
                        enemyDetected = true;
                        goal = hit.transform.position;
                    }
                    else
                    {
                        //print("Enemy not Detected");
                        enemyDetected = false;
                    }
                }
            }
        }
        yield return null;
    }
}
