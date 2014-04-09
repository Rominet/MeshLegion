﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AnimationControllerScript))]
public class MonsterScript : MonoBehaviour {

    [SerializeField]
    private float _chokeDuration = 5;
    public float ChokeDuration
    {
        get { return _chokeDuration; }
        set { _chokeDuration = value; }
    }

    [SerializeField]
    private NavManagerScript _navScript;
    public NavManagerScript NavScript
    {
        get { return _navScript; }
        set { _navScript = value; }
    }

    [SerializeField]
    private AnimationControllerScript _animControlerScript;
    public AnimationControllerScript AnimControlerScript
    {
        get { return _animControlerScript; }
        set { _animControlerScript = value; }
    }

    [SerializeField]
    private GhostScript _ghostScript;
    public GhostScript GhostScript
    {
        get { return _ghostScript; }
        set { _ghostScript = value; }
    }

    public enum State
    {
        IDLE, FOLLOWING, ATTACKING, CHOKING
    }
    private State _currentState = State.IDLE;

	// Use this for initialization
	void Start () {
        AnimControlerScript.SelectAnimation(_currentState);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnParticleCollision(GameObject other) {
        if (_currentState != State.CHOKING)
            StartCoroutine("Choke");
    }

    IEnumerator Choke()
    {
        _currentState = State.CHOKING;
        AnimControlerScript.SelectAnimation(_currentState);

        if (NavScript != null)
        {
            NavScript.NavAgent.Stop();
            NavScript.enabled = false;
        }
        else if (GhostScript != null)
        {
            GhostScript.AllowedToMove = false;
        }
        yield return new WaitForSeconds(5);
        if (NavScript != null)
        {
            NavScript.enabled = true;
        }
        else if (GhostScript != null)
        {
            GhostScript.AllowedToMove = true;
        }

        _currentState = State.FOLLOWING;
        AnimControlerScript.SelectAnimation(_currentState);
    }
}