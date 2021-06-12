using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Util : MonoBehaviour {

	public static Vector2 RoundToDegrees(Vector2 input, float degrees) {
		float round = degrees * Mathf.Deg2Rad;
		float angle = Mathf.Atan2(input.y, input.x);

		Vector2 output;
		if (angle % round != 0) {
			float newAngle = Mathf.Round(angle / round) * round;
			output = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
		} else {
			output = input.normalized;
		}
		return output;
	}
}
