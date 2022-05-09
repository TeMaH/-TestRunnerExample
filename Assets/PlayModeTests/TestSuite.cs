using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    private Game _game;
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

    [Test]
    public void MathFMin()
    {
        var minValue = Mathf.Min(-1000.0f, -1.0f);
        Assert.IsTrue(minValue == -1000.0f);
    }

        [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = _game.GetSpawner().SpawnAsteroid();
        float initialYPos = asteroid.transform.position.y;

        yield return new WaitForSeconds(1.0f);

        Assert.Less(asteroid.transform.position.y, initialYPos);

        Object.Destroy(asteroid);
        yield return new WaitForSeconds(1.0f);

    }

    [UnityTest]
    public IEnumerator GameOverOccursOnAsteroidCollision()
    {
        GameObject asteroid = _game.GetSpawner().SpawnAsteroid();

        asteroid.transform.position = _game.GetShip().transform.position;

        yield return new WaitForSeconds(1.0f);

        Assert.True(_game.isGameOver);
        yield return new WaitForSeconds(1.0f);

    }

    [UnityTest]
    public IEnumerator NewGameRestartsGame()
    {
        _game.isGameOver = true;
        _game.NewGame();

        Assert.False(_game.isGameOver);

        yield return null;
        yield return new WaitForSeconds(1.0f);

    }

    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        GameObject laser = _game.GetShip().SpawnLaser();
        float initialYPos = laser.transform.position.y;

        yield return new WaitForSeconds(1.0f);

        Assert.Greater(laser.transform.position.y, initialYPos);
        yield return new WaitForSeconds(1.0f);

    }

    /*
     * A special test with an important distinction.
     * Notice how you are explicitly using UnityEngine.Assertions for this test?
     * That’s because Unity has a special Null class which is different from a “normal” Null class.
     * The NUnit framework assertion Assert.IsNull() will not work for Unity null checks.
     * When checking for nulls in Unity, you must explicitly use the UnityEngine.Assertions.Assert, not the NUnit Assert.
     */

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

    [UnityTest]
    public IEnumerator DestroyedAsteroidRaisesScore()
    {
        GameObject asteroid = _game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;

        GameObject laser = _game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(_game.score, 1);
    }

    // https://github.com/nunit/docs/wiki/NUnit-Documentation
}
