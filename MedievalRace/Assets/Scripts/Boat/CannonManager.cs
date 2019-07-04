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

    #region Constrains
    [Tooltip("contr clockwise, from OX")]
    [SerializeField]
    private float maxFireAngle = 45;
    [Tooltip("contr clockwise, from OX")]
    [SerializeField]
    private float minAngle = - 45;
    #endregion

    public GameObject cannonballPref;
    private int linePointCount = 20;

    void Start()
    {
        ChangeTrajectory(Side.Right, Vector3.right);
        if (photonView.IsMine == false || PhotonNetwork.IsConnected == false)
        {
            return;
        }
        panAndZoom = PanAndZoom.instance;
        panAndZoom.onTap += Fire;
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
        for (int i = 0; i < linePointCount; i++)
        {
            upPositions[i] = up.gameObject.transform.position +
                Quaternion.Euler(direction) * new Vector3(i, Mathf.Sin(i), 0f);
        }
        up.positionCount = linePointCount;
        up.SetPositions(upPositions);
        down.positionCount = linePointCount;
        down.SetPositions(upPositions);
    }

}
