using System;
using SimpleUi.Abstracts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Game.Ui.Windows.TouchPad
{
	public class TouchpadView : UiView, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		private Action<PointerEventData> _dragAction;
		private Action<PointerEventData> _pointerDownAction;
		private Action<PointerEventData> _pointerUpAction;

		public void SetDragAction(Action<PointerEventData> dragAction) => _dragAction = dragAction;
		public void SetPointerDownAction(Action<PointerEventData> pointerDownAction) => _pointerDownAction = pointerDownAction;
		public void SetPointerUpAction(Action<PointerEventData> pointerUpAction) => _pointerUpAction = pointerUpAction;

		public void OnDrag(PointerEventData eventData)
		{
			_dragAction.Invoke(eventData);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_pointerDownAction.Invoke(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_pointerUpAction.Invoke(eventData);
		}
	}
}