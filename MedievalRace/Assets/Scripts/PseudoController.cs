using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Photon.Pun;

namespace Com.MyCompany.MyGame
{
    public class PseudoController : MonoBehaviourPunCallbacks, IPunObservable
{

    [Tooltip("The current Health of our player")]
    public int Health = 100;
    bool IsFiring = false;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject bulletPrefab;

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PseudoController.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs ();

        if (allowFire && IsFiring)
        {
            allowFire = false;
            CreateBullets();
        }
    }

    void CreateBullets()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        var bullet = PhotonNetwork.Instantiate(this.bulletPrefab.name, this.transform.position + Vector3.up*2, this.transform.rotation, 0);
        bullet.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);        

        Debug.Log("FIRE!");
        StartCoroutine(ResetFire());
        //StartCoroutine(DestroyBullet(bullet, 15));
    }

    /*IEnumerator DestroyBullet(GameManager go, float t)
    {
        yield return new WaitForSeconds(t);
        PhotonNetwork.Destroy(go);
    }*/


    void OnCollisionEnter(Collision col)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (col.gameObject.tag == "Bullet")
        {
            Destroy(col.gameObject);
            Health -= 10;
            print("health = " + Health);
            //TODO: spawn explosion
            //col.contacts[0].point;
        }
    }

    public int fire_cd = 5;

    IEnumerator ResetFire()
    {
        yield return new WaitForSeconds(fire_cd);

        IsFiring = false;
        allowFire = true;
    }

    void ProcessInputs()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        GetComponent<Rigidbody>().velocity = new  Vector3(h, 0, v)*3;

        if (Input.GetButtonDown("Fire1"))
        {
            if (allowFire)
            {
                IsFiring = true;
            }
        }
    }

    bool allowFire = true;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (int)stream.ReceiveNext();
        }
    }


    #endregion
}
}