using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code from Jason Yang
/// https://github.com/dci05049/Verlet-Rope-Unity
/// </summary>
public class Line : MonoBehaviour, IActivatable
{
	[SerializeField] private LineSegment lineSegmentPrefab;
	[SerializeField] private Transform segmentParent;
	[SerializeField] private HookSegment hook;
	public HookSegment Hook { get { return hook; } }

	[SerializeField] private float castForce = 300f;
	[SerializeField] private float reelSpeed = 1f;

	[SerializeField] private DistanceJoint2D distanceJoint;
	[SerializeField] private LineRenderer lineRenderer;
	private List<LineSegment> LineSegments = new List<LineSegment>();
	[SerializeField] private float ropeSegLen = 0.25f;
	[SerializeField] private int segmentLength = 35;
	[SerializeField] private float lineWidth = 0.1f;

	private bool isReeled;
	public bool IsReeled { get { return isReeled; } }

	// Use this for initialization
	void Start()
	{
		distanceJoint.distance = ropeSegLen * segmentLength;
		Vector3 ropeStartPoint = transform.position;
		for (int i = 0; i < segmentLength - 1; i++)
		{
			this.LineSegments.Add(Instantiate<LineSegment>(lineSegmentPrefab, segmentParent));
			this.LineSegments[i].Init(ropeStartPoint, lineWidth / 2);
		}
		hook.Init(ropeStartPoint, lineWidth / 2);
		hook.OnReelComplete += OnReel;
		isReeled = true;
	}

	// Update is called once per frame
	void Update()
	{
		this.DrawRope();
	}

	public void Activate() {
		// Cast(dir);
		// TODO cast perpendicular to the parent line somehow ___|__
	}

	// Send line out
	public void Cast(Vector2 dir) {
		isReeled = false;
		hook.AddForce(dir * castForce);
	}

	// Pull line in
	public void Reel() {
		if (!isReeled) {
			hook.Reel(transform.position, reelSpeed);
		}
	}

	private void OnReel() {
		isReeled = true;
	}

	private void FixedUpdate()
	{
		this.Simulate();
	}


#region Line rendering & stuff
	private void Simulate()
	{
		// SIMULATION
		Vector2 forceGravity = new Vector2(0f, -1.5f); // TODO expose grav?

		for (int i = 1; i < LineSegments.Count; i++)
		{
			LineSegment firstSegment = this.LineSegments[i];
			Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
			firstSegment.posOld = firstSegment.posNow;
			firstSegment.posNow += velocity;
			firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
			this.LineSegments[i] = firstSegment;
		}

		//CONSTRAINTS
		for (int i = 0; i < 50; i++)
		{
			this.ApplyConstraint();
		}
	}

	private void ApplyConstraint()
	{
		LineSegment firstSegment = this.LineSegments[0];
		firstSegment.posNow = transform.position;
		this.LineSegments[0] = firstSegment;

		LineSegment endSegment = LineSegments[LineSegments.Count - 1];
		endSegment.posNow = hook.transform.position;
		LineSegments[LineSegments.Count - 1] = endSegment;

		// TODO limit hook position as dist from first segment

		for (int i = 0; i < LineSegments.Count - 1; i++)
		{
			LineSegment firstSeg = this.LineSegments[i];
			LineSegment secondSeg = this.LineSegments[i + 1];

			float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
			float error = Mathf.Abs(dist - this.ropeSegLen);
			Vector2 changeDir = Vector2.zero;

			if (dist > ropeSegLen)
			{
				changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
			} else if (dist < ropeSegLen)
			{
				changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
			}

			Vector2 changeAmount = changeDir * error;
			if (i != 0)
			{
				firstSeg.posNow -= changeAmount * 0.5f;
				this.LineSegments[i] = firstSeg;
				secondSeg.posNow += changeAmount * 0.5f;
				this.LineSegments[i + 1] = secondSeg;
			}
			else
			{
				secondSeg.posNow += changeAmount;
				this.LineSegments[i + 1] = secondSeg;
			}
		}
	}

	private void DrawRope()
	{
		float lineWidth = this.lineWidth;
		lineRenderer.startWidth = lineWidth;
		lineRenderer.endWidth = lineWidth;

		Vector3[] ropePositions = new Vector3[LineSegments.Count];
		for (int i = 0; i < ropePositions.Length; i++)
		{
			ropePositions[i] = this.LineSegments[i].posNow;
		}

		lineRenderer.positionCount = ropePositions.Length;
		lineRenderer.SetPositions(ropePositions);
	}
#endregion
}