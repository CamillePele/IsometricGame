using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class Editor
    {
        public static void DisplaySprite(Sprite sprite, float maxSize)
        {
            if (sprite == null)
            {
                sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            }
            
            // Actual rect
            Rect actualRect = EditorGUILayout.GetControlRect(GUILayout.Height(0));
            
            // Display entity sprite
            float spriteRatio = sprite.rect.width / sprite.rect.height;

            float width = 0;
            float height = 0;
                
            if (spriteRatio > 1)
            {
                width = maxSize;
                height = maxSize / spriteRatio;
            }
            else
            {
                height = maxSize;
                width = maxSize * spriteRatio;
            }
            Rect spriteRect = EditorGUILayout.GetControlRect(GUILayout.Width(width), GUILayout.Height(height));
            spriteRect.x += (actualRect.width - width) / 2;

            EditorGUI.DrawTextureTransparent(spriteRect, sprite.texture);
        }
    }
}