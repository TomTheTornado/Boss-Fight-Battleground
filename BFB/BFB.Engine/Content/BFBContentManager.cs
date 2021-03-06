using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio; 
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace BFB.Engine.Content
{
    /// <summary>
    /// A modified version of the Monogame ContentManager. Rather than extending it, one of the properties is a Monogame ContentManager.
    /// </summary>
    public class BFBContentManager
    {

        #region Properties
        
        public  GraphicsDevice GraphicsDevice { get; set; }

        private readonly ContentManager _contentManager;
        
        private readonly  Dictionary<string, Texture2D> _textureContent;
        
        private readonly Dictionary<string, AnimatedTexture> _animatedTexturesContent;

        private readonly Dictionary<string, AtlasTexture> _atlasTexturesContent;
            
        private readonly  Dictionary<string, SpriteFont> _fontContent;

        private readonly  Dictionary<string, Song> _audioSongContent;

        private readonly Dictionary<string, SoundEffect> _audioSoundEffectContent; 


        #endregion

        #region Constructor

        /// <summary>
        /// A constructor that does constructor things: initializes parameters.
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="graphicsDevice"></param>
        public BFBContentManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            GraphicsDevice = graphicsDevice;
            _textureContent = new Dictionary<string, Texture2D>();
            _animatedTexturesContent = new Dictionary<string, AnimatedTexture>();
            _atlasTexturesContent = new Dictionary<string, AtlasTexture>();
            _fontContent = new Dictionary<string, SpriteFont>();
            _audioSongContent = new  Dictionary<string, Song>();  
            _audioSoundEffectContent = new Dictionary<string, SoundEffect>();
        }
        
        #endregion
        
        #region AddTexture

        /// <summary>
        /// Adds a static texture to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <param name="texture"></param>
        public void AddTexture(string textureKey, Texture2D texture)
        {
            if (!_textureContent.ContainsKey(textureKey))
                _textureContent.Add(textureKey, texture);
        }
        
        #endregion
        
        #region GetTexture
        /// <summary>
        /// Returns a loaded texture.
        ///
        /// Throws a KeyNotFoundException when the key is not found.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <returns></returns>
        public Texture2D GetTexture(string textureKey)
        {
            if(string.IsNullOrEmpty(textureKey))
                throw new ArgumentNullException(nameof(textureKey),"The textureKey provided was null. Try supplying a valid string");
            
            return _textureContent.ContainsKey(textureKey) ? _textureContent[textureKey] : throw new KeyNotFoundException($"The textureKey: {textureKey} was not found.");
        } 
        
        #endregion

        #region UnloadTexture
        /// <summary>
        /// Unloads the texture if it will never be used again.
        /// </summary>
        /// <param name="textureKey"></param>
        public void UnloadTexture(string textureKey)
        {
            if (_textureContent.ContainsKey(textureKey)) 
                return;
            
            _textureContent[textureKey].Dispose();
            _textureContent.Remove(textureKey);
        }


        #endregion

        #region AddSoundEffectAudio
        /// <summary>
        /// Adds the loaded audio sound effects to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="audioKey"></param>
        /// <param name="audio"></param>
        public void AddSoundEffectAudio(string audioKey, SoundEffect audio) 
        {
            if (!_audioSoundEffectContent.ContainsKey(audioKey))
                _audioSoundEffectContent.Add(audioKey, audio);
        }

        #endregion

        #region GetSoundEffectAudio
        /// <summary>
        /// Returns the loaded audio for sound effects from the dictionary.
        /// </summary>
        /// <param name="audioKey"></param>
        /// <returns></returns>
        public SoundEffect GetSoundEffectAudio(string audioKey) 
        {
            return _audioSoundEffectContent.ContainsKey(audioKey) ? _audioSoundEffectContent[audioKey] : throw new KeyNotFoundException($"The audioKey: {audioKey} was not found.");
        }

        #endregion

        #region UnloadSoundEffectAudio
        /// <summary>
        /// Unloads the loaded audio sound effects if we'll never use it again. Also disposes it.
        /// </summary>
        /// <param name="audioKey"></param>
        public void UnloadSoundEffectAudio(string audioKey) 
        {
            if (_audioSoundEffectContent.ContainsKey(audioKey))
                return;

            _audioSoundEffectContent[audioKey].Dispose();
            _audioSoundEffectContent.Remove(audioKey);
        }


        #endregion

        #region AddSongAudio
        /// <summary>
        /// Adds the loaded audio songs to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="audioKey"></param>
        /// <param name="audio"></param>
        public void AddSongAudio(string audioKey, Song audio) 
        {
            if (!_audioSongContent.ContainsKey(audioKey))
                _audioSongContent.Add(audioKey, audio);
        }

        #endregion

        #region GetSongAudio
        /// <summary>
        /// Returns the loaded audio for songs from the dictionary.
        /// </summary>
        /// <param name="audioKey"></param>
        /// <returns></returns>
        [CanBeNull]
        public Song GetSongAudio(string audioKey)
        {
            return _audioSongContent.ContainsKey(audioKey) ? _audioSongContent[audioKey] : null;
        }

        #endregion

        #region UnloadSongAudio
        /// <summary>
        /// Unloads the loaded audio songs if we'll never use it again. Also disposes it.
        /// </summary>
        /// <param name="audioKey"></param>
        public void UnloadSongAudio(string audioKey)
        {
            if (_audioSongContent.ContainsKey(audioKey))
                return;

            _audioSongContent[audioKey].Dispose();
            _audioSongContent.Remove(audioKey);
        }


        #endregion

        #region AddFont
        /// <summary>
        /// Adds the loaded font to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="fontKey"></param>
        /// <param name="font"></param>
        public void AddFont(string fontKey, SpriteFont font)
        {
            if (!_fontContent.ContainsKey(fontKey))
                _fontContent.Add(fontKey, font);
        }
        
        #endregion
        
        #region getFont
        /// <summary>
        /// Returns the loaded font that you wanna use.
        /// </summary>
        /// <param name="fontKey"></param>
        /// <returns></returns>
        public SpriteFont GetFont(string fontKey)
        {
            return _fontContent.ContainsKey(fontKey) ? _fontContent[fontKey] :  throw new KeyNotFoundException($"The fontKey: {fontKey} was not found.");
        }
        
        #endregion
        
        #region UnloadFont
        /// <summary>
        /// Unloads the font.
        /// </summary>
        /// <param name="fontKey"></param>
        public void UnloadFont(string fontKey)
        {
            if (_fontContent.ContainsKey(fontKey)) 
                return;
            
            _fontContent.Remove(fontKey);
        }

        
        #endregion
        
        #region AddAnimatedTexture
        /// <summary>
        /// Adds an animated texture to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <param name="texture"></param>
        public void AddAnimatedTexture(string textureKey, AnimatedTexture texture)
        {
            if (!_animatedTexturesContent.ContainsKey(textureKey))
                _animatedTexturesContent.Add(textureKey, texture);
        }
        
        #endregion
        
        #region GetAnimatedTexture
        /// <summary>
        /// Gives the animated texture that has already been loaded.
        /// </summary>
        /// <param name="textureKey"></param>
        /// <returns>Returns the correct animated texture</returns>
        public AnimatedTexture GetAnimatedTexture(string textureKey)
        {
            return _animatedTexturesContent.ContainsKey(textureKey) ? _animatedTexturesContent[textureKey] : throw new KeyNotFoundException($"The animated texture Key: {textureKey} was not found.");
        }
        
        #endregion
        
        #region GetAtlasTexture
        
        public AtlasTexture GetAtlasTexture(string atlasKey)
        {
            return _atlasTexturesContent.ContainsKey(atlasKey)
                ? _atlasTexturesContent[atlasKey]
                : _atlasTexturesContent["Tiles:Missing"];
        }
        
        #endregion
        
        #region AddAtlasTexture

        /// <summary>
        /// Adds an animated texture to the dictionary, provided it does not already exist.
        /// </summary>
        /// <param name="atlasKey"></param>
        /// <param name="texture"></param>
        public void AddAtlasTexture(string atlasKey, AtlasTexture texture)
        {
            if (!_atlasTexturesContent.ContainsKey(atlasKey))
                _atlasTexturesContent.Add(atlasKey, texture);
        }
        #endregion
        
        #region UnloadAnimatedTexture
        /// <summary>
        /// Unloads the animated texture.
        /// </summary>
        /// <param name="textureKey"></param>
        public void UnloadAnimatedTexture(string textureKey)
        {
            if (_animatedTexturesContent.ContainsKey(textureKey)) 
                return;
            
            _animatedTexturesContent[textureKey].Texture.Dispose();
            _animatedTexturesContent.Remove(textureKey);
        }

        
        #endregion
        
        #region ParseContent
        /// <summary>
        /// Parses content.json to figure out what content needs to be loaded.
        /// </summary>
        public void ParseContent(bool includeAudio = false )
        {
            string json;
            
            //Get file for Parsing
            using (StreamReader r = new StreamReader("content.json"))
            {
                json = r.ReadToEnd();
            }
            
            ContentJSONSchema content = JsonConvert.DeserializeObject<ContentJSONSchema>(json);
            
            //Parse texture
            ParseTextures(content.Textures);
            
            //Parse fonts
            ParseFonts(content.Fonts);
            
            //Parse animated textures
            ParseAnimatedTextures(content.AnimatedTextures);
            
            //Parse atlas textures
            ParseAtlasTextures(content.AtlasTextures);

            //Parse audio
            if(!includeAudio)
                ParseAudio(content.Audio); 
        }
        
        #endregion
        
        #region ParseTextures
        /// <summary>
        /// Parses the textures and loads them.
        /// </summary>
        /// <param name="textureConfig"></param>
        private void ParseTextures(Dictionary<string,string> textureConfig)
        {
            foreach (var texture in textureConfig)
            {
                Console.WriteLine("Loading Texture: " + texture.Key);
                
                if(_textureContent.ContainsKey(texture.Key))
                    throw new DuplicateNameException($"The texture key: {texture.Key} was found more then once while parsing textures");
                
                _textureContent.Add(texture.Key, _contentManager.Load<Texture2D>(texture.Value));
            }
        }
        
        #endregion
        
        #region ParseAnimatedTextures
        /// <summary>
        /// Parses and loads the animated textures.
        /// </summary>
        /// <param name="textureConfig"></param>
        private void ParseAnimatedTextures(Dictionary<string,AnimatedTexture> textureConfig)
        {
            foreach ((string key, AnimatedTexture value) in textureConfig)
            {
                Console.WriteLine("Loading Animated Texture: " + key);
                
                if(_animatedTexturesContent.ContainsKey(key))
                    throw new DuplicateNameException($"The animated texture key: {key} was found more then once while parsing animated textures");

                value.Texture = _contentManager.Load<Texture2D>(value.Location);
                
                //color
                if (value.RandomColor)
                {
                    Random random = new Random();

                    Color valueParsedColor = new Color
                    {
                        R = (byte) random.Next(0, 255),
                        G = (byte) random.Next(0, 255),
                        B = (byte) random.Next(0, 255),
                        A = 255
                    };
                    // = new Color(r,g,b,1f);
                    value.ParsedColor = valueParsedColor;
                }
                else if (value.ColorConfig == null)
                {
                    value.ParsedColor = Color.White;
                }
                else
                {
                    string[] colorArray = value.ColorConfig.Split(",");

                    if (colorArray.Length != 4)
                        throw new Exception($"Color parsing failed for Animation texture: {key}");
                
                    int r = int.Parse(colorArray[0]);
                    int g = int.Parse(colorArray[1]);
                    int b = int.Parse(colorArray[2]);
                    float alpha = float.Parse(colorArray[3]);
                
                    value.ParsedColor = new Color(r,g,b,alpha);
                }
                
                _animatedTexturesContent.Add(key, value);
            }
        }
        
        #endregion
        
        #region ParseAtlasTexture

        private void ParseAtlasTextures(Dictionary<string,AtlasTexture> atlasConfig) 
        {
            foreach ((string key, AtlasTexture value) in atlasConfig)
            {
                Console.WriteLine("Loading Atlas Texture: " + key);
                
                if(_atlasTexturesContent.ContainsKey(key))
                    throw new DuplicateNameException($"The animated texture key: {key} was found more then once while parsing animated textures");

                if(!_textureContent.ContainsKey(value.TextureKey))
                    throw new KeyNotFoundException($"The key: \"{value.TextureKey}\" was not a valid key. Try confirming that the key exist in the Textures section of the json");
                    
                value.Texture = _textureContent[value.TextureKey];//Get reference to the correct texture
                
                _atlasTexturesContent.Add(key, value);
            }
        }
        
        #endregion
        
        #region ParseFonts
        
        /// <summary>
        /// Parses and loads the fonts.
        /// </summary>
        /// <param name="fontConfig"></param>
        private void ParseFonts(Dictionary<string,string> fontConfig)
        {
            foreach (var font in fontConfig)
            {
                Console.WriteLine("Loading font: " + font.Key);
                
                if(_textureContent.ContainsKey(font.Key))
                    throw new DuplicateNameException($"The font key: {font.Key} was found more then once while parsing fonts");
                
                _fontContent.Add(font.Key, _contentManager.Load<SpriteFont>(font.Value));
            }
        }

        #endregion

        #region ParseAudio
        /// <summary>
        /// Parses and loads the audio.
        /// </summary>
        private void ParseAudio(Dictionary<string, AudioSound> audioConfig) 
        {
            foreach (var audio in audioConfig)
            {
                Console.WriteLine("Loading audio: " + audio.Key);
                if (audio.Value.SoundType == AudioType.Song)
                {
                    if (_audioSongContent.ContainsKey(audio.Key))
                        throw new DuplicateNameException($"The audio key: {audio.Key} was found more then once while parsing audio");
                    _audioSongContent.Add(audio.Key, _contentManager.Load<Song>(audio.Value.Location));
                }
                else
                {
                    if (_audioSoundEffectContent.ContainsKey(audio.Key))
                        throw new DuplicateNameException($"The audio key: {audio.Key} was found more then once while parsing audio");
                    _audioSoundEffectContent.Add(audio.Key, _contentManager.Load<SoundEffect>(audio.Value.Location));
                }


            }
        }

        #endregion
    }

    #region Content Schema Classes
    
    /// <summary>
    /// Schema for what the content will look like.
    /// </summary>
    [UsedImplicitly]
    public class ContentJSONSchema
    {
        [UsedImplicitly]
        public Dictionary<string,string> Fonts { get; set; }
        
        public Dictionary<string,AudioSound> Audio { get; set; }
        
        [UsedImplicitly]
        public Dictionary<string,string> Textures { get; set; }
        
        [UsedImplicitly]
        public Dictionary<string,AtlasTexture> AtlasTextures { get; set; }
        
        [UsedImplicitly]
        public Dictionary<string,AnimatedTexture> AnimatedTextures { get; set; }

    }
    
    #endregion
    
}