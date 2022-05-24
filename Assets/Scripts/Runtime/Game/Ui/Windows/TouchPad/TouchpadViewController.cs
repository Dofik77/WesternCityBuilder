using ECS.Game.Components.General.Input;
using ECS.Utils.Extensions;
using Leopotam.Ecs;
using PdUtils.Interfaces;
using SimpleUi.Abstracts;
using UnityEngine.EventSystems;

namespace Runtime.Game.Ui.Windows.TouchPad
{
	public interface ITouchpadViewController
	{
		void SetActive(bool value);
	}
	
	public class TouchpadViewController : UiController<TouchpadView>, IUiInitializable, ITouchpadViewController
	{
		private readonly EcsWorld _world;
		private EcsEntity _inputEntity;
		private bool _active;
		private int _tapCount;
		
		public TouchpadViewController(EcsWorld world)
		{
			_world = world;
		}
		public void Initialize()
		{
			View.SetDragAction(OnDragAction);
			View.SetPointerDownAction(OnPointerDownAction);
			View.SetPointerUpAction(OnPointerUpAction);
		}

		private void OnPointerDownAction(PointerEventData eventData)
		{
			if(!_active)
				return;
			_tapCount++;
			if (_tapCount > 1)
				return;
			
			_inputEntity = _world.GetInput();
			_inputEntity.Get<PointerDownComponent>().Position = eventData.pointerCurrentRaycast.screenPosition;
			// var entity = _world.GetEntity<PlayerComponent>();
			// if (!entity.IsNull())
				// entity.GetAndFire<RemapPointComponent>().Input = eventData.pointerCurrentRaycast.worldPosition;
		}

		private void OnDragAction(PointerEventData eventData)
		{
			// if(eventData.delta.sqrMagnitude <= 1) return;
			// var worldPos = eventData.pointerCurrentRaycast;
			_inputEntity = _world.GetInput();
			_inputEntity.Get<PointerDragComponent>().Position = eventData.pointerCurrentRaycast.screenPosition;
		}

		
		private void OnPointerUpAction(PointerEventData eventData)
		{
			if(!_active)
				return;
			_tapCount--;
			if (_tapCount > 0)
				return;
			_tapCount = 0;
			_inputEntity = _world.GetInput();
			_inputEntity.Get<PointerUpComponent>().Position = eventData.pointerCurrentRaycast.screenPosition;
		}

		public void SetActive(bool value) => _active = value;
	}

	public struct InputComponent : IEcsIgnoreInFilter
	{
		
	}
}