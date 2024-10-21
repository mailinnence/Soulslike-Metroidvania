using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class money : MonoBehaviour

{
    [SerializeField]
	public Text textField;




    // Start is called before the first frame update
    void Start()
    {
       
    }

	void Update()
	{
	    if (Input.GetKeyDown(KeyCode.J))
	    {
	        AddValue(100);
	    }
	}


	void AddValue(int valueToAdd)
	{
	    int currentValue = int.Parse(textField.text);
	    textField.text = (currentValue + valueToAdd).ToString();
	}
	
}