using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;

namespace com.cloudpop
{
    public class GunController : MonoBehaviour
    {
        #region Variables

        public float damage = 2f, range = 100f, fireRate = 0.5f;
        public Camera fpsCam;
        private ParticleSystem bulletEffect;

        private float nextTimeToFire = 10f;
        private bool shooting = false;

        private float coolDown;

        private Player player;

        private float number = 2f;

        private PhotonView PV;

        public Gun[] gunSelection;
        public Transform weaponParent;
        private GameObject currentWeapon;

        private int currentIndex = 0;

        public LayerMask canBeShot;

        private bool isReloading;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            // InvokeRepeating("Shoot", 0.0f, 1.0f / nextTimeToFire);

            player = GameObject.Find("Player(Clone)").GetComponent<Player>();
            PV = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();

            //PV.RPC("Equip", RpcTarget.All, 0);
            //Equip(0);
            bulletEffect = GameObject.Find("Bullet Effect").GetComponent<ParticleSystem>();

            foreach (Gun a in gunSelection)
            {
                a.Initialize();
                Equip(0);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            //{
            //    nextTimeToFire = Time.time + 1f / fireRate;
            //    Shoot();
            //}
            //    shooting = false;
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    shooting = true;
            //    PV.RPC("Shoot", RpcTarget.All);
            //}

            if (Pause.paused && PV.IsMine)
            {
                return;
            }

            if (PV.IsMine && Input.GetKeyDown(KeyCode.Alpha1))
            {
                PV.RPC("Equip", RpcTarget.All, 0);
            }

            if (currentWeapon != null)
            {
                if (PV.IsMine)
                {
                    if (Input.GetButtonDown("Fire1") && coolDown <= 0)
                    {
                        if (gunSelection[currentIndex].FireBullet())
                        {
                            PV.RPC("Shoot", RpcTarget.All);
                        }
                        else
                            StartCoroutine(Reload(gunSelection[currentIndex].reload));
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                        StartCoroutine(Reload(gunSelection[currentIndex].reload));


                    if (coolDown > 0)
                        coolDown -= Time.deltaTime;
                }
            }

            number = Random.Range(0, 3);

            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    Equip(0); 
            //}
        }

        IEnumerator Reload(float waitTime)
        {
            isReloading = true;
            yield return new WaitForSeconds(waitTime);
            gunSelection[currentIndex].Reload();
            isReloading = false;
        }

        [PunRPC] //called by other machines on network
        private void Shoot()
        {
            //if (!shooting)
            //    return;

            bulletEffect.Play();

            //bloom
            Vector3 gunBloom = fpsCam.transform.position + fpsCam.transform.forward * 1000f;
            gunBloom += Random.Range(-gunSelection[currentIndex].bloom, gunSelection[currentIndex].bloom) * fpsCam.transform.up;
            gunBloom += Random.Range(-gunSelection[currentIndex].bloom, gunSelection[currentIndex].bloom) * fpsCam.transform.right;
            gunBloom -= fpsCam.transform.position;
            gunBloom.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, gunBloom, out hit, range))
            {
                if (PV.IsMine)
                {
                    //shooting other player on network
                    if (hit.collider.gameObject.layer == 9)
                    {
                        //RPC call to damage player

                    }

                    Debug.Log(hit.transform.name);
                    if (hit.transform.name == "Wall")
                    { }
                    else if (hit.transform.name == "Wall (1)")
                    { }
                    else if (hit.transform.name == "Wall (2)")
                    { }
                    else if (hit.transform.name == "Wall (3)")
                    { }
                    else if (hit.transform.name == "Floor")
                    { }
                    else
                    {
                        if (hit.transform.name == "Cloud_Cumulus_Fluffy") //lightning cloud effect
                        {
                            damage = 7f;
                            PV.RPC("TakeDamage", RpcTarget.All, damage);
                            Debug.Log(damage);
                        }
                        else if (hit.transform.name == "Cloud_Cumulus_Med") //rain cloud effect
                        {
                            damage = (damage * 2);
                            PV.RPC("TakeDamage", RpcTarget.All, damage);
                            Debug.Log(damage);
                        }
                        else if (hit.transform.name == "Cloud_Strato") //snow cloud effect
                        {
                            float slowSpeed = player.speed / 2;
                            player.speed = slowSpeed;
                        }
                        else if (hit.transform.name == "Cloud_Cumulus")
                        {
                            switch (number)
                            {
                                case 0:
                                    damage = 5f;
                                    PV.RPC("TakeDamage", RpcTarget.All, damage);
                                    Debug.Log(damage);
                                    break;
                                case 1:
                                    nextTimeToFire = 1.5f;
                                    break;
                                case 2:
                                    player.health += 5f;
                                    break;

                            }
                        }
                        Destroy(hit.transform.gameObject); //destroy gameObject hit
                    }
                }
            }
            coolDown = gunSelection[currentIndex].fireRate;
        }

       [PunRPC]
        public void Equip(int index)
        {
            if (currentWeapon != null)
            {
                if (isReloading) 
                    StartCoroutine("Reload");
               
                Destroy(currentWeapon);
            }

            currentIndex = index;

            GameObject equiped = Instantiate(gunSelection[index].prefab, weaponParent.position, weaponParent.rotation, weaponParent);
            equiped.transform.localPosition = Vector3.zero;
            equiped.transform.localEulerAngles = Vector3.zero;

            currentWeapon = equiped;
        }

        [PunRPC]
        private void TakeDamage(float dmg)
        {
            GameObject.Find("Player(Clone)").GetComponent<Player>().TakeDamage(dmg);
        }

        public void RefreshAmmo(Text ammo)
        {
            int clip = gunSelection[currentIndex].GetClip();
            int ammoAmount = gunSelection[currentIndex].GetAmmo();

            ammo.text = clip.ToString("D2") + " / " + ammoAmount.ToString("D2");
        }
    }
}