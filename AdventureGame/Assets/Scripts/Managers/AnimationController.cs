using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
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
        StartCoroutine(EPlayAnimation(animator, collectionName, stateName, stateLayer, waitForEnd, forceState));
    }

    private IEnumerator EPlayAnimation(Animator animator, string collectionName, string animationName, int animLayer = 0, bool waitForEnd = false, bool forceAnim = false)
    {
        AnimationClip clip = AnimationController.Instance.GetAnimationClip(collectionName, animationName);
        int animHash = clip.GetHashCode();

        if (animator.GetCurrentAnimatorStateInfo(animLayer).fullPathHash != animHash)
        {
            if (waitForEnd && animator.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1)
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

        if(forceAnim)
        {
            animator.Play(clip.name);
        }
    }
}