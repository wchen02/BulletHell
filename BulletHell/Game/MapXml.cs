using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BulletHell.Game.Library.xml;
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;

namespace BulletHell.Game
{
    public enum TagID
    {
        Map,
        Head,
        Body,
        Title,
        Meta,
        Obj,
        MoveableSpriteObj,
        PlaneObj,
        BulletObj,
        Details,
        BulletType,
        EnemyPlaneObj,
        Behaviors,
        HeroPlaneObj,
        Object,
        MoveableSpriteObject,
        PlaneObject,
        EnemyPlaneObject,
        HeroPlaneObject,
        Event,
        CreateObj,
        Path
    }

    /* This class define the syntax used for the maploader */
    public class MapXml : XmlStruct
    {
        public XmlTag Map { get; private set; }
        public XmlTag Head{ get; private set; }
        public XmlTag Body { get; private set; }
        public XmlTag Title { get; private set; }
        public XmlTag Meta { get; private set; }

        /* depeneent tags */
        public XmlTag Obj { get; private set; }
        public XmlTag MoveableSpriteObj { get; private set; }
        public XmlTag PlaneObj { get; private set; }
        public XmlTag BulletObj { get; private set; }
        public XmlTag Details { get; private set; }
        public XmlTag BulletType { get; private set; }
        public XmlTag EnemyPlaneObj { get; private set; }
        public XmlTag Behaviors { get; private set; }
        public XmlTag Path { get; private set; }
        public XmlTag HeroPlaneObj { get; private set; }

        /* inddependednt tags, think complete word for completely indpendent tag */
        public XmlTag Object { get; private set; }
        public XmlTag MoveableSpriteObject { get; private set; }
        public XmlTag PlaneObject { get; private set; }
        public XmlTag EnemyPlaneObject { get; private set; }
        public XmlTag HeroPlaneObject { get; private set; }
        public XmlTag Event { get; private set; }
        public XmlTag CreateObj { get; private set; }

        private Dictionary<string, XmlTag> xml;

        public MapXml(XmlTag root)
            : base(root) {}

        public MapXml() : this(null) {
            initialized();
        }

