using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200096a; bodies translated from the 1.10.1 x86-64 binary.
	public interface IUserEditableUIComponent
	{
		void TransformUpdated();

		RectTransform GetRectTransform();

		bool GetTouchPosition(out Vector2 touch);

		bool GetPinchInfo(out float pinchDistance, out float initialPinchDistance);

		void SetEditing(bool editing);
	}
}
