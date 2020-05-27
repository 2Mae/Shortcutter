using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Shortcutter
{
    public class ShortcutHandler
    {
        public static bool displayScore = false;

        public const string symbol = "/";

        public static string lastCall;
        const string temporaryProfile = "CommandPalette";
        async static public void TriggerShortcut(string shortcut)
        {
            IShortcutManager manager = ShortcutManager.instance;
            List<string> shortcuts = manager.GetAvailableShortcutIds().ToList();
            if (!shortcuts.Contains(shortcut))
            {
                Debug.LogWarning($"{shortcut} is not a shortcut!");
                return;
            }

            if (manager.GetAvailableProfileIds().Contains(temporaryProfile))
            {
                manager.DeleteProfile(temporaryProfile);
            }
            manager.CreateProfile(temporaryProfile);
            string previousProfile = manager.activeProfileId;
            manager.activeProfileId = temporaryProfile;
            manager.ClearShortcutOverride(shortcut);
            var binding = new ShortcutBinding(new KeyCombination(KeyCode.F12));
            manager.RebindShortcut(shortcut, binding);
            EditorWindow.focusedWindow.SendEvent(Event.KeyboardEvent("F12"));

            await Task.Delay(100);

            manager.activeProfileId = previousProfile;
            manager.DeleteProfile(temporaryProfile);

        }

        static public List<ISuggestion> Shortcuts(string input)
        {


            IEnumerable<string> e = ShortcutManager.instance.GetAvailableShortcutIds();
            List<string> shortcuts = e.ToList();

            List<ISuggestion> suggestions = new List<ISuggestion>();

            for (int i = 0; i < shortcuts.Count; i++)
            {
                
                EvaluateShortcut(input, shortcuts[i], ref suggestions);
            }
            return suggestions.Take(40).ToList();
        }

        public static void EvaluateShortcut(string input, string rawShortcut, ref List<ISuggestion> suggestions)
        {
            string shortcut = rawShortcut.ToLower();
            char[] separator = new char[] { '/', ' ' };
            string[] inputparts = input.Split(separator);
            float score = 0;

            int lastSlash = shortcut.LastIndexOf('/') + 1;
            if (lastSlash < 0) { lastSlash = 0; }

            string[] shortcutparts = shortcut.Split(separator);
            for (int j = 0; j < shortcutparts.Length; j++)
            {
                if (inputparts.Contains(shortcutparts[j]))
                {
                    score += (j + 1);
                }
            }

            string tip = shortcut.Substring(lastSlash);
            for (int j = 0; j <= Mathf.Min(tip.Length, input.Length); j++)
            {
                if (string.Equals(input.Substring(0, j), tip.Substring(0, j), StringComparison.InvariantCultureIgnoreCase))
                {
                    score += 1;
                }
                else
                {
                    {
                        break;
                    }
                }
            }


            ShortcutSuggestion suggestion = new ShortcutSuggestion(rawShortcut);
            suggestion.score = score;

            bool inserted = false;
            for (int j = 0; j < suggestions.Count; j++)
            {
                if (score > suggestions[j].score)
                {
                    suggestions.Insert(j, suggestion);
                    inserted = true;
                    break;
                }
            }
            if (!inserted)
            {
                suggestions.Add(suggestion);
            }
        }

        public void OnChanged(string input, out List<ISuggestion> suggestions)
        {
            suggestions = Shortcuts(input);
        }

        public class ShortcutSuggestion : ISuggestion
        {
            public string shortcut;
            public float score { get; set; }
            public string label
            {
                get
                {
                    string debugInfo = "";
                    if (displayScore){debugInfo = $"[{score}] ";}

                    return $"{debugInfo}{symbol}{shortcut}";
                }
            }

            public StringEvent onComplete = new StringEvent();
            public StringEvent onSubmit = new StringEvent();
            public void Complete(string input) { onComplete.Invoke(input); }
            public void Submit(string input) { onSubmit.Invoke(input); }
            public ShortcutSuggestion(string shortcut)
            {
                this.shortcut = shortcut;
                onComplete.AddListener((input) => { Interpreter.UpdateTermField(label); });
                onSubmit.AddListener((input) => { TriggerShortcut(shortcut); });
            }
        }
    }
}