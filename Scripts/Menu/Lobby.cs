using System;
using Godot;

public class Lobby : Control {
	public override void _Ready () {
		GetTree ().Connect ("connected_to_server", this, "_OnJoinedAServer");
	}

	string name = "";
	string ipAdress = "";

	void _OnNameChanged (string new_text) {
		name = new_text;
		GameRoot.username = name;
	}

	void _OnIpChanged (string new_text) {
		ipAdress = new_text;
	}

	void _OnHostPressed () {
		Network.Host ();
		this.Hide ();
		GetNode<Control> ("../SaveMenu").Show ();
	}

	void _OnJoinPressed () {
		Network.Join (ipAdress);
	}

	void _OnJoinedAServer () {
		GameRoot.LoadGameScene ("");
	}

	void _OnBackPressed () {
		this.Hide ();
		GetNode<Control> ("../BaseMenu").Show ();
	}
	// void _StartGame () {
	// 	//
	// 	Global.LoadGameScene ("");
	// }
}
