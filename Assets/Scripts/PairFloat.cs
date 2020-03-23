using UnityEngine;

[System.Serializable]
public class PairFloat : System.Object {
	public float first;
	public float second;
	
	public PairFloat() {
		first = 0.0f;
		second = 0.0f;
	}
	
	public PairFloat(float first, float second) {
		this.first = first;
		this.second = second;
	}

	public void SetBoth(float value) {
		first = value;
		second = value;
	}
	
	public float Min {
		get { return first; }
		set { first = value; }
	}
	
	public float Max {
		get { return second; }
		set { second = value; }
	}

	public float Left {
		get { return first; }
		set { first = value; }
	}
	
	public float Right {
		get { return second; }
		set { second = value; }
	}

	public float From {
		get { return first; }
		set { first = value; }
	}
	
	public float To {
		get { return second; }
		set { second = value; }
	}

}
