using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Photon.Pun;

public class BoatEngine : MonoBehaviourPunCallbacks, IPunObservable
{
    public PanAndZoom panAndZoom;

    public float speed = 1f;
    public float controlForce = 100f;
    public float maxVelocity = 0.6f;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance = null;
    [SerializeField]
    private float maxAngularVelocity = 20f;

    private void Awake()
    {
        panAndZoom = PanAndZoom.instance;

        if (BoatEngine.LocalPlayerInstance != null && photonView.IsMine)
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Destroy(this.gameObject);
                return;
            }
        }

        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            BoatEngine.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (photonView.IsMine == true && LocalPlayerInstance == this.gameObject)
        {
            panAndZoom = PanAndZoom.instance;

            panAndZoom.onSwipe += Control;
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
        
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == true && LocalPlayerInstance == this.gameObject)
        {
            Move();
        }        
    }

    public void Move()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > maxVelocity)
        {
            gameObject.GetComponent<Rigidbody>().velocity = maxVelocity * gameObject.GetComponent<Rigidbody>().velocity.normalized;
        }
    }

    public void Control(Vector2 direction)
    {
        Vector3 recalculatedDirection = new Vector3(0f, direction.x * controlForce, 0f) * Time.deltaTime;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddTorque(recalculatedDirection);
        if (rb.angularVelocity.magnitude > maxAngularVelocity)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxAngularVelocity;
        }
    }

    public void OnDestroy()
    {
        panAndZoom.onSwipe -= Control;

        if (BoatEngine.LocalPlayerInstance == this.gameObject && photonView.IsMine)
        {
            BoatEngine.LocalPlayerInstance = null;
            //panAndZoom.onSwipe = null;
            
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
