using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetSliderValue : MonoBehaviour{

	public void setSliderValue() {
		string value = Mathf.RoundToInt(this.gameObject.GetComponent<Slider>().value * 100) + "%";

		this.gameObject.GetComponentInChildren<Text>().text = value;
	}
}
