using System;
using Godot;

public class GUI : Node {
	public InventoryGUI inventory;
	public CraftListGUI crafting;
	public enum Window {
		Nothing,
		Inventory,
		Option,
	}
	public Window current;
	public override void _Ready () {
		current = Window.Nothing;
		inventory = (InventoryGUI) GetNode ("Inventory");
		inventory.Hide ();
		crafting = GetNode<CraftListGUI> ("Crafting");
		crafting.Hide ();
	}

	public GUIWindow GetWindowNode (Window window) {
		if (window == Window.Inventory) {
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

	public override void _Input (InputEvent @event) {
		if (@event.IsActionPressed ("inventory")) {
			Toggle (Window.Inventory);
		}
	}
}
