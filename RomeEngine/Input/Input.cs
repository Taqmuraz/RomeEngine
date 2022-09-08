using System;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public interface IInputHandler
    {
        void OnKeyDown(KeyCode key);
        void OnKeyUp(KeyCode key);
        void OnMouseDown(Vector2 mousePosition, int button);
        void OnMouseMove(Vector2 mousePosition);
        void OnMouseUp(Vector2 mousePosition, int button);
    }
	public sealed class Input : IInputHandler
	{
		static SafeDictionary<KeyCode, KeyInfo> keys = new SafeDictionary<KeyCode, KeyInfo>(() => emptyKey);
		static List<(KeyCode key, KeyState state)> keysToUpdate = new List<(KeyCode, KeyState)>();

		static readonly KeyInfo emptyKey = new EmptyKeyInfo();

		public static Vector2 MousePosition { get; private set; }

		class EmptyKeyInfo : KeyInfo
		{
			public override KeyState KeyState { get => KeyState.None; set { return; } }
		}

		public class KeyInfo
		{
			public virtual KeyState KeyState { get; set; }
		}

		public Input()
		{

		}

		public static void UpdateInput()
		{
			lock (keys)
			{
				foreach (var key in keys)
				{
					switch (key.Value.KeyState)
					{
						case KeyState.Down: key.Value.KeyState = KeyState.Hold; break;
						case KeyState.Up: key.Value.KeyState = KeyState.None; break;
					}
				}
				lock (keysToUpdate)
				{
					foreach (var key in keysToUpdate)
					{
						var info = keys[key.key];
						if (info == emptyKey) keys[key.key] = info = new KeyInfo();
						info.KeyState = key.state;
					}
					keysToUpdate.Clear();
				}
			}
		}

		public static Vector2 GetWASD()
		{
			Vector2 wasd = Vector2.zero;
			if (GetKey(KeyCode.W)) wasd.y += 1f;
			if (GetKey(KeyCode.A)) wasd.x -= 1f;
			if (GetKey(KeyCode.S)) wasd.y -= 1f;
			if (GetKey(KeyCode.D)) wasd.x += 1f;
			return wasd;
		}

		public static KeyInfo GetKeyInfo(KeyCode key)
		{
			return keys[key];
		}


		public static bool GetKeyDown(KeyCode key)
		{
			return keys[key].KeyState == KeyState.Down;
		}
		public static bool GetKey(KeyCode key)
		{
			return keys[key].KeyState != KeyState.None;
		}
		public static bool GetKeyUp(KeyCode key)
		{
			return keys[key].KeyState == KeyState.Up;
		}

		public void OnKeyDown(KeyCode key)
		{
			lock (keys)
			{
				if (keys[key].KeyState != KeyState.Hold) lock (keysToUpdate) keysToUpdate.Add((key, KeyState.Down));
			}
		}

		public void OnKeyUp(KeyCode key)
		{
			lock (keysToUpdate) keysToUpdate.Add((key, KeyState.Up));
		}

		public void OnMouseDown(Vector2 point, int button)
		{
			button++;
			lock (keysToUpdate) keysToUpdate.Add(((KeyCode)button, KeyState.Down));
			MousePosition = point;
		}

		public void OnMouseMove(Vector2 point)
		{
			MousePosition = point;
		}

		public void OnMouseUp(Vector2 point, int button)
		{
			button++;
			lock (keysToUpdate) keysToUpdate.Add(((KeyCode)button, KeyState.Up));
			MousePosition = point;
		}
	}
}
