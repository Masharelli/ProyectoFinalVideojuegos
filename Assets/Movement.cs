using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    float v, h;
    float cameraRot;
    float vel;


    public bool medic;

    public bool soldado;
    bool active;
    bool move;
    bool shot;
    bool gunOut = false;
    public bool hasActivated = false;


    public Image Mov;
    public int MaxMov;

    public Image hpbar; 
    
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

    public AudioSource SonidoDisparar;
    int healcount = 0;
    Animator anim;
    GameObject gun;
    Camera camera;
    public GameObject healthpack;
    private int ContadorDisparos;
    private bool PowerUp;
    public RawImage aimcross;
    public RawImage etiquetaJugador;
    public RawImage etiquetapowerUp;

    public Text TextoProbabilidad;
    public RawImage etiquetaSoldado;

    float probabilidadDeDsiparo;
    public GameObject turnController;


    // Start is called before the first frame update
    void Start()
    {
        SonidoDisparar = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        active = false;
        move = false;
        this.initX = this.gameObject.transform.position.x;
        this.initZ = this.gameObject.transform.position.z;
        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;
        gun = this.transform.Find("Bip001").gameObject.transform.Find("Bip001 Pelvis").gameObject.transform.Find("Bip001 Spine").gameObject.transform.Find("Bip001 R Clavicle").gameObject.transform.Find("Bip001 R UpperArm").gameObject.transform.Find("Bip001 R Forearm").gameObject.transform.Find("Bip001 R Hand").gameObject.transform.Find("R_hand_container").gameObject.transform.Find("w_shotgun").gameObject;
        camera = this.transform.Find("cam").gameObject.GetComponent<Camera>();    
        aimcross.enabled = false;
        etiquetaJugador.enabled = false;
        etiquetaSoldado.enabled = false;
        etiquetapowerUp.enabled = false;
        PowerUp = true;
        ContadorDisparos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.hp <= 0)
        {
            anim.SetTrigger("dead");
            turnController.GetComponent<Controller>().deatht2++;
            Destroy(this.gameObject);
            turnController.GetComponent<Controller>().refreshTeams();

        }
        cameraRot = Input.GetAxis("Mouse X");

        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;

        this.distX = Mathf.Abs(currX-initX);
        this.distZ = Mathf.Abs(currZ-initZ);

        this.currDist = Mathf.Sqrt((distX*distX) + (distZ * distZ));


        if (!shot) {
            if (active && move)
            {

                hpbar.fillAmount = ((this.hp * 100) / maxhp) / 100;
                if (MaxMov - currDist <= 0)
                {
                    move = false;
                }
                else
                {
                    distPerc = (float)currDist / MaxMov;
                    Mov.fillAmount = distPerc;
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    vel = 6;
                }
                else
                {
                    vel = 3;
                }
                v = Input.GetAxis("Horizontal");
                h = Input.GetAxis("Vertical");


                if (v == 0 && h == 0)
                {
                    anim.SetFloat("v", 0);
                }
                else
                {
                    anim.SetFloat("v", vel);
                }
                transform.Translate(v * Time.deltaTime * vel, 0, h * Time.deltaTime * vel);
                transform.Rotate(0, cameraRot, 0);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    anim.SetTrigger("wave");
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (!gunOut)
                    {
                        anim.SetTrigger("gunO");
                        gun.SetActive(true);
                        aimcross.enabled = true;

                        gunOut = true;
                        anim.SetBool("gunOut", true);
                    }
                    else
                    {
                        gun.SetActive(false);
                        aimcross.enabled = false;

                        gunOut = false;
                        anim.SetBool("gunOut", false);
                    }
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    if (medic == true)
                    {
                        if (healcount < 3)
                        {
                            Instantiate(healthpack, this.transform.position + this.transform.forward * 1f, this.transform.rotation);
                            healcount++;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    if (gunOut)
                    {
                        var ray = camera.ScreenPointToRay(Input.mousePosition);
                        SonidoDisparar.Play();
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.tag.Equals("soldierP2") || hit.collider.tag.Equals("soldierP1"))
                            {
                                probabilidadDeDsiparo = Random.Range(2, 8) + hit.distance;
                                ContadorDisparos++;
                                if (ContadorDisparos == 2)
                                {
                                    etiquetapowerUp.enabled = true;
                                }
                                else
                                {
                                    etiquetapowerUp.enabled = false;
                                }
                                if (ContadorDisparos == 3)
                                {
                                    PowerUp = true;
                                    Debug.Log("powerUp");
                                    probabilidadDeDsiparo = 0;
                                    ContadorDisparos = 0;
                                }
                                float disparoImpreso = 100 - probabilidadDeDsiparo * 10;
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
                                    if (hit.transform.gameObject.GetComponent<AI>().hp > 0)
                                        hit.transform.gameObject.GetComponent<AI>().hp -= 50;
                                    hitAnim.SetTrigger("damage");
                                }
                                else
                                {
                                    Debug.Log("Disparo fallado");
                                }
                            }
                        }
                    }
                    anim.SetTrigger("shoot");
                    move = false;
                    shot = true;
                }
                var rayEnemigo = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit enemigo;

                if (Physics.Raycast(rayEnemigo, out enemigo))
                {
                    if (enemigo.collider.tag.Equals("soldierP2"))
                    {
                        aimcross.color = Color.red;
                    }
                    else
                    {
                        aimcross.color = Color.black;
                    }
                }

                if (this.medic == true)
                {
                    etiquetaJugador.enabled = true;
                }
                else
                {
                    etiquetaJugador.enabled = false;
                }

                if (this.soldado == true)
                {
                    etiquetaSoldado.enabled = true;
                }
                else
                {
                    etiquetaSoldado.enabled = false;
                }
            }
        }
        
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                turnController.GetComponent<Controller>().resetT1();
                turnController.GetComponent<Controller>().ChangeTurn();
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "healthpack")
        {
            this.hp = this.hp + 50;
            if(this.hp > maxhp)
            {
                this.hp = maxhp;
            }
            Destroy(other.gameObject);
        }
    }
    public void activate(){
        
        active = true;
        move = true;
        if (!hasActivated)
        {
            shot = false;
            hasActivated = true;
            initX = this.gameObject.transform.position.x;
            initZ = this.gameObject.transform.position.z;
            Mov.fillAmount = 0;
        }
        //print(this.hp);
        currX = this.gameObject.transform.position.x;
        currZ = this.gameObject.transform.position.z;
       
    }
    public void deactivate(){
        active = false;
        move = false;
    }
    public Vector3 getTransform()
    {
        return this.transform.position;
    }
}
