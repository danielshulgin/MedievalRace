using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Photon.Pun;

public class CannonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public List<GameObject> leftCannons;
    public List<GameObject> rightCannons;
    public PanAndZoom panAndZoom;

    public GameObject cannonballPref;
    
    void Start()
    {
        if (photonView.IsMine == false || PhotonNetwork.IsConnected == false)
        {
            return;
        }
        panAndZoom = PanAndZoom.instance;
        panAndZoom.onTap += Fire;
    }

    public void Fire(Vector2 position)
    {
        if (position.x < Camera.main.pixelWidth  / 2)
        {
            foreach (GameObject cannon in leftCannons)
            {
                Instantiate(cannonballPref, cannon.transform);
            }
        }
        else
        {
            foreach (GameObject cannon in rightCannons)
            {
                Instantiate(cannonballPref, cannon.transform);
            }
        }

    }
}
