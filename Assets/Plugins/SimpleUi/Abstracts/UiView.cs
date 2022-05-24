﻿using SimpleUi.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleUi.Abstracts
{
	public abstract class UiView : UIBehaviour, IUiView
	{
		public bool IsShow { get; private set; }

		public virtual void Show()
		{
			gameObject.SetActive(true);
			IsShow = true;
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
			IsShow = false;
		}

		void IUiView.SetParent(Transform parent)
		{
			transform.SetParent(parent, false);
		}

		void IUiView.SetOrder(int index)
		{
			var parent = transform.parent;
			if (parent == null)
				return;
			var childCount = parent.childCount - 1;
			transform.SetSiblingIndex(childCount - index);
		}

		IUiElement[] IUiView.GetUiElements()
		{
			return gameObject.GetComponentsInChildren<IUiElement>();
		}

		void IUiView.Destroy()
		{
			Destroy(gameObject);
		}
	}
}