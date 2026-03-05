using System;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public GameObject emitter; // Emits spheres
    public GameObject sphereGameObject; 

    private int _numSpheres;
    private int _sphereIndex;
    private List<Sphere> _spheres;
    private double _timeToEmit;
    

    public float scale = 1;
    public float period = 0.5f;
    public int maxSpheres = 25;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _numSpheres = 0;
        _sphereIndex = 0;
        _spheres = new List<Sphere>();
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;
        // Emit spheres
        _timeToEmit -= deltaTime;
        if (_timeToEmit <= 0.0) EmitSpheres();
    }


    private void EmitSpheres()
    {
        // Initialize local position of a sphere
        Vector3 localPos = new Vector3(0f, 0f, 0f);

        // Get the world position of a sphere
        Vector3 worldPos = emitter.transform.TransformPoint(localPos);


        // Initialize a sphere 
        Sphere sphere = new Sphere(scale, worldPos, sphereGameObject);
        if (_numSpheres < maxSpheres)
        {
            // Add another sphere
            _spheres.Add(sphere);
            _numSpheres++;
        }
        else
        {
            Sphere destroy = _spheres[_sphereIndex];
            Destroy(destroy.SphereGameObject);
            // Keep the number of sphere to a finite amount by just replacing the old sphere
            _spheres[_sphereIndex++] = sphere;
            // If the end is reached, reset the index to start remove the index-0 sphere
            if (_sphereIndex >= maxSpheres)
                _sphereIndex = 0;
        }

        // Reset the time
        _timeToEmit = period;
    }

}


