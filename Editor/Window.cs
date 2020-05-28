using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Shortcutter
{
	public class Window : EditorWindow
	{
		List<string> items = new List<string>();
		private const string theTitle = "Shortcutter";
		Label infoLabel;
		Interpreter seashell => Interpreter.instance;

		TextField textField;
		string field
		{
			get
			{
				return textField.value;
			}
			set
			{
				textField.value = value;
			}
		}
		ListView listView;

		[MenuItem("Window/" + theTitle)]
		public static void ShowWindow() { GetWindow<Window>().OnShow(); }

		public void UpdateInput(string input)
		{
			if (field != input)
			{
				field = input;
				// seashell.listIndex = 0;
			}
			UpdateStuff();
		}

		public void OnShow()
		{

			titleContent = new GUIContent(theTitle);
			minSize = new Vector2(400, 32);

			textField.Focus();
		}

		public void OnEnable()
		{
			seashell.updateInput.AddListener(UpdateInput);
			seashell.update.AddListener(UpdateStuff);

			InitiateTextField();
			InitiateListView();

			// rootVisualElement.Add(new ObjectField() { objectType = typeof(Transform) });

			// rootVisualElement.Add(new InspectorElement(FindObjectOfType<Camera>()));

			infoLabel = new Label(seashell.info);
			//rootVisualElement.Add(infoLabel);
			rootVisualElement.Add(textField);
			rootVisualElement.Add(listView);
		}

		private void OnDisable()
		{
			seashell.update.RemoveListener(UpdateStuff);

		}
		public void UpdateStuff()
		{
			infoLabel.text = $" {seashell.info}";
			listView.itemsSource = seashell.suggestions;
			// listView.selectedIndex = seashell.listIndex;
		}

		private void OnKeyDown(KeyDownEvent e)
		{
			switch (e.keyCode)
			{
				case KeyCode.DownArrow:
					OnDown();
					e.PreventDefault();
					break;
				case KeyCode.UpArrow:
					OnUp();
					e.PreventDefault();
					break;
				case KeyCode.Return:
					seashell.Submit(field);
					// seashell.listIndex = 0;
					break;
				case KeyCode.Tab:
					seashell.Complete(field);
					textField.Focus();
					e.PreventDefault();
					break;
			}
		}

		private void TextFieldChange(ChangeEvent<string> changeEvent)
		{

			listView.selectedIndex = 0;

			//todo: watch out for recursion
			seashell.OnFieldChange(textField.value);
			UpdateStuff();
		}

		private void OnUp()
		{
			if (listView.selectedIndex > 0)
			{
				listView.selectedIndex--;
			}
		}

		private void OnDown()
		{
			if (listView.selectedIndex < listView.itemsSource.Count - 1)
			{
				listView.selectedIndex++;
			}
		}



		private void InitiateTextField()
		{
			textField = new TextField();
			textField.RegisterCallback<KeyDownEvent>(OnKeyDown);
			textField.RegisterValueChangedCallback<string>(TextFieldChange);
			textField.delegatesFocus = true;//Not sure what this one does, but needed for focus to work
		}


		public void InitiateListView()
		{
			Func<VisualElement> makeItem = () => new Label();
			Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = seashell.suggestions[i].label;
			const int itemHeight = 16;

			listView = new ListView(seashell.suggestions, itemHeight, makeItem, bindItem);

			listView.style.flexGrow = 1.0f;
			listView.RegisterCallback<FocusEvent>(OnListFocus);
			listView.onSelectionChanged += OnSelectionChanged;
		}

		private void OnSelectionChanged(List<object> obj)
		{
			seashell.listIndex = listView.selectedIndex;
		}

		private async void OnListFocus(FocusEvent evt)
		{
			textField.Focus();
			await Task.Delay(20);

			textField.SelectRange(textField.value.Length, textField.value.Length);
		}
	}
}

