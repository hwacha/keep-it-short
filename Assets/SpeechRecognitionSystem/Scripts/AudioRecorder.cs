using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using SpeechRecognitionSystem;

public class AudioRecorder : MonoBehaviour, IMicrophone {
    public int MicrophoneIndex = 0;
    public int GetRecordPosition( ) {
        return Microphone.GetPosition( Microphone.devices [ MicrophoneIndex ] );
    }
    public AudioClip GetAudioClip( ) {
        return _audioClip;
    }
    public bool IsRecording( ) {
        return Microphone.IsRecording( Microphone.devices [ MicrophoneIndex ] );
    }

    [System.Serializable]
    public class MicReadyEvent : UnityEvent<IMicrophone> { }

    public MicReadyEvent MicReady = new MicReadyEvent( );

    private void Awake( ) {
        if ( Application.platform == RuntimePlatform.Android ) {
            if ( !Permission.HasUserAuthorizedPermission( Permission.Microphone ) ) {
                Permission.RequestUserPermission( Permission.Microphone );
            }
        }

        for (int i = 0; i < Microphone.devices.Length; i++) {
            Debug.Log(Microphone.devices[i] + ": " + i);
        }
    }

    private void FixedUpdate( ) {
        bool micAutorized = true;
        if ( Application.platform == RuntimePlatform.Android ) {
            micAutorized = Permission.HasUserAuthorizedPermission( Permission.Microphone );
        }
        if ( micAutorized ) {
            if ( _firstLoad ) {
                _audioClip = Microphone.Start( Microphone.devices [ MicrophoneIndex ], true, LENGTH_SEC, FREQ );
                this.MicReady?.Invoke( this );
                _firstLoad = false;
            }
        }

        Debug.Log(IsRecording());
        Debug.Log(GetAudioClip());
    }
    private void OnDestroy( ) {
        Microphone.End( Microphone.devices [ MicrophoneIndex ] );
        _firstLoad = true;
    }

    private bool _firstLoad = true;
    private AudioClip _audioClip = null;
    private const int LENGTH_SEC = 2;
    private const int FREQ = 16000;
}
