using UnityEngine;

public class Sphere {
    public float Scale;
    public Vector3 Position;
    public GameObject SphereGameObject;
    
    public Sphere(float scale, Vector3 position, GameObject sphere) {
        Scale = scale;
        Position = position;
        SphereGameObject = Object.Instantiate(sphere, Position, Quaternion.identity);
    }
}
