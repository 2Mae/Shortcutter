using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Shortcutter
{
	public interface ISuggestion
	{
		// public VisualElement visualElement;
		string label { get; }
		float score { get; set; }


		void Submit(string input);
		void Complete(string input);
	}

	public class Suggestion : ISuggestion
	{
		public string label { get; set; }
		public float score { get; set; }
		public StringEvent onComplete = new StringEvent();
		public StringEvent onSubmit = new StringEvent();
		public void Complete(string input) { onComplete.Invoke(input); }
		public void Submit(string input) { onSubmit.Invoke(input); }
		public Suggestion(string label)
		{
			this.label = label;
			onComplete.AddListener((input) => { Interpreter.UpdateTermField(label); });
			onSubmit.AddListener((input) => { Debug.Log(label); });
		}
	}

}
