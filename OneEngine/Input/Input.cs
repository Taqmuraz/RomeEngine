using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
    public enum KeyCode
    {
        None = 0,
        MouseL = 1, MouseR = 2, MouseM = 3,
        N0 = 48, N1, N2, N3, N4, N5, N6, N7, N8, N9,
        A = 65, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        ShiftKey = 16, ControlKey = 17, Space = 32
    }
    public enum KeyState
    {
        None,
        Down,
        Hold,
        Up
    }

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

		public static Vector2 mousePosition { get; private set; }

		class EmptyKeyInfo : KeyInfo
		{
			public override KeyState keyState { get => KeyState.None; set { return; } }
		}

		public class KeyInfo
		{
			public virtual KeyState keyState { get; set; }
		}

		internal Input()
		{

		}

		public static void UpdateInput()
		{
			lock (keys)
			{
				foreach (var key in keys)
				{
					switch (key.Value.keyState)
					{
						case KeyState.Down: key.Value.keyState = KeyState.Hold; break;
						case KeyState.Up: key.Value.keyState = KeyState.None; break;
					}
				}
				lock (keysToUpdate)
				{
					foreach (var key in keysToUpdate)
					{
						var info = keys[key.key];
						if (info == emptyKey) keys[key.key] = info = new KeyInfo();
						info.keyState = key.state;
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
			return keys[key].keyState == KeyState.Down;
		}
		public static bool GetKey(KeyCode key)
		{
			return keys[key].keyState != KeyState.None;
		}
		public static bool GetKeyUp(KeyCode key)
		{
			return keys[key].keyState == KeyState.Up;
		}

		public void OnKeyDown(KeyCode key)
		{
			lock (keys)
			{
				if (keys[key].keyState != KeyState.Hold) lock (keysToUpdate) keysToUpdate.Add((key, KeyState.Down));
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
			mousePosition = point;
		}

		public void OnMouseMove(Vector2 point)
		{
			mousePosition = point;
		}

		public void OnMouseUp(Vector2 point, int button)
		{
			button++;
			lock (keysToUpdate) keysToUpdate.Add(((KeyCode)button, KeyState.Up));
			mousePosition = point;
		}
	}
}
