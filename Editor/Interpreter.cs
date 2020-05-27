using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shortcutter
{
	public class StringEvent : UnityEvent<string> { }

	public class Interpreter
	{
		public List<ISuggestion> suggestions;
		public int listIndex=0;
		public ISuggestion selectedSuggestion => suggestions[listIndex];
		ShortcutHandler utopic; 

		public StringEvent updateInput = new StringEvent();
		public UnityEvent update = new UnityEvent();


		static Interpreter _instance;
		public static Interpreter instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Interpreter();
				}
				return _instance;
			}
		}
		public string info = " ";


		public Interpreter()
		{
			utopic = new ShortcutHandler();			
		}

		public static void UpdateTermField(string input)
		{
			Interpreter.instance.updateInput.Invoke(input);
		}

		public void OnFieldChange(string newInput)
		{
			utopic.OnChanged(newInput, out suggestions);
		}

		public void Complete(string input)
		{
			selectedSuggestion.Complete(input);

		}
		public void Submit(string input)
		{
			selectedSuggestion.Submit(input);
			UpdateTermField("");
		}
	}
}
