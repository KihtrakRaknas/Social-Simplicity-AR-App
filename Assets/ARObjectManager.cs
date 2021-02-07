using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    static Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameObject getObject(string name)
    {
        if (!Objects.ContainsKey(name))
            Objects.Add(name, GameObject.Find(name));
        return Objects[name];
    }
}
