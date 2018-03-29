﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using data;
using UnityEngine;
using UnityEngine.UI;
using DS = data.DataSingleton;
using E = main.Loader;

namespace wallSystem
{
	public class PickupGenerator : MonoBehaviour {
		private List<GameObject> _destroy;
		

		private static Data.Point ReadFromExternal(string inputFile){
			
			var p = new Process
			{
				StartInfo = new ProcessStartInfo("python",
					"Assets/InputFiles~/" + inputFile)
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					RedirectStandardInput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
 			p.Start ();
			p.StandardInput.Write(JsonUtility.ToJson(E.Get().CurrTrial.Value) +"\n");

			p.WaitForExit ();
			var line = p.StandardOutput.ReadLine();

			
			while (!p.StandardError.EndOfStream) {
				var outputLine = p.StandardError.ReadLine ();
				UnityEngine.Debug.LogError(outputLine);

			}

			if (line == null)
			{
				UnityEngine.Debug.LogError("PYTHON FILE ERROR!");
				return new Data.Point{X = 5, Y = 5};
			}
		
			var arr = line.Split (',');
			return new Data.Point
			{
				X = float.Parse(arr[0]),
				Y = float.Parse(arr[1])
			};

		}

		// Use this for initialization
		private void Start () {	
		
			var gen = GameObject.Find("WallCreator").GetComponent<GenerateGenerateWall>();

			_destroy = new List<GameObject>(); //This initializes the food object destroy list

			var goalText = GameObject.Find("Goal").GetComponent<Text>();
			goalText.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 30);


			var l1 = E.Get().CurrTrial.Value.ActiveGoals;
			var l2 = E.Get().CurrTrial.Value.InactiveGoals;
			var l3 = E.Get().CurrTrial.Value.InvisibleGoals;
			var hashset = new HashSet<int>(l2);
			var hashset2 = new HashSet<int>(l3);
			var hashset3 = new HashSet<int>(l1);
			
			var merged = new List<int>();
			merged.AddRange(l1);
			merged.AddRange(l2);
			merged.AddRange(l3);


			foreach (var val in merged)
			{
			
				var item = DS.GetData().Goals[Mathf.Abs(val) - 1];
				gen.SetWaveSrc(item.SoundLocation);


				//Here is the text to determine the type of food that exists here

				//And this section sets the text.
				goalText.text = E.Get().CurrTrial.Value.Header;
				goalText.color = Color.white;
				Data.Point p;

				if (item.Location == null)
				{
					p = ReadFromExternal(item.PythonFile);
					
				}
				else
				{
					try
					{
						p = new Data.Point {X = item.Location[0], Y = item.Location[2], Z = item.Location[1]};
					}
					catch (Exception _)
					{
						p = new Data.Point {X = item.Location[0], Y = 0.5f, Z = item.Location[1]};
						
					}
				}

				


				var prefab = (GameObject) Resources.Load("prefabs/" + item.Type, typeof(GameObject));

				var obj = Instantiate(prefab);
				if (!item.Type.Equals("2DImageDisplayer"))
					obj.AddComponent<RotateBlock>();

				obj.transform.localScale *= item.Size;
				obj.transform.position = new Vector3(p.X, p.Z, p.Y);
				var sprite = item.ImageLoc;
				if (sprite != null)
				{
					var pic = Img2Sprite.LoadNewSprite(Constants.InputDirectory + sprite);
					obj.GetComponent<SpriteRenderer>().sprite = pic;
				}

				var color = Data.GetColour(item.Color);
				try
				{
					obj.GetComponent<Renderer>().material.color = color;
					obj.GetComponent<Renderer>().enabled = !hashset2.Contains(val);
					obj.GetComponent<Collider>().enabled = !hashset.Contains(val);
					if (hashset3.Contains(val))
					{
						obj.tag = "Pickup";
					}
				}
				catch (Exception _)
				{
					print("Visibility not working");
				}

				_destroy.Add(obj);
			}
			GameObject.Find("FirstPerson").GetComponent<PlayerController>().ExternalStart(0, 0);

		}

		//And here we destroy all the food.
		private void OnDestroy()
		{
			foreach (var t in _destroy)
			{
				if (t != null) Destroy(t);
			}
		}
    
	}
}