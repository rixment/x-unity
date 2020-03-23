using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Broid2D : MonoBehaviour 
{
	[SerializeField]
	private Slider turnSlider;

	[SerializeField]
	private InputField turnInput;

	[SerializeField]
	private Slider animSlider;

	[SerializeField]
	private Slider powSlider;

	[SerializeField]
	private InputField powInput;

	[SerializeField]
	private Toggle toggle3d;

	private float animate;

	private float pow;

	private float turnFraction;

	private LineRenderer line;

	Action generateModel;

	public float TurnFraction
	{
		get { return turnFraction; }
		set 
		{ 
			turnFraction = value;
			turnSlider.value = value;
			turnInput.text = value.ToString();

			generateModel();
		}
	}

	public float Pow
	{
		get { return pow; }
		set
		{
			pow = value;
			powSlider.value = value;
			powInput.text = value.ToString();

			generateModel();
		}
	}

	public void OnTurnChange()
	{
		if (turnInput.text.EndsWith(".") || turnInput.text.EndsWith("0"))
			return;

		float newFraction;
		if (float.TryParse(turnInput.text, out newFraction))
			TurnFraction = newFraction;
	}

	public void OnPowChange()
	{
		if (turnInput.text.EndsWith(".") || turnInput.text.EndsWith("0"))
			return;

		float newPow;
		if (float.TryParse(powInput.text, out newPow))
			Pow = newPow;
	}

	public void On3dModelChange()
	{
		generateModel = toggle3d.isOn ? new Action(Generate3d) : new Action(Generate2d);
		generateModel();
	}

	public float Animate
	{
		get { return animate; }
		set
		{
			animate = value;
			animSlider.value = value;
		}
	}

	[SerializeField]
	public GameObject prefab;

	private List<Vector3> points = new List<Vector3>();
	private List<GameObject> objects = new List<GameObject>();

	void Awake()
	{
		line = GetComponent<LineRenderer>();
	}
	
	void Start () 
	{
		On3dModelChange();
		Pow = 1.0f;
		TurnFraction = 0.0f;		

		int PointsCount = 500;
		line.positionCount = PointsCount;
		for (int i = 0; i < PointsCount; ++i)
		{
			points.Add(new Vector2());
			GameObject point = Instantiate(prefab);
			//point.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.yellow, new Color(1,0.5f,0), (float)i / PointsCount);
			objects.Add(point);
			LineRenderer lr = GetComponent<LineRenderer>();
		}
	}

	void Update()
	{
		if (animSlider.value == 0)
			return;

		TurnFraction += Time.deltaTime * (Animate * 0.01f);

		generateModel();

		line.SetPositions(points.ToArray());
	}

	private void Generate2d()
	{
		for (int i = 0; i < points.Count; ++i)
		{
			float dst = Mathf.Pow(i / (float)(points.Count - 1), pow);
			float angle = 2 * Mathf.PI * TurnFraction * i;

			float x = dst * Mathf.Cos(angle);
			float y = dst * Mathf.Sin(angle);

			points[i] = new Vector2(x, y);
			objects[i].transform.position = points[i];
		}
	}

	private void Generate3d()
	{
		for (int i = 0; i < points.Count; ++i)
		{
			float t = Mathf.Pow(i / (float)(points.Count - 1), pow);
			float inclination = Mathf.Acos(1 - 2 * t);
			float azimuth = 2 * Mathf.PI * TurnFraction * i;
			
			float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
			float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
			float z = Mathf.Cos(inclination);

			points[i] = new Vector3(x, y, z);
			objects[i].transform.position = points[i];
		}
	}
}
