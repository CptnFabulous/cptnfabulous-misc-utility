using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class TextUtility
    {
        public static bool CharMatches(char c, List<char> array)
        {
            int index = array.IndexOf(c);
            //Debug.Log(c + ", " + array + ", " + WithinArray(index, array.Count));
            return MathUtility.WithinArray(index, array.Count);
        }
        public static bool IsUppercase(char c) => CharMatches(c, uppercaseLetters);
        public static bool IsLowercase(char c) => CharMatches(c, lowercaseLetters);
        public static char Capitalise(char c)
        {
            int lowercaseIndex = lowercaseLetters.IndexOf(c);
            if (MathUtility.WithinArray(lowercaseIndex, lowercaseLetters.Count))
            {
                c = uppercaseLetters[lowercaseIndex];
            }
            return c;
        }

        public static readonly List<char> uppercaseLetters = new List<char>(new char[26]
        {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        });
        public static readonly List<char> lowercaseLetters = new List<char>(new char[26]
        {
        'a',
        'b',
        'c',
        'd',
        'e',
        'f',
        'g',
        'h',
        'i',
        'j',
        'k',
        'l',
        'm',
        'n',
        'o',
        'p',
        'q',
        'r',
        's',
        't',
        'u',
        'v',
        'w',
        'x',
        'y',
        'z',
        });

        /// <summary>
        /// Formats a name string to start with a capital letter and have spaces.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FormatNameForPresentation(string text)
        {
            // Display name of binding
            // Cut off everything before the binding name
            int lastSlashBeforeName = text.LastIndexOf("/");
            string displayName = text.Remove(0, lastSlashBeforeName + 1);
            // Capitalise and format binding name

            //Debug.Log(displayName.Length);
            if (displayName.Length > 0) displayName = Capitalise(displayName[0]) + displayName.Remove(0, 1);

            int displayNameIndex = 1;
            while (displayNameIndex < displayName.Length)
            {
                if (IsUppercase(displayName[displayNameIndex]))
                {
                    displayName = displayName.Insert(displayNameIndex, " ");
                    displayNameIndex++; // Increase the index a second time to account for the string length being increased by one
                }
                displayNameIndex++;

            }
            //Debug.Log(displayName);

            return displayName;
        }

        /// <summary>
        /// Formats a name string to start with a capital letter and have spaces.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FormatNameForPresentation(string text, List<char> addSpaces, List<char> replaceWithSpaces)
        {
            int index = 0;
            while (index < text.Length)
            {
                char current = text[index];
                if ((CharMatches(current, addSpaces) || IsUppercase(current)) && index > 0)
                {
                    Debug.Log(text + ": " + current + " needs a space added");
                    text = text.Insert(index, " ");
                    index += 2; // Increase the index a second time to account for the string length being increased by one
                    break;
                }
                else if (CharMatches(current, replaceWithSpaces))
                {
                    text = text.Remove(index, 1);
                    if (index > 0)
                    {
                        text = text.Insert(index, " ");
                        index++;
                    }
                    break;
                }
                else if (index == 0)
                {
                    text = Capitalise(text[0]) + text.Remove(0, 1);
                    index++;
                    break;
                }
                else
                {
                    index++;
                }
            }
            //Debug.Log(text);

            return text;
        }
    }
}
