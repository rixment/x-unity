using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{	
	public float pow = 2.0f;

	public float turnFraction = 1.61f;

	private List<Vector3> points = new List<Vector3>();

	[SerializeField]
	private MeshRenderer mesh;

	public BoidSettings settings;

	[SerializeField]
	private float speed;

	private float colliderRadius = 0.1f;

	private Vector3 velocity;

	private float weightAlignment;
	private float weightCohesion;
	private float weightSeparation;

	void Start()
	{
		speed = Random.Range(settings.speed.Min, settings.speed.Max);
		weightAlignment = Random.Range(settings.weightAlignment.Min, settings.weightAlignment.Max);
		weightCohesion = Random.Range(settings.weightCohesion.Min, settings.weightCohesion.Max);
		weightSeparation = Random.Range(settings.weightSeparation.Min, settings.weightSeparation.Max);

		int PointsCount = 16;
		for (int i = 0; i < PointsCount; ++i)
			points.Add(new Vector3());

		Generate3d();		
	}

	public Color Color
	{
		set { mesh.material.color = value; }
	}

	public void UpdateBoid(BoidManager.BoidData boidData)
	{
		Vector3 acceleration = Vector3.zero;

		if (IsHeadingForCollision())
		{
			Vector3 collisionAvoidDir = ObstacleRays();
			Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
			acceleration = collisionAvoidForce;
		}
		else 
		if (boidData.flockMatesCount != 0)
		{
			boidData.flockCentre /= boidData.flockMatesCount;
			boidData.flockHeading /= boidData.flockMatesCount;

			Vector3 offsetToFlockmatesCentre = (boidData.flockCentre - transform.position);

			var alignmentForce = SteerTowards(boidData.flockHeading) * weightAlignment * settings.multiplierAlignment;
			var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * weightCohesion * settings.multiplierCohesion;
			var seperationForce = SteerTowards(boidData.avoidanceHeading) * weightSeparation * settings.multiplierSeparation;

			acceleration += alignmentForce;
			acceleration += cohesionForce;
			acceleration += seperationForce;
		}

		velocity = transform.forward * speed * Time.deltaTime;
		velocity += acceleration * Time.deltaTime;

		transform.position += velocity; 
		transform.forward = velocity;
	}

	private void Generate3d()
	{
		for (int i = 0; i < points.Count; ++i)
		{
			float t = Mathf.Pow(i / (float)(points.Count - 1), pow);
			float inclination = Mathf.Acos(1 - 2 * t);
			float azimuth = 2 * Mathf.PI * turnFraction * i;

			float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
			float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
			float z = Mathf.Cos(inclination);

			points[i] = new Vector3(x, y, z);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * settings.collisionAvoidDst);

		Gizmos.DrawSphere(transform.position, colliderRadius);

		Gizmos.DrawWireSphere(transform.position, settings.perceptionRadius);

		Gizmos.color = Color.white;

		foreach (var point in points)
			Gizmos.DrawLine(transform.position, transform.position + point);
	}

	private Vector3 ObstacleRays()
	{
		foreach(var point in points)
		{
			Vector3 dir = transform.TransformDirection(point);
			Ray ray = new Ray(transform.position, dir);
			if (!Physics.SphereCast(ray, colliderRadius, settings.collisionAvoidDst))
				return dir;
		}
		return transform.forward;
	}

	private Vector3 SteerTowards(Vector3 vector)
	{
		Vector3 v = vector.normalized * settings.speed.Max - velocity;
		return Vector3.ClampMagnitude(v, settings.maxSteerForce);
	}

	private bool IsHeadingForCollision()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		return Physics.SphereCast(ray, colliderRadius, settings.collisionAvoidDst);
	}
}