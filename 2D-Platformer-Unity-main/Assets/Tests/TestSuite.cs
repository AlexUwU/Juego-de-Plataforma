using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TestSuite
{
    private GameObject playerObject;
    private PlayerController playerController;
    private GameObject coinObject;
    private GameManager gameManager;
    private GameObject uiManagerObject;

    [SetUp]
    public void Setup()
    {
        playerObject = Resources.Load<GameObject>("Prefabs/Player");
        playerObject = Object.Instantiate(playerObject);

        playerController = playerObject.GetComponent<PlayerController>();
        playerController.rb = playerObject.GetComponent<Rigidbody2D>();
        playerController.jumpForce = 60f;

        if (playerController.groundCheck == null)
        {
            playerController.groundCheck = new GameObject("GroundCheck").transform;
        }

        uiManagerObject = Resources.Load<GameObject>("Prefabs/Canvas");
        uiManagerObject = Object.Instantiate(uiManagerObject);

        UIManager.instance = uiManagerObject.GetComponent<UIManager>();
        UIManager.instance.playerController = playerController;

        gameManager = new GameObject().AddComponent<GameManager>();
        gameManager.playerController = playerController;
        GameManager.instance = gameManager;

        GameObject coinTextObject = new GameObject("CoinText");
        coinTextObject.AddComponent<TMPro.TextMeshProUGUI>();
        gameManager.coinText = coinTextObject.GetComponent<TMP_Text>();

        gameManager.coinCount = 0;

        coinObject = Resources.Load<GameObject>("Prefabs/Pickup/Dollar");
        coinObject = Object.Instantiate(coinObject);
        coinObject.transform.position = new Vector3(0, 0, 0);

        coinObject.GetComponent<pickup>().pt = pickup.pickupType.coin;
        coinObject.GetComponent<pickup>().PickupEffect = new GameObject();
    }

    [UnityTest]
    public IEnumerator TestSalto()
    {
        playerController.IsGrounded();

        float initialY = playerController.rb.position.y;

        playerController.Jump(playerController.jumpForce);

        yield return new WaitForSeconds(1f);

        Assert.Greater(playerController.rb.position.y, initialY, "El jugador no salto correctamente.");
    }

    [UnityTest]
    public IEnumerator TestRecogerMoneda()
    {
        playerObject.transform.position = new Vector3(0, 0, 0);

        var playerCollider = playerObject.GetComponent<Collider2D>();
        var coinCollider = coinObject.GetComponent<Collider2D>();

        coinObject.GetComponent<pickup>().OnTriggerEnter2D(playerCollider);

        yield return null;

        Assert.AreEqual(1, GameManager.instance.coinCount, "El contador de monedas no se incrementó correctamente.");
    }

    [UnityTest]
    public IEnumerator TestKillzone()
    {
        GameObject killzoneObject = Resources.Load<GameObject>("Prefabs/KillZone");
        killzoneObject = Object.Instantiate(killzoneObject);

        playerObject.transform.position = new Vector3(0, 2, 0);
        killzoneObject.transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.5f);

        Assert.IsFalse(playerObject.activeSelf, "El jugador no murió al caer en la Killzone.");
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerObject);
        Object.DestroyImmediate(coinObject);
        Object.DestroyImmediate(gameManager);
        Object.DestroyImmediate(uiManagerObject);
    }
}

