using System;
using UnityEngine.UIElements;



namespace WS.WSToolbar
{
    public static class WSElementUtility
    {
        public static TextField CreateTextField(string value, string label, EventCallback<ChangeEvent<string>> callback)
        {
            var textField = new TextField(label)
            {
                value = value
            };
            textField.RegisterValueChangedCallback(callback);
            return textField;
        }

        public static Button CreateButton(string text, Action clickEvent)
        {
            return new Button(clickEvent)
            {
                text = text
            };
        }
    }

// Utility extension methods to remove whitespace and special characters
    public static class StringExtensions
    {
        public static string RemoveWhitespaces(this string input)
        {
            return string.Join("", input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string RemoveSpecialCharacters(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9]", "");
        }
    }
}