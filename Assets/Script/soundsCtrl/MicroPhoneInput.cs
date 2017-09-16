using System;  
using System.Collections.Generic;  
using System.IO;  
using System.Linq;  
using System.Text;  
using UnityEngine;  
using System.Collections;
using AssemblyCSharp;

  
[RequireComponent (typeof(AudioSource))]  

public class MicroPhoneInput : MonoBehaviour {  

	private static MicroPhoneInput m_instance;  

	public float sensitivity=100;  
	public float loudness=0;  

	private AudioSource playAudio;

	private static string[] micArray=null;  

	const int HEADER_SIZE = 44;  

	const int RECORD_TIME = 10;  
	List<int> userList;
	private AudioClip redioclip;
	// Use this for initialization   
	void Start () {  
		SocketEventHandle.getInstance ().micInputNotice += micInputNotice;
		playAudio = GameObject.Find ("GamePlayAudio").GetComponent<AudioSource> ();
		if (playAudio.clip == null) {
			playAudio.clip = AudioClip.Create("playRecordClip", 160000, 1, 8000, false, false);  
		}
	}  

	// Update is called once per frame   
	void Update ()  
	{  
		//loudness = GetAveragedVolume () * sensitivity;  
	}  

	public static MicroPhoneInput getInstance()  
	{  
		if (m_instance == null)   
		{  
			micArray = Microphone.devices;  
			if (micArray.Length == 0)  
			{  
				Debug.LogError ("Microphone.devices is null");  
			}  
			foreach (string deviceStr in Microphone.devices)  
			{  
				Debug.Log("device name = " + deviceStr);  
			}  
			if(micArray.Length==0)  
			{  
				Debug.LogError("no mic device");  
			}  

			GameObject MicObj=new GameObject("MicObj");  
			m_instance= MicObj.AddComponent<MicroPhoneInput>();  
		}  
		return m_instance;  
	}  
		
	public void StartRecord(List<int> _userList)  
	{  
		userList = _userList;
		GetComponent<AudioSource>().Stop();  
		if (micArray.Length == 0)  
		{  
			Debug.Log("No Record Device!");  
			return;  
		}  
		//GetComponent<AudioSource>().loop = false;  
		//GetComponent<AudioSource>().mute = true;  
		redioclip = Microphone.Start("inputMicro", false, RECORD_TIME, 8000); //22050    
		while (!(Microphone.GetPosition(null)>0)) {  
		}  
	//	GetComponent<AudioSource>().Play ();  
		Debug.Log("StartRecord");  
		//倒计时   
		//StartCoroutine(TimeDown());  
	}  

	public  void StopRecord()  
	{  
		MyDebug.Log("StopRecord");  
		if (micArray.Length == 0)  
		{  
			Debug.Log("No Record Device!");  
			return;  
		}  
		if (!Microphone.IsRecording(null))  
		{  
			return;  
		}  
		Microphone.End (null);  
		GetComponent<AudioSource>().clip =redioclip ;  
		ChatSocket.getInstance ().sendMsg (new MicInputRequest(userList,GetClipData()));
		PlayRecord ();
	}  

	public Byte[] GetClipData()  
	{  
		if (GetComponent<AudioSource>().clip == null)  
		{  
			Debug.Log("GetClipData audio.clip is null");  
			return null;   
		}  

		float[] samples = new float[GetComponent<AudioSource>().clip.samples];  
		MyDebug.Log ("samples.Length = "+samples.Length);
		GetComponent<AudioSource>().clip.GetData(samples, 0);  


		Byte[] outData = new byte[samples.Length * 2];  
		//Int16[] intData = new Int16[samples.Length];   
		//converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]   

		int rescaleFactor = 32767; //to convert float to Int16   
		for (int i = 0; i < samples.Length; i++)  
		{  
			short temshort = (short)(samples[i] * rescaleFactor);  
			Byte[] temdata=System.BitConverter.GetBytes(temshort);  
			outData[i*2]=temdata[0];  
			outData[i*2+1]=temdata[1];  
		}  
		if (outData == null || outData.Length <= 0)  
		{  
			Debug.Log("GetClipData intData is null");  
			return null;   
		}  
		//return intData;   
		return outData;  
	}  


	public void PlayClipData(Int16[] intArr)  
	{  
		if (intArr.Length == 0)  
		{  
			Debug.Log("get intarr clipdata is null");  
			return;  
		}  
		MyDebug.Log ("PlayClipData");
		//从Int16[]到float[]   
		float[] samples = new float[intArr.Length];  
		int rescaleFactor = 32767;  
		for (int i = 0; i < intArr.Length; i++)  
		{  
			samples[i] = (float)intArr[i] / rescaleFactor;  
		}  
			
		playAudio.clip.SetData(samples, 0);  
		playAudio.mute = false;  
		playAudio.Play();  
	}

	private void PlayRecord()  
	{  
		if (GetComponent<AudioSource>().clip == null)  
		{  
			Debug.Log("audio.clip=null");  
			return;  
		}  
		GetComponent<AudioSource>().mute = false;  
		GetComponent<AudioSource>().loop = false;  
		GetComponent<AudioSource>().Play ();  

	}  
		

	public  float GetAveragedVolume()  
	{  
		float[] data=new float[256];  
		float a=0;  
		GetComponent<AudioSource>().GetOutputData(data,0);  
		foreach(float s in data)  
		{  
			a+=Mathf.Abs(s);  
		}  
		return a/256;  
	}  



	private IEnumerator TimeDown()  
	{  
		int time = 0;  
		while (time < RECORD_TIME)  
		{  
			if (!Microphone.IsRecording (null))   
			{ //如果没有录制   
				Debug.Log ("IsRecording false");  
				yield break;  
			}  
			Debug.Log("yield return new WaitForSeconds "+time);  
			yield return new WaitForSeconds(1);  
			time++;  
		}  
		if (time >= RECORD_TIME)  
		{  
			MyDebug.Log("RECORD_TIME is out! stop record!");  
			StopRecord();  
		}  
		yield return 0;  
	}

	public void micInputNotice(ClientResponse response){
		MyDebug.Log ("micInputNotice");
		if(GlobalDataScript.soundToggle){
			byte[] data = response.bytes;
			int i = 0;
			List<short> result = new List<short>();
			while(data.Length - i >= 2)
			{
				result.Add(BitConverter.ToInt16(data,i));
				i += 2;
			}
			Int16[] arr = result.ToArray();//这就是你要的
			PlayClipData(arr);
		}
	}
	//save to localhost
	public bool Save(string filename) {  
		MyDebug.Log("Application.persistentDataPath = "+Application.persistentDataPath);  

		AudioClip clip = GetComponent<AudioSource> ().clip;

		if (!filename.ToLower().EndsWith(".wav")) {  
			filename += ".wav";  
		}  

		string filepath = Path.Combine(Application.persistentDataPath, filename);  

		Debug.Log(filepath);  

		// Make sure directory exists if user is saving to sub dir.  
		Directory.CreateDirectory(Path.GetDirectoryName(filepath));  

		using (FileStream fileStream = CreateEmpty(filepath)) {  

			ConvertAndWrite(fileStream, clip);  

			WriteHeader(fileStream, clip);  
		}  

		return true; // TODO: return false if there's a failure saving the file  
	}

	private FileStream CreateEmpty(string filepath) {  
		FileStream fileStream = new FileStream(filepath, FileMode.Create);  
		byte emptyByte = new byte();  

		for(int i = 0; i < HEADER_SIZE; i++) //preparing the header  
		{  
			fileStream.WriteByte(emptyByte);  
		}  

		return fileStream;  
	} 

	private void ConvertAndWrite(FileStream fileStream, AudioClip clip) {  

		float[] samples = new float[clip.samples];  

		clip.GetData(samples, 0);  

		Int16[] intData = new Int16[samples.Length];  

		Byte[] bytesData = new Byte[samples.Length * 2];  

		int rescaleFactor = 32767; //to convert float to Int16  

		for (int i = 0; i<samples.Length; i++) {  
			intData[i] = (short) (samples[i] * rescaleFactor);  
			Byte[] byteArr = new Byte[2];  
			byteArr = BitConverter.GetBytes(intData[i]);  
			byteArr.CopyTo(bytesData, i * 2);  
		}  

		fileStream.Write(bytesData, 0, bytesData.Length);  
	}  

	private void WriteHeader(FileStream fileStream, AudioClip clip) {  

		int hz = clip.frequency;  
		int channels = clip.channels;  
		int samples = clip.samples;  

		fileStream.Seek(0, SeekOrigin.Begin);  

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");  
		fileStream.Write(riff, 0, 4);  

		Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);  
		fileStream.Write(chunkSize, 0, 4);  

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");  
		fileStream.Write(wave, 0, 4);  

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");  
		fileStream.Write(fmt, 0, 4);  

		Byte[] subChunk1 = BitConverter.GetBytes(16);  
		fileStream.Write(subChunk1, 0, 4);  

		UInt16 two = 2;  
		UInt16 one = 1;  

		Byte[] audioFormat = BitConverter.GetBytes(one);  
		fileStream.Write(audioFormat, 0, 2);  

		Byte[] numChannels = BitConverter.GetBytes(channels);  
		fileStream.Write(numChannels, 0, 2);  

		Byte[] sampleRate = BitConverter.GetBytes(hz);  
		fileStream.Write(sampleRate, 0, 4);  

		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
		fileStream.Write(byteRate, 0, 4);  

		UInt16 blockAlign = (ushort) (channels * 2);  
		fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);  

		UInt16 bps = 16;  
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);  
		fileStream.Write(bitsPerSample, 0, 2);  

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");  
		fileStream.Write(datastring, 0, 4);  

		Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);  
		fileStream.Write(subChunk2, 0, 4);  

		//      fileStream.Close();  
	}  
}