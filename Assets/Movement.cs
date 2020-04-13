using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    float v, h;
    float cameraRot;
    float vel;

    


    bool active;
    bool move;
    bool shoot;
    bool gunOut = false;
    bool hasActivated = false;


    public Image Mov;
    public int MaxMov;

    private float initX;
    private float initZ;


    private float currX;
    private float currZ;

    private float distX;
    private float distZ;

    private float currDist;
    private float distPerc;

    Animator anim;
    GameObject gun;
    public GameObject balita;

    float bulletSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        active = false;
        move = false;
        this.initX = this.gameObject.transform.position.x;
        this.initZ = this.gameObject.transform.position.z;
        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;
        gun = this.transform.Find("Bip001").gameObject.transform.Find("Bip001 Pelvis").gameObject.transform.Find("Bip001 Spine").gameObject.transform.Find("Bip001 R Clavicle").gameObject.transform.Find("Bip001 R UpperArm").gameObject.transform.Find("Bip001 R Forearm").gameObject.transform.Find("Bip001 R Hand").gameObject.transform.Find("R_hand_container").gameObject.transform.Find("w_shotgun").gameObject;
                  
    }

    // Update is called once per frame
    void Update()
    {

        cameraRot = Input.GetAxis("Mouse X");

        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;

        this.distX = Mathf.Abs(currX-initX);
        this.distZ = Mathf.Abs(currZ-initZ);

        this.currDist = Mathf.Sqrt((distX*distX) + (distZ * distZ));

        if (active && move){
            if (MaxMov - currDist <= 0)
            {
                move = false;
            }
            else
            {
                distPerc = (float)currDist / MaxMov;
                Mov.fillAmount = distPerc;
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                vel=6;
            }else{
                vel=3;
            }
            v = Input.GetAxis("Horizontal");
            h = Input.GetAxis("Vertical"); 
               

            if(v == 0 && h == 0)
            {
                anim.SetFloat("v", 0);
            }
            else
            {
                anim.SetFloat("v", vel);
            }
            transform.Translate(v*Time.deltaTime*vel,0,h*Time.deltaTime*vel);
            transform.Rotate(0, cameraRot, 0);

            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("wave");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(!gunOut){
                   anim.SetTrigger("gunO"); 
                   gun.SetActive(true);
                    gunOut = true;
                    anim.SetBool("gunOut", true);
                }else{
                    gun.SetActive(false);
                    gunOut = false;
                    anim.SetBool("gunOut", false);
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                if(gunOut){
                GameObject disparo = Instantiate(balita,this.transform.Find("brazo").gameObject.transform.position,this.transform.Find("brazo").gameObject.transform.rotation);
                Rigidbody rigDisparo = disparo.GetComponent<Rigidbody>();
                rigDisparo.AddForce(transform.forward * bulletSpeed);
                Destroy(disparo,3);
                 }
                anim.SetTrigger("shoot");
            }
        }
    }

    public void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "bala"){
            Destroy(collisionInfo.gameObject);
            Debug.Log("Pum");
        }
    }

    public void activate(){
        active = true;
        move = true;
        if (!hasActivated)
        {
            hasActivated = true;
            initX = this.gameObject.transform.position.x;
            initZ = this.gameObject.transform.position.z;
            Mov.fillAmount = 0;
        }

        currX = this.gameObject.transform.position.x;
        currZ = this.gameObject.transform.position.z;
       
    }
    public void deactivate(){
        active = false;
        move = false;
    }
}
