using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestForLections
{
    // First test
    [Test]
    public void CalcDamageV1()
    {
        var damageValue = DamageCalculation.CalculateDamage(1.1f);
        Assert.IsTrue(damageValue == 1);
    }

    // First Failed test
    [Test]
    public void CalcDamageV2()
    {
        var damageValue = DamageCalculation.CalculateDamage(9.9f);
        Assert.IsTrue(damageValue == 10);
    }

    // Test with Play
    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject gameGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        var game = gameGameObject.GetComponent<Game>();
        Debug.Log(TestContext.CurrentContext.Test.FullName);

        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialYPos = asteroid.transform.position.y;

        yield return new WaitForSeconds(1.0f);

        Assert.Less(asteroid.transform.position.y, initialYPos);

        Object.Destroy(asteroid);
        Object.Destroy(game.gameObject);
    }

    /*
     * A special test with an important distinction.
     * Notice how you are explicitly using UnityEngine.Assertions for this test?
     * That’s because Unity has a special Null class which is different from a “normal” Null class.
     * The NUnit framework assertion Assert.IsNull() will not work for Unity null checks.
     * When checking for nulls in Unity, you must explicitly use the UnityEngine.Assertions.Assert, not the NUnit Assert.
     */
    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        _game = gameGameObject.GetComponent<Game>();
        Debug.Log(TestContext.CurrentContext.Test.FullName);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(_game.gameObject);
    }
    private Game _game;
    [UnityTest]
    public IEnumerator LaserDestroysAsteroid()
    {
        GameObject asteroid = _game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;

        GameObject laser = _game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        //Assert.IsNull(asteroid);
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }

    public class TestInput : IInput
    {
        bool _isLeft;
        bool _isRight;
        public TestInput(bool left = false, bool right = false)
        {
            _isLeft = left;
            _isRight = right;
        }
        public bool IsLeft => _isLeft;

        public bool IsRight => _isRight;
    }
    // Test input
    [UnityTest]
    public IEnumerator ShipMovementLeft()
    {
        GameObject gameGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        var game = gameGameObject.GetComponent<Game>();
        Debug.Log(TestContext.CurrentContext.Test.FullName);

        var ship = game.GetShip();
        ship.Init(new TestInput(true, false));

        float initialXPos = ship.transform.position.x;
        Debug.Log(ship.transform.position);

        yield return new WaitForSeconds(1.0f);

        Debug.Log(ship.transform.position);

        Assert.Less(ship.transform.position.x, initialXPos);

        Object.Destroy(game.gameObject);
    }
}
