using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace SparsSorcerousSundries.Utilities
{
    public class AssetLoaderExtensions
    {
        //stolen from Vek to try and get a portrait into the game
        public static Sprite LoadInternalPortrait(ModContextBase modContext, string folder, string file, Vector2Int size, TextureFormat format)
        {
            return Image2Sprite.Create($"{modContext.ModEntry.Path}Assets{Path.DirectorySeparatorChar}{folder}{Path.DirectorySeparatorChar}{file}", size, format);
        }
        // Loosely based on https://forum.unity.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
        public static class Image2Sprite
        {
            public static string icons_folder = "";
            public static Sprite Create(string filePath, Vector2Int size, TextureFormat format)
            {
                var bytes = File.ReadAllBytes(icons_folder + filePath);
                var texture = new Texture2D(size.x, size.y, format, false);
                _ = texture.LoadImage(bytes);
                var sprite = Sprite.Create(texture, new Rect(0, 0, size.x, size.y), new Vector2(0, 0));
                return sprite;
            }
        }
        //public static class Audio2Clip
        //{
        //    public static string icons_folder = "";
        //    public static AudioClip Create(string filePath, Vector2Int size, TextureFormat format)
        //    {
        //        var bytes = File.ReadAllBytes(icons_folder + filePath);
        //        var texture = new Texture2D(size.x, size.y, format, false);
        //        _ = texture.LoadImage(bytes);
        //        var sprite = Sprite.Create(texture, new Rect(0, 0, size.x, size.y), new Vector2(0, 0));
        //        return sprite;
        //    }
        //}

    }
}
