using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/// <summary>
/// AnimationManager: Manage the animations and the collection of Animation Clips of the game.
/// Note: animation state MUST be inside the GameObject's Animator
/// </summary>
public class AnimationManager : MonoBehaviour
{
    public bool Initialize()
    {
        return true;
    } 

    [Serializable]
    public class AnimCollection
    {
        public string CollectionName;
        public List<ClipAnimation> ClipList;

        [Serializable]
        public class ClipAnimation
        {
            public string AnimationName;
            public AnimationClip AnimationClip;
        }
    }

    public List<AnimCollection> Collection;

    public AnimationClip GetAnimationClip(string collection, string animName)
    {
        AnimCollection col;
        for(int i = 0; i < Collection.Count; i++)
        { 
            if(collection == Collection[i].CollectionName)
            {
                col = Collection[i];
                for (int j = 0; j < col.ClipList.Count; j++)
                {
                    if(animName == col.ClipList[j].AnimationName)
                    {
                        return col.ClipList[j].AnimationClip;
                    }
                }
            }
        }
        return null;
    }

    public void PlayAnimation(Animator animator, string collectionName, string stateName, int stateLayer = 0, bool waitForEnd = false, bool forceState = false)
    {
        AnimationClip clip = GetAnimationClip(collectionName, stateName);

        if(forceState)
        {
            animator.Play(clip.name);
        }
        else
        {
            if (IsDifferentAnimation(animator, stateLayer, clip))
            {
                animator.Play(clip.name);
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(stateLayer).normalizedTime == 1)
                {
                    animator.Play(clip.name);
                }
            }
        }

        if (waitForEnd)
        {
            StartCoroutine(EPlayAnimation(animator, clip, stateLayer));
        }
        
    }

    private IEnumerator EPlayAnimation(Animator animator, AnimationClip clip, int animLayer = 0)
    {
        int animHash = clip.GetHashCode();

        if (IsDifferentAnimation(animator, animLayer, clip))
        {
            if ( animator.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1)
            {
                animator.CrossFadeInFixedTime(clip.name, 0.3f);

                while (animator.GetCurrentAnimatorStateInfo(animLayer).fullPathHash != animHash)
                {
                    yield return null;
                }

                float counter = 0;
                float waitTime = animator.GetCurrentAnimatorStateInfo(animLayer).length;

                while (counter < waitTime)
                {
                    counter += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                animator.Play(clip.name);
            }
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(animLayer).normalizedTime == 1)
            {
                animator.Play(clip.name);
            }
        }
    }

    private bool IsDifferentAnimation(Animator animator, int animLayer, AnimationClip clip)
    {
        if (animator.GetCurrentAnimatorStateInfo(animLayer).fullPathHash == clip.GetHashCode())
        {
            return false;
        }
        return true;

    }
}