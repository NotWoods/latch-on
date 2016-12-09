using UnityEngine;
using System.Collections.Generic;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	const string SpawnPointName = "Spawn Point";

	public GameObject PlayerPrefab;

	[SerializeField]
	private Dictionary<ControlType, GameObject> Players = new Dictionary<ControlType, GameObject>();
	private Transform spawnPoint;

	void Awake() {
		spawnPoint = transform.Find(SpawnPointName);
	}

	public GameObject SpawnPlayer(ControlType controller = ControlType.Keyboard) {
		if (Players.ContainsKey(controller)) return null;

		GameObject player = Instantiate(PlayerPrefab, spawnPoint.position, Quaternion.identity);

		PlayerMarker marker = player.AddComponent<PlayerMarker>();
		marker.Controller = controller;

		Players.Add(controller, player);
		player.GetComponent<Prime31.CharacterController2D>().warpToGrounded();
		return player;
	}
}
