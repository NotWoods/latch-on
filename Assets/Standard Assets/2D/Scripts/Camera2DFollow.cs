using System;
using UnityEngine;

namespace UnityStandardAssets._2D {
   public class Camera2DFollow : MonoBehaviour {
		public Transform target;
		public float damping = 1;
		public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
		public float lookAheadMoveThreshold = 0.1f;
		public Vector3 offset = new Vector3 (0, 2, 0);
		public Vector3 lookOffset = Vector3.zero;

		public Vector2 minPos = new Vector2(-500, -500);
		public Vector2 maxPos = new Vector2(500, 500);

		private float m_OffsetZ;
		private Vector3 m_LastTargetPosition;
		private Vector3 m_CurrentVelocity;
		private Vector3 m_LookAheadPos;

		private void Start() {
			m_LastTargetPosition = target.position;
			m_OffsetZ = (transform.position - target.position).z;
			transform.parent = null;
		}

		private void Update() {
			float xMoveDelta = (target.position - m_LastTargetPosition).x;
			bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
			if (updateLookAheadTarget) {
				m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
			} else {
				m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
			}

			Vector3 aheadTargetPos = target.position + offset + lookOffset + m_LookAheadPos + Vector3.forward*m_OffsetZ;
			if (aheadTargetPos.x < minPos.x) {aheadTargetPos = new Vector3(minPos.x, aheadTargetPos.y, aheadTargetPos.z);
			} else if (aheadTargetPos.x > maxPos.x) {aheadTargetPos = new Vector3(maxPos.x, aheadTargetPos.y, aheadTargetPos.z);}
			if (aheadTargetPos.y < minPos.y) {aheadTargetPos = new Vector3(aheadTargetPos.x, minPos.y, aheadTargetPos.z);
			} else if (aheadTargetPos.y > maxPos.y) {aheadTargetPos = new Vector3(aheadTargetPos.x, maxPos.y, aheadTargetPos.z);}
			Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
			transform.position = newPos;
			m_LastTargetPosition = target.position;

			if (m_OffsetZ > -5) {m_OffsetZ = -5;}
			if (m_OffsetZ < -70) {m_OffsetZ = -70;}
			if (Input.GetAxis("Zoom") != 0) {
				m_OffsetZ += Input.GetAxis("Zoom");
			}
			if (Input.touchCount == 2) {
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				m_OffsetZ -= deltaMagnitudeDiff * 0.5f;
			}
		}
	}
}
