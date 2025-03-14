using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
	public Transform Hero; // A karakter referenciája
	public float smoothSpeed = 5f; // A követés simasága
	public Vector3 offset; // Eltolás a karakterhez képest

	void LateUpdate()
	{
		if (Hero != null)
		{
			Vector3 targetPosition = new Vector3(Hero.position.x + offset.x, Hero.position.y + offset.y, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
		}
	}
}
