using System;
using System.Collections.Generic;
using SimpleUi.Abstracts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Game.Ui.Windows.FocusSpace
{
    public interface IFocusViewController
    {
        void Focus(GameObject gameObject, Action onComplete = null);
        void Unfocus(GameObject gameObject);
        void UnfocusAll();
    }
    public class FocusViewController : UiController<FocusView>, IInitializable, IFocusViewController
    {
        [Inject] private readonly Camera _gameCamera;
        private readonly List<GameObject> _inFocusObjects = new List<GameObject>();
        private int _indexOrder = 1;

        public void Initialize()
        {
            View.canvas.worldCamera = _gameCamera;
            View.canvas.gameObject.SetActive(false);
        }

        public void Focus(GameObject gameObject, Action onComplete = null)
        {
            if (_inFocusObjects.Count == 0)
            {
                View.canvas.gameObject.SetActive(true);
                View.SetActiveBack(true, onComplete);
            }

            _inFocusObjects.Add(gameObject);
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = ++_indexOrder;
            gameObject.AddComponent<GraphicRaycaster>();
        }

        public void Unfocus(GameObject gameObject)
        {
            _indexOrder--;
            Object.Destroy(gameObject.GetComponent<GraphicRaycaster>());
            Object.Destroy(gameObject.GetComponent<Canvas>());
            _inFocusObjects.Remove(gameObject);

            if (_inFocusObjects.Count == 0)
                View.SetActiveBack(false, 
                    () => View.canvas.gameObject.SetActive(false));
        }

        public void UnfocusAll()
        {
            var count = _inFocusObjects.Count;
            for (var index = 0; index < count; index++)
            {
                var obj = _inFocusObjects[0];
                Unfocus(obj);
            }
        }
    }
}