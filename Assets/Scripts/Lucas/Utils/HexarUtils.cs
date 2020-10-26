﻿using System;
using UnityEngine;

namespace HexarUtils
{
    public static class CustomUtilities
    {
        /// <summary>
        /// Create Text in the world
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parent"></param>
        /// <param name="localPosition"></param>
        /// <param name="fontSize"></param>
        /// <param name="color"></param>
        /// <param name="textAnchor"></param>
        /// <param name="textAlignment"></param>
        /// <param name="sortingOrder"></param>
        /// <returns></returns>
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        /// <summary>
        /// Create Text in the world
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="text"></param>
        /// <param name="localPosition"></param>
        /// <param name="fontSize"></param>
        /// <param name="color"></param>
        /// <param name="textAnchor"></param>
        /// <param name="textAlignment"></param>
        /// <param name="sortingOrder"></param>
        /// <returns></returns>
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        // ========= MOUSE POSITION ===========
        /// <summary>
        /// Get Mouse position in World with Z = 0
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        /// <summary>
        /// Get Mouse Position in World
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        /// <summary>
        /// Get Mouse Position in World
        /// </summary>
        /// <param name="worldCamera"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        /// <summary>
        /// Get Mouse Position in World
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="worldCamera"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}
