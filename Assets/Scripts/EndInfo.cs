using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInfo : MonoBehaviour
{
	public TMPro.TextMeshProUGUI timeText;
	public TMPro.TextMeshProUGUI starsText;

	public int starsMax;

	public void SetInfo(string time, int starsCollected)
	{
		timeText.text = "Time: " + time;
		starsText.text = "Stars: " + starsCollected + "/" + starsMax;
	}
}