        private void initialized()
        {
            xml = new Dictionary<string, XmlTag>();
            Root = new XmlTag("map");
            Root.tagId = (int)TagID.Map;
                Head = new XmlTag("head");
                Head.tagId = (int)TagID.Head;
  
                    Title = new XmlTag("title");
                    Title.tagId = (int)TagID.Title;
                Head.Children.Add(Title);

                    Meta = new XmlTag("meta");
                    Meta.tagId = (int)TagID.Meta;
                    Meta.Attributes.Add("content","untitled");
                    Meta.EssentialAttributes.Add("content");
                Head.Children.Add(Meta);

            Root.Children.Add(Head);

                Body = new XmlTag("body");
                Body.tagId = (int)TagID.Body;
                Body.Attributes.Add("background", "");
                Body.Attributes.Add("bgmusic", "");
                Body.Attributes.Add("scrollspeed", "");

                    Obj = new XmlTag("obj");
                    Obj.tagId = (int)TagID.Obj;
                    Obj.Attributes.Add("id", "");
                    Obj.Attributes.Add("src", "");
                    Obj.EssentialAttributes.Add("id");
                    Obj.EssentialAttributes.Add("src");
                Body.Children.Add(Obj);

                    MoveableSpriteObj = new XmlTag("moveablespriteobj");
                    MoveableSpriteObj.tagId = (int)TagID.MoveableSpriteObj;
                    MoveableSpriteObj.Attributes.Add("id", "");
                    MoveableSpriteObj.Attributes.Add("obj", "");
                    MoveableSpriteObj.Attributes.Add("vx", "");
                    MoveableSpriteObj.Attributes.Add("vy", "");
                    MoveableSpriteObj.Attributes.Add("width", "default");
                    MoveableSpriteObj.Attributes.Add("height", "default");
                    MoveableSpriteObj.Attributes.Add("moverate", "15");
                    MoveableSpriteObj.EssentialAttributes.Add("id");
                    MoveableSpriteObj.EssentialAttributes.Add("obj");
                    MoveableSpriteObj.EssentialAttributes.Add("vx");
                    MoveableSpriteObj.EssentialAttributes.Add("vy");
                Body.Children.Add(MoveableSpriteObj);

                        BulletType = new XmlTag("bullettype");
                        BulletType.tagId = (int)TagID.BulletType;
                               
                    PlaneObj = new XmlTag("planeobj");
                    PlaneObj.tagId = (int)TagID.PlaneObj;
                    PlaneObj.Attributes.Add("id", "");
                    PlaneObj.Attributes.Add("moveablespriteobj", "");
                    PlaneObj.Attributes.Add("hp", "1");
                    PlaneObj.EssentialAttributes.Add("id");
                    PlaneObj.EssentialAttributes.Add("moveablespriteobj");
                    PlaneObj.Children.Add(BulletType);
                Body.Children.Add(PlaneObj);

                    Path = new XmlTag("path");
                    Path.tagId = (int)TagID.Path;
                    Path.Attributes.Add("id", "");
                    Path.Attributes.Add("path", "");
                    Path.Attributes.Add("xreverse", "false");
                    Path.Attributes.Add("yreverse", "false");
                    Path.EssentialAttributes.Add("id");
                Body.Children.Add(Path);

                    BulletObj = new XmlTag("bulletobj");
                    BulletObj.tagId = (int)TagID.BulletObj;
                    BulletObj.Attributes.Add("id", "");
                    BulletObj.Attributes.Add("moveablespriteobj", "");
                    BulletObj.Attributes.Add("killable", "false");
                    BulletObj.Attributes.Add("bounceable", "false");
                    BulletObj.EssentialAttributes.Add("id");
                    BulletObj.EssentialAttributes.Add("moveablespriteobj");
                        Details = new XmlTag("details");
                        Details.tagId = (int)TagID.Details;
                        Details.Attributes.Add("id", "");
                        Details.Attributes.Add("amount", "");
                        Details.Attributes.Add("separation", "");
                        Details.Attributes.Add("interval", "250");
                        Details.EssentialAttributes.Add("id");
                        Details.EssentialAttributes.Add("amount");
                        Details.EssentialAttributes.Add("separation");
                    BulletObj.Children.Add(Details);

                Body.Children.Add(BulletObj);
				
                    EnemyPlaneObj = new XmlTag("enemyplaneobj");
                    EnemyPlaneObj.tagId = (int)TagID.EnemyPlaneObj;
                    EnemyPlaneObj.Attributes.Add("id", "");
                    EnemyPlaneObj.Attributes.Add("planeobj", "");
                    EnemyPlaneObj.Attributes.Add("x", "");
                    EnemyPlaneObj.Attributes.Add("y", "");
                    EnemyPlaneObj.Attributes.Add("path", "");
                    EnemyPlaneObj.EssentialAttributes.Add("id");
                    EnemyPlaneObj.EssentialAttributes.Add("planeobj");
                    EnemyPlaneObj.EssentialAttributes.Add("x");
                    EnemyPlaneObj.EssentialAttributes.Add("y");
                        Behaviors = new XmlTag("behaviors");
                        Behaviors.tagId = (int)TagID.Behaviors;
                    EnemyPlaneObj.Children.Add(Behaviors);
                    
                Body.Children.Add(EnemyPlaneObj);

                    HeroPlaneObj = new XmlTag("heroplaneobj");
                    HeroPlaneObj.tagId = (int)TagID.HeroPlaneObj;
                    HeroPlaneObj.Attributes.Add("id", "");
                    HeroPlaneObj.Attributes.Add("planeobj", "");
                    HeroPlaneObj.Attributes.Add("x", "");
                    HeroPlaneObj.Attributes.Add("y", "");
                    HeroPlaneObj.EssentialAttributes.Add("id");
                    HeroPlaneObj.EssentialAttributes.Add("planeobj");
                    HeroPlaneObj.EssentialAttributes.Add("x");
                    HeroPlaneObj.EssentialAttributes.Add("y");
                Body.Children.Add(HeroPlaneObj);

                    Object = new XmlTag("object");
                    Object.tagId = (int)TagID.Object;
                    Object.Attributes.Add("id", "");
                    Object.Attributes.Add("src", "");
                    Object.Attributes.Add("x", "");
                    Object.Attributes.Add("y", "");
                    Object.Attributes.Add("width", "default");
                    Object.Attributes.Add("height", "default");
                    Object.EssentialAttributes.Add("src");
                    Object.EssentialAttributes.Add("x");
                    Object.EssentialAttributes.Add("y");
                Body.Children.Add(Object);

                    EnemyPlaneObject = new XmlTag("enemyplaneobject");
                    EnemyPlaneObject.tagId = (int)TagID.EnemyPlaneObject;
                    EnemyPlaneObject.Attributes.Add("src", "");
                    EnemyPlaneObject.Attributes.Add("x", "");
                    EnemyPlaneObject.Attributes.Add("y", "");
                    EnemyPlaneObject.Attributes.Add("vx", "");
                    EnemyPlaneObject.Attributes.Add("vy", "");
                    EnemyPlaneObject.Attributes.Add("width", "default");
                    EnemyPlaneObject.Attributes.Add("height", "default");
                    EnemyPlaneObject.Attributes.Add("moverate", "15");
                    EnemyPlaneObject.Attributes.Add("hp", "1");

                    EnemyPlaneObject.EssentialAttributes.Add("src");
                    EnemyPlaneObject.EssentialAttributes.Add("x");
                    EnemyPlaneObject.EssentialAttributes.Add("y");
                    EnemyPlaneObject.EssentialAttributes.Add("vx");
                    EnemyPlaneObject.EssentialAttributes.Add("vy");
                Body.Children.Add(EnemyPlaneObject);

                    HeroPlaneObject = new XmlTag("heroplaneobject");
                    HeroPlaneObject.tagId = (int)TagID.HeroPlaneObject;
                    HeroPlaneObject.Attributes.Add("src", "");
                    HeroPlaneObject.Attributes.Add("x", "");
                    HeroPlaneObject.Attributes.Add("y", "");
                    HeroPlaneObject.Attributes.Add("vx", "");
                    HeroPlaneObject.Attributes.Add("vy", "");
                    HeroPlaneObject.Attributes.Add("width", "default");
                    HeroPlaneObject.Attributes.Add("height", "default");
                    HeroPlaneObject.Attributes.Add("moverate", "15");
                    HeroPlaneObject.Attributes.Add("hp", "1");

                    HeroPlaneObject.EssentialAttributes.Add("src");
                    HeroPlaneObject.EssentialAttributes.Add("x");
                    HeroPlaneObject.EssentialAttributes.Add("y");
                    HeroPlaneObject.EssentialAttributes.Add("vx");
                    HeroPlaneObject.EssentialAttributes.Add("vy");
                    HeroPlaneObject.Children.Add(BulletType);
                Body.Children.Add(HeroPlaneObject);

                    Event = new XmlTag("event");
                    Event.tagId = (int)TagID.Event;

                        CreateObj = new XmlTag("createobj");
                        CreateObj.tagId = (int)TagID.CreateObj;
                        CreateObj.Attributes.Add("time", "0");
                        CreateObj.Attributes.Add("obj", "");
                        CreateObj.EssentialAttributes.Add("time");
                        CreateObj.EssentialAttributes.Add("obj");
                    Event.Children.Add(CreateObj);
                Body.Children.Add(Event);

            Root.Children.Add(Body);
            Map = Root;
            Map.tagId = Root.tagId;

            EssentialTags = new List<TagID>();
            EssentialTags.Add((TagID)Map.tagId);
            EssentialTags.Add((TagID)Head.tagId);
            EssentialTags.Add((TagID)Title.tagId);
            EssentialTags.Add((TagID)Body.tagId);

            xml.Add("map", Map);
            xml.Add("head", Head);
            xml.Add("title", Title);
            xml.Add("meta", Meta);
            xml.Add("body", Body);
            xml.Add("obj", Obj);
            xml.Add("moveablespriteobj", MoveableSpriteObj);
            xml.Add("moveablespriteobject", MoveableSpriteObject);
            xml.Add("bulletobj", BulletObj);
            xml.Add("bullettype", BulletType);
            xml.Add("details", Details);
            xml.Add("planeobj", PlaneObj);
            xml.Add("planeobject", PlaneObject);
            xml.Add("enemyplaneobj", EnemyPlaneObj);
            xml.Add("enemyplaneobject", EnemyPlaneObject);
            xml.Add("behaviors", Behaviors);
            xml.Add("object", Object);
            xml.Add("heroplaneobject", HeroPlaneObject);
            xml.Add("heroplaneobj", HeroPlaneObj);
            xml.Add("event", Event);
            xml.Add("createobj", CreateObj);
            xml.Add("path", Path);
        }

        /* Returns a tag given the tag name. This is super ugly. */
        public override XmlTag getTagFromString(String element)
        {
            try
            {
                element = element.ToLower();
                if(!xml.ContainsKey(element))
                    throw new Exception("Error 1: Tag not found, '" + element + "'.");

                return new XmlTag(xml[element]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
