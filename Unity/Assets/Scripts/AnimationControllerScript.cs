using UnityEngine;
using System.Collections;

public class AnimationControllerScript : MonoBehaviour {

    [SerializeField]
    private Animation _anim;
    public Animation Anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    // IDLE, FOLLOWING, ATTACKING, CHOKING
    [SerializeField]
    private AnimationClip _idleAnimation;
    public AnimationClip IdleAnimation
    {
        get { return _idleAnimation; }
        set { _idleAnimation = value; }
    }

    [SerializeField]
    private AnimationClip _walkAnimation;
    public AnimationClip WalkAnimation
    {
        get { return _walkAnimation; }
        set { _walkAnimation = value; }
    }

    private Animation _attackAnimation;

    [SerializeField]
    private AnimationClip _chokeAnimation;
    public AnimationClip ChokeAnimation
    {
        get { return _chokeAnimation; }
        set { _chokeAnimation = value; }
    }

	// Use this for initialization
	void Start () {
        Anim.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SelectAnimation(MonsterScript.State monsterState)
    {
        switch(monsterState)
        {
            case MonsterScript.State.IDLE:
                Anim.Play(IdleAnimation.name);
                break;
            case MonsterScript.State.FOLLOWING:
                Anim.Play(WalkAnimation.name);
                break;
            case MonsterScript.State.CHOKING:
                Anim.Play(ChokeAnimation.name);
                break;
        }
    }
}
