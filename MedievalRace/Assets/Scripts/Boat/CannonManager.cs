using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Photon.Pun;

public enum Side
{
    Left, Right
}

public class CannonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public List<GameObject> leftCannons;
    public List<GameObject> rightCannons;
    public PanAndZoom panAndZoom;

    #region LineRenderers
    [SerializeField]
    private LineRenderer lineRendererUpRight;
    [SerializeField]
    private LineRenderer lineRendererUpLeft;
    [SerializeField]
    private LineRenderer lineRendererDownRight;
    [SerializeField]
    private LineRenderer lineRendererDownLeft;
    #endregion

    #region AngleConstrains
    [Tooltip("contr clockwise, from OX")]
    [SerializeField]
    private float maxFireAngle = 45;
    [Tooltip("contr clockwise, from OX")]
    [SerializeField]
    private float minAngle = - 45;
    private Vector2 UR;
    private Vector2 DR;
    private Vector2 UL;
    private Vector2 DL;
    #endregion

    public GameObject cannonballPref;
    private int linePointCount = 20;

    void Start()
    {
        CalcuateAngleConstrains();
        if (photonView.IsMine == false || PhotonNetwork.IsConnected == false)
        {
            return;
        }
        panAndZoom = PanAndZoom.instance;
        panAndZoom.onTap += Fire;
        //panAndZoom.onStartTouch += 
    }

    public void CalcuateAngleConstrains()
    {
        UR = (Vector2)(Quaternion.Euler(0, 0, maxFireAngle) * Vector2.right);
        DR = (Vector2)(Quaternion.Euler(0, 0, minAngle) * Vector2.right);
        UL = (Vector2)(Quaternion.Euler(0, 0, 180 - maxFireAngle) * Vector2.right);
        DL = (Vector2)(Quaternion.Euler(0, 0, 180 - minAngle) * Vector2.right);
    }

    private void Update()
    {
        ChangeTrajectory(Side.Right, Vector3.right);
    }

    private void OnDestroy()
    {
        if (photonView.IsMine == true)
            panAndZoom.onTap -= Fire;
    }

    public void Fire(Vector2 position)
    {
        if (position.x < Camera.main.pixelWidth / 2)
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

    public void ShowTrajectory(Side side)
    {
        if (side == Side.Right)
        {
            lineRendererUpRight.enabled = lineRendererDownRight.enabled = true;
        }
        else
        {
            lineRendererUpLeft.enabled = lineRendererDownLeft.enabled = true;
        }
    }

    public void HideTrajectory(Side side)
    {
        if (side == Side.Right)
        {
            lineRendererUpRight.enabled = lineRendererDownRight.enabled = false;
        }
        else
        {
            lineRendererUpLeft.enabled = lineRendererDownLeft.enabled = false;
        }
    }

    public void ChangeTrajectory(Side side, Vector3 direction)
    {
        if (side == Side.Right)
        {
            ChangeTrajectory(lineRendererUpRight, lineRendererDownRight, direction);
        }
        else
        {
            ChangeTrajectory(lineRendererUpLeft, lineRendererDownLeft, direction);
        }
    }

    private void ChangeTrajectory(LineRenderer up, LineRenderer down, Vector3 direction)
    {
        Vector3[] upPositions = new Vector3[linePointCount];
        Vector3[] downPositions = new Vector3[linePointCount];
        Quaternion quternionDirection = Quaternion.Euler(direction);
        for (int i = 0; i < linePointCount; i++)
        {
            upPositions[i] = up.gameObject.transform.position +
                quternionDirection * new Vector3(i, Mathf.Sin(i), 0f);
        }
        up.positionCount = linePointCount;
        up.SetPositions(upPositions);
        down.positionCount = linePointCount;
        down.SetPositions(upPositions);
    }

}
