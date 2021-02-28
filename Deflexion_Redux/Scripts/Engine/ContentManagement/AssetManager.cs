using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Deflexion_Redux {

    [Serializable]
    public enum TextureType {
        player_bottom,
        player_gun,
        player_shield,
        
        turret_bottom,
        turret_top,
        
        test_drone,
        
        test_tile,
        test_poopy_level,
        test_player_blast,
        test_enemy_blast,

        test_space_background,

        editor_test_1,
        editor_test_2,
        editor_test_3,

        deflexion_logo,
    }

    public enum FontType {
        arial
    }

    public enum LevelType {
        new_level,
        test_level,
    }

    public enum SoundType {
        test_song_intro,
        test_song_body,
    }

    public class AssetManager {

        public static Dictionary<TextureType, Texture2D> textures = new Dictionary<TextureType, Texture2D>();
        public static Dictionary<FontType, SpriteFont> fonts = new Dictionary<FontType, SpriteFont>();
        public static Dictionary<LevelType, Level> levels = new Dictionary<LevelType, Level>();
        public static Dictionary<SoundType, SoundEffect> sounds = new Dictionary<SoundType, SoundEffect>();

        public static void LoadTextures(ContentManager content) {
            
            // Normal Textures
            textures.Add(TextureType.player_bottom, content.Load<Texture2D>("Sprites/test_ship_bottom"));
            textures.Add(TextureType.player_shield, content.Load<Texture2D>("Sprites/test_shield_2"));
            textures.Add(TextureType.player_gun, content.Load<Texture2D>("Sprites/test_ship_top"));

            textures.Add(TextureType.turret_bottom, content.Load<Texture2D>("Sprites/turret_bottom"));
            textures.Add(TextureType.turret_top, content.Load<Texture2D>("Sprites/turret_top"));

            textures.Add(TextureType.test_drone, content.Load<Texture2D>("Sprites/drone_temp"));

            textures.Add(TextureType.test_poopy_level, content.Load<Texture2D>("poopyLevel"));
            textures.Add(TextureType.test_player_blast, content.Load<Texture2D>("Sprites/shotgunBlast"));
            textures.Add(TextureType.test_enemy_blast, content.Load<Texture2D>("Sprites/enemyBlast"));

            textures.Add(TextureType.test_space_background, content.Load<Texture2D>("Sprites/spaceBackground_test"));

            textures.Add(TextureType.deflexion_logo, content.Load<Texture2D>("Sprites/deflexionLogo"));

            // Tilesets
            textures.Add(TextureType.test_tile, content.Load<Texture2D>("Tilesets/Tile"));
            textures.Add(TextureType.editor_test_1, content.Load<Texture2D>("Tilesets/Editor_Testing"));
            textures.Add(TextureType.editor_test_2, content.Load<Texture2D>("Tilesets/Editor_Testing2"));
            textures.Add(TextureType.editor_test_3, content.Load<Texture2D>("Tilesets/Editor_Testing3"));

            // Fonts
            fonts.Add(FontType.arial, content.Load<SpriteFont>("Arial"));

            // Levels
            levels.Add(LevelType.test_level, XmlManager.Load<Level>(Path.Combine(content.RootDirectory, "testLevel2.xml")));

            // Sounds
            sounds.Add(SoundType.test_song_intro, content.Load<SoundEffect>("Audio/Xyralon (Intro)"));
            sounds.Add(SoundType.test_song_body, content.Load<SoundEffect>("Audio/Xyralon (Body)"));
        }
    }
}