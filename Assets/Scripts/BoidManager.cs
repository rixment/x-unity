using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BoidManager : MonoBehaviour 
{
    public struct BoidData
    {
        public Vector3 position;
        public Vector3 direction;

        public Vector3 flockHeading;
        public Vector3 flockCentre;
        public Vector3 avoidanceHeading;
        public int flockMatesCount;

        public static int Size => Marshal.SizeOf<BoidData>();
    }

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    public ComputeShader compute;
    
    private Boid[] boids;

    void Start () 
    {
        boids = FindObjectsOfType<Boid>();
        foreach (Boid b in boids)
            b.settings = settings;
    }

    void FixedUpdate () 
    {
        if (boids != null) 
        {
            int numBoids = boids.Length;
            var boidData = new BoidData[numBoids];

            for (int i = 0; i < boids.Length; i++) {
                boidData[i].position = boids[i].transform.position;
                boidData[i].direction = boids[i].transform.forward;
            }

            var boidBuffer = new ComputeBuffer (numBoids, BoidData.Size);
            boidBuffer.SetData (boidData);

            int idxKernel = compute.FindKernel("CSMain");

            compute.SetBuffer (idxKernel, "boids", boidBuffer);
            compute.SetInt ("numBoids", boids.Length);
            compute.SetFloat ("viewRadius", settings.perceptionRadius);
            compute.SetFloat ("avoidRadius", settings.avoidanceRadius);

            int threadGroups = Mathf.CeilToInt (numBoids / (float) threadGroupSize);
            
            compute.Dispatch (idxKernel, threadGroups, 1, 1);

            boidBuffer.GetData (boidData);

            for(int i = 0; i < boids.Length; i++)
                boids[i].UpdateBoid(boidData[i]);

            boidBuffer.Release ();
        }
    }
}
