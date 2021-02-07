using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submit : MonoBehaviour
{
    public Text username;
    public Text password;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onClickFun()
    {
        spawner.username = username.text;
        spawner.password = password.text;
        canvas.enabled = false;
    }
}
