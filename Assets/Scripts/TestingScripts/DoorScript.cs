
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorScript : MonoBehaviour
{
	public GameObject m_PlayerView;

	private float m_PickUpDistance = 3f;
	private float m_Distance = 3f;
	private float m_MaxDistanceGrab = 4f;

	private GameObject m_TargetObject;
	public float m_ThrowStrength = 50f;
	private bool mIsHolding;
	private bool m_PickingUp;

	//======================================

	public float m_DoorPickupRange = 2f;
	public float m_DoorThrow = 10f;
	public float m_DoorDistance = 2f;
	public float m_DoorMaxGrab = 3f;

	void Start()
	{
		mIsHolding = false;
		m_PickingUp = false;
		m_TargetObject = null;

		m_PlayerView = GameObject.FindGameObjectWithTag("MainCamera");
	}

	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (!mIsHolding)
			{
				tryPickObject();
				m_PickingUp = true;
			}
			else
			{
				holdObject();
			}
		}
		else if (mIsHolding)
		{
			DropObject();
		}
	}

	private void tryPickObject()
	{
		RaycastHit hit;

		if (Physics.Raycast(m_PlayerView.transform.position,m_PlayerView.transform.forward, out hit, m_PickUpDistance))
		{
			m_TargetObject = hit.collider.gameObject;
			if (hit.collider.tag == "Door" && m_PickingUp)
			{
				mIsHolding = true;
				m_TargetObject.GetComponent<Rigidbody>().useGravity = true;
				m_TargetObject.GetComponent<Rigidbody>().freezeRotation = false;
				/**/
				m_PickUpDistance = m_DoorPickupRange;
				m_ThrowStrength = m_DoorThrow;
				m_Distance = m_DoorDistance;
				m_MaxDistanceGrab = m_DoorMaxGrab;
			}
		}
	}

	private void holdObject()
	{
		Ray playerAim = m_PlayerView.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		Vector3 nextPos = m_PlayerView.transform.position + playerAim.direction * m_Distance;
		Vector3 currPos = m_TargetObject.transform.position;

		m_TargetObject.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

		if (Vector3.Distance(m_TargetObject.transform.position, m_PlayerView.transform.position) > m_MaxDistanceGrab)
		{
			DropObject();
		}
	}

	private void DropObject()
	{
		mIsHolding = false;
		m_PickingUp = false;
		m_TargetObject.GetComponent<Rigidbody>().useGravity = true;
		m_TargetObject.GetComponent<Rigidbody>().freezeRotation = false;
		m_TargetObject = null;
	}
}