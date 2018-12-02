using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHelmsman : MonoBehaviour
{
	[SerializeField] private Engines engines;
	// [SerializeField] private float raycastInterval = 0.3f;
	// private bool isRaycasting;

	// private IEnumerator ProximitySensors()
	// {
	// 	while (isRaycasting)
	// 	{
			
	// 		yield return new WaitForSeconds(raycastInterval);
	// 	}
	// }

	// private void OnEnable()
	// {
	// 	isRaycasting = true;
	// 	StartCoroutine(ProximitySensors());
	// }

	// private void OnDisable()
	// {
	// 	isRaycasting = false;
	// }
}
