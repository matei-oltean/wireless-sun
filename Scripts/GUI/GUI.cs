using System;
using Godot;

public class GUI : Node {
	public InventoryGUI inventory;
	public CraftListGUI crafting;
	public PauseMenu pauseMenu;
	public enum Window {
		Nothing,
		Inventory,
		Pause
	}
	public Window current;
	public override void _Ready () {
		current = Window.Nothing;
		inventory = (InventoryGUI) GetNode ("Inventory");
		inventory.Hide ();
		crafting = GetNode<CraftListGUI> ("Crafting");
		crafting.Hide ();
		pauseMenu = GetNode<PauseMenu> ("PauseMenu");
		pauseMenu.Hide ();
		GetNode<Control> ("ShowOnGameplay").Hide ();

		GameRoot.Instance.Connect (nameof (GameRoot.GameplayStarted), this, nameof (GameplayStart));
	}

	public void GameplayStart () { GetNode<Control> ("ShowOnGameplay").Show (); }
	public void GameplayEnd () { GetNode<Control> ("ShowOnGameplay").Hide (); }

	public GUIWindow GetWindowNode (Window window) {
		if (window == Window.Inventory) {
			return inventory;
		}
		if (window == Window.Pause) {
			return inventory;
		}
		return null;
	}
	public void Open (Window window) {
		if (window == current) {
			return;
		}
		GetWindowNode (current)?.Minimise ();
		GetWindowNode (window)?.Maximise ();
		current = window;
	}
	public void Toggle (Window window) {
		if (window == current) {
			Open (Window.Nothing);
		} else {
			Open (window);
		}
	}
}