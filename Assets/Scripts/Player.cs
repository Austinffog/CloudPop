using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace com.cloudpop //a class referred to by using as a prefix
{

    public class Player : MonoBehaviourPunCallbacks
    {
        #region Variables
        private PhotonView PV;

        public CharacterController controller;
        public float speed = 10f;
        public float speedBoost = 0.5f;
        private float gravity = -9.81f, jumpHeight = 1.5f, groundDistance = 0.4f;
        public Transform groundCheck;
        public LayerMask groundMask;
        private Vector3 velocity;
        private bool isGrounded;

        public float health = 100f;
        private GameObject gun;

        public Transform weaponParent;
        private Vector3 weaponParentOrigin, weaponParentBreathingPosition;
        private float idleCounter, movementCounter;

        public GameObject cameraParent, weapon;

        private GameManager gameManager;
        private Transform healthBar;
        private GunController gunController;
        private Text ammoText;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            PV = GetComponent<PhotonView>();
            if (!PV.IsMine)
            {
                gameObject.layer = 9;
            }

            cameraParent.SetActive(PV.IsMine);
            weapon.SetActive(PV.IsMine);

            gunController = GameObject.Find("Weapon").GetComponent<GunController>();
            groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();

            //gun = (GameObject)Resources.Load("Prefabs/Guns", typeof(GameObject));
            //Instantiate(gun, new Vector3(0f, 0f, 0f), gameObject.transform.rotation);

            if (Camera.main)
                Camera.main.enabled = false;

            weaponParentOrigin = weaponParent.localPosition;

            if (PV.IsMine)
            {
                healthBar = GameObject.Find("HUD/Health/HealthBar").transform;
                ammoText = GameObject.Find("HUD/AmmoCounter/Ammo").GetComponent<Text>();
                RefreshHealthBar();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!PV.IsMine)
                return;

            Movement();

            RefreshHealthBar();
            gunController.RefreshAmmo(ammoText);

            bool pause = Input.GetKeyDown(KeyCode.Escape);

            //pause
            if (pause)
            {
                GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
            }
            if (Pause.paused)
            {
                pause = false;
            }

        }

        void Movement()
        {
            //float moveSpeed = 0.5f;
            //float v = Input.GetAxis("Vertical");
            //float h = Input.GetAxis("Horizontal");
            //transform.position += new Vector3(h * moveSpeed, 0 , v * moveSpeed);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;

            //sprint
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool isSprinting = sprint && z > 0 && isGrounded;

            float sprintSpeed = speed;
            if (isSprinting)
                sprintSpeed *= speedBoost;

            controller.Move(move * sprintSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            //idle Breathing
            if (x == 0 && z == 0)
            {
                Breathing(idleCounter, 0.05f, 0.05f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, weaponParentBreathingPosition, Time.deltaTime * 2f);
            }
            else if (!isSprinting) //walking
            {
                Breathing(movementCounter, 0.1f, 0.1f);
                movementCounter += Time.deltaTime * 2f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, weaponParentBreathingPosition, Time.deltaTime * 5f);
            }
            else //running
            {
                Breathing(movementCounter, 0.1f, 0.1f);
                movementCounter += Time.deltaTime * 3f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, weaponParentBreathingPosition, Time.deltaTime * 8f);
            }

            if (Pause.paused)
            {
                x = 0f;
                z = 0f;
                isGrounded = false;
                isSprinting = false;
            }

        }

        void Breathing(float z, float xIntensity, float yIntensity)
        {
            weaponParentBreathingPosition = weaponParentOrigin + new Vector3(Mathf.Cos(z) * xIntensity, Mathf.Sin(z * 2) * yIntensity, 0);
        }

        public void TakeDamage(float dmg)
        {
            if (PV.IsMine)
            {
                health -= dmg;
                RefreshHealthBar();
                Debug.Log(health);

                if (health <= 0)
                {
                    gameManager.Spawn();
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        private void RefreshHealthBar()
        {
            float healthPercentage = health / 100.00f;
            healthBar.localScale = Vector3.Lerp(healthBar.localScale, new Vector3(healthPercentage, 1, 1), Time.deltaTime * 8f);
        }
    }
}
