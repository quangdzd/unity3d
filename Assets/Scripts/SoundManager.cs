using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource ambientSource;
    public AudioSource sfxSource;

    [Header("Sound Library")]
    private Dictionary<string , AssetReferenceT<AudioClip> > audioLibrary = new Dictionary<String , AssetReferenceT<AudioClip>>() ;
    private Dictionary<string , AudioClip > cacheAudioLibrary = new Dictionary<String , AudioClip>() ;

    [Header("Sound Library")]
    public GameObject audioListener;
    private AssetReferenceT<AudioClip> currentMusicRef;
    public SoundLibrary soundLibrary;
    protected override void Awake()
    {
        base.Awake();
        audioListener = Camera.main.gameObject;

        foreach(Sound s in soundLibrary.soundUnits)
        {
            if(audioLibrary.ContainsKey(s.key))
            {

                continue;
            }
            else{
                audioLibrary.Add(s.key, s.clipRef);

            }

        }
    }
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlaySFXCache("click");
        }
    }

    public void LoadSFX(AssetReferenceT<AudioClip> clipRef)
    {

        clipRef.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> task)=>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = task.Result;
                sfxSource.PlayOneShot(audioClip);

                StartCoroutine(ReleaseAfterPlay(clipRef , audioClip.length));
            }
            else{
                Debug.LogWarning("Failed to load audio");
            }
        };

    }

    public void PlaySFX(string key)
    {
        if(audioLibrary.TryGetValue(key, out AssetReferenceT<AudioClip> clipRef))
        {
            LoadSFX(clipRef);
        }
        else
        {
            Debug.LogWarning(key + " is not found!!!");
        }
    }



    public void LoadSFXCache(string key , AssetReferenceT<AudioClip> clipRef)
    {

        clipRef.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> task)=>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = task.Result;

                if (!cacheAudioLibrary.ContainsKey(key))
                {
                    cacheAudioLibrary.Add(key, audioClip);

                }  

                sfxSource.PlayOneShot(audioClip);

            }
            else{
                Debug.LogWarning("Failed to load audio");
            }
        };

    }
    public void PlaySFXCache(string key , Transform source = null)
    {
        if (source != null && audioListener != null)
            {
                if (Vector3.Distance(source.position, audioListener.transform.position) > 150)
                {

                    return;
                }
            }
        if(cacheAudioLibrary.TryGetValue(key, out AudioClip clipCache))
        {
            sfxSource.PlayOneShot(clipCache);

        }
        else
        {
            if(audioLibrary.TryGetValue(key, out AssetReferenceT<AudioClip> clipRef))
            {
                LoadSFXCache(key , clipRef);

            }
            else
            {
                Debug.LogWarning(key + " is not found!!!");

            }   
        }
    }

    public void LoadAmbient(AssetReferenceT<AudioClip> clipRef)
    {

        clipRef.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> task)=>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = task.Result;
                ambientSource.PlayOneShot(audioClip);

                StartCoroutine(ReleaseAfterPlay(clipRef , audioClip.length));
            }
            else{
                Debug.LogWarning("Failed to load audio");
            }
        };

    }

    public void PlayAmbient(string key)
    {
        if(audioLibrary.TryGetValue(key, out AssetReferenceT<AudioClip> clipRef))
        {
            LoadAmbient(clipRef);
        }
        else
        {
            Debug.LogWarning(key + " is not found!!!");
        }
    }



    public void LoadAmbientCache(string key , AssetReferenceT<AudioClip> clipRef)
    {

        clipRef.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> task)=>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = task.Result;

                if (!cacheAudioLibrary.ContainsKey(key))
                {
                    cacheAudioLibrary.Add(key, audioClip);
                }  

                ambientSource.PlayOneShot(audioClip);
            }
            else{
                Debug.LogWarning("Failed to load audio");
            }
        };

    }
    public void PlayAmbientCache(string key)
    {
        if(cacheAudioLibrary.TryGetValue(key, out AudioClip clipCache))
        {
            ambientSource.PlayOneShot(clipCache);
        }
        else
        {
            if(audioLibrary.TryGetValue(key, out AssetReferenceT<AudioClip> clipRef))
            {
                LoadAmbientCache(key , clipRef);
            }
            else
            {
                Debug.LogWarning(key + " is not found!!!");
            }   
        }
    }



    public void LoadMusic(AssetReferenceT<AudioClip> clipRef , bool loop = true)
    {

        clipRef.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> task)=>
        {
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip audioClip = task.Result;
                musicSource.loop = loop;
                musicSource.clip = audioClip;
                musicSource.Play();

                if (currentMusicRef != null && currentMusicRef != clipRef)
                {
                    currentMusicRef.ReleaseAsset();
                }

                currentMusicRef = clipRef;
            }
            else{
                Debug.LogWarning("Failed to load audio");
            }
        };

    }

    public void PlayMusic(string key)
    {
        if(audioLibrary.TryGetValue(key, out AssetReferenceT<AudioClip> clipRef))
        {
            LoadMusic(clipRef);
        }
        else
        {
            Debug.LogWarning(key + " is not found!!!");
        }
    }







    public void ReleaseCache()
    {
        foreach(string key in cacheAudioLibrary.Keys)
        {
            AssetReferenceT<AudioClip> clipRef = audioLibrary[key];
            clipRef.ReleaseAsset();
        }

        cacheAudioLibrary.Clear();
    }

    public void SetMasterVolume(float level)
    {

    }
    public void SetSFXVolume(float level)
    {

    }

    public void SetSoundVolume(float level)
    {

    }


    IEnumerator ReleaseAfterPlay(AssetReferenceT<AudioClip> clipRef, float delay)
    {
        yield return new WaitForSeconds(delay);
        clipRef.ReleaseAsset();
    }
}




