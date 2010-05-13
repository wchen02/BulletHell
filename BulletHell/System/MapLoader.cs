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
using BulletHell.Game.Library;
using BulletHell.Game.Library.Behavior;

namespace BulletHell.System
{
    /* Should build an abstract layer for future reusability */
    public class MapLoader
    {
        private MapXml mapXml;
        private XmlValidator xmlValidator;
        private Stack<XmlTag> stack;
        private XmlTextReader reader;

        private HeroPlaneObj hero;
        private SoundEffect BGM;
        private Texture2D background;
        private Dictionary<string, Obj> objs;
        private Dictionary<string, Obj> objects;
        private Dictionary<string, MoveableSpriteObj> moveableSpriteObjs;
        private Dictionary<string, PlaneObj> planeObjs;
        private Dictionary<string, BulletObj> bulletObjs;
        private Dictionary<string, EnemyPlaneObj> enemyPlaneObjs;
        private Dictionary<string, Path> paths;
        private List<Event> events;

        private String title, objective;
        private float scrollSpeed;

        public MapLoader(MapXml mapXml)
        {
            this.mapXml = mapXml; // The XmlStruct to validate the map with
            this.xmlValidator = new XmlValidator(new MapXml());
        }

        public void reset()
        {
            stack = new Stack<XmlTag>();
            hero = null;
            BGM = null;
            background = null;
            objs = new Dictionary<string, Obj>();
            objects = new Dictionary<string, Obj>();
            moveableSpriteObjs = new Dictionary<string, MoveableSpriteObj>();
            planeObjs = new Dictionary<string, PlaneObj>();
            bulletObjs = new Dictionary<string, BulletObj>();
            enemyPlaneObjs = new Dictionary<string, EnemyPlaneObj>();
            events = new List<Event>();
            paths = new Dictionary<string,Path>();
            title = null;
            objective = null;
            scrollSpeed = 0.0f;
        }

        public Map loadMap(String filename)
        {
            reset();
            reader = new XmlTextReader(filename);
            if (!xmlValidator.validateXml(filename))
            {
                Console.WriteLine("Fail to validate the given map, file might be corrupted or incorrectly created. Please check the syntax.");
                //exits the program if desired.
            }

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // Open tag

                        /* Pushes a new copy of the tag into the stack */
                        stack.Push(mapXml.getTagFromString(reader.Name.ToLower()));

                        /* sets the attrib values */
                        while (reader.MoveToNextAttribute())
                            stack.Peek().Attributes[reader.Name.ToLower()] = reader.Value.ToLower();

                        /* Converts the tag into Map related value */
                        convertTagToValue(stack.Peek());
                        break;

                    case XmlNodeType.Text: // Tag value
                        setElementValue(stack.Peek(), reader.Value);
                        break;

                    case XmlNodeType.EndElement: // Close tag
                        stack.Pop();
                        break;
                }
            }
            return new Map(title, objective, background, BGM, scrollSpeed, enemyPlaneObjs, objects, hero, events);
        }

        /* Should be improved to use polymorphism, such that tags[element.tagId].setValue(value) is possible */
        private void setElementValue(XmlTag element, string value)
        {
            switch ((TagID)element.tagId)
            {
                case TagID.Body:
                    break;
                case TagID.Head:
                    break;
                case TagID.Title:
                    title = value;
                    break;
                case TagID.Meta:
                    break;
                case TagID.Obj:
                    break;
                case TagID.MoveableSpriteObj:
                    break;
                case TagID.PlaneObj:
                    break;
                case TagID.BulletObj:
                    break;
                case TagID.Details:
                    break;
                case TagID.BulletType:
                    setBulletType(value);
                    break;
                case TagID.EnemyPlaneObj:
                    break;
                case TagID.Behaviors:
                    setBehaviors(value);
                    break;
                case TagID.HeroPlaneObj:
                    break;
                case TagID.Object:
                    break;
                case TagID.EnemyPlaneObject:
                    break;
                case TagID.HeroPlaneObject:
                    break;
                case TagID.Map:
                    break;
                case TagID.Event:
                    break;
                case TagID.CreateObj:
                    break;
                case TagID.Path:
                    setPath(value);
                    break;
            }
        }

        /* setBulletType's subproblem */
        private void setBulletTypeSub(PlaneObj plane, string value)
        {
            foreach (string input in getStringsFromValue(value, ','))
                plane.addBulletType(bulletObjs[input]);
        }

        private void setBulletType(string value)
        {
            XmlTag bullet = stack.Pop(); // Has to pop the current tag, in order to see it's parent

            /* Shared tag between PlaneObj and HeroPlaneObject */
            if ((TagID)stack.Peek().tagId == TagID.PlaneObj)
                setBulletTypeSub(planeObjs[stack.Peek().Attributes["id"]], value);
            else if ((TagID)stack.Peek().tagId == TagID.HeroPlaneObject)
                setBulletTypeSub(hero, value);
            else
                throw new Exception("BulletType does not have a parent called \"" + stack.Peek().Tag + "\".");

            stack.Push(bullet); // Puts it back in afterward.
        }

        /* This method eliminates all the whitespaces and adds the strings delimited by the delimiter to the return list.
         * Ex: value = "   \t\n   \tThis     \n, is \t,an \t\n\t,  example."
         * will return, inputs = { "This", "is", "an", "example" } */
        private List<string> getStringsFromValue(string value, char delimiter)
        {
            int index = 0, lastIndex = 0;
            string input = value.Trim(); // trim removes whitespaces on both end of the string, does not remove the whitespaces in between
            List<string> inputs = new List<string>();

            index = input.IndexOf(delimiter, 0); // finds the index of the first delimiter

            while (index != -1) // while there are more delimiters
            {
                inputs.Add(input.Substring(lastIndex, index - lastIndex).Trim());

                lastIndex = index + 1; // skips the delimiter char
                index = input.IndexOf(delimiter, lastIndex);
            }

            inputs.Add(input.Substring(lastIndex).Trim()); // the last item is not delimited

            return inputs;
        }

        /* Needs an improvement, something like parentRef.behaviors.Add(new Behavior(behaviors[value], parentRef) sould eliminate this routine of if/elseif checkings; */
        private Behavior getBehaviorFromString(string value, ref EnemyPlaneObj parentRef)
        {
            if (value.ToLower() == "shootstraight")
                return new ShootingStraightBehavior(parentRef);
            else if (value.ToLower() == "shoottarget")
                return new ShootingTargetBehavior(parentRef);
            else if (value.ToLower() == "shootcircle")
                return new ShootingCircleBehavior(parentRef);
            else if (value.ToLower() == "shootstraightangle")
                return new ShootingStraightAngleBehavior(parentRef);
            else if (value.ToLower() == "evade")
                return new EvadingBehavior(parentRef);
            else if (value.ToLower() == "chase")
                return new ChasingBehavior(parentRef);
            else if (value.ToLower() == "selfdestruct")
                return new SelfDestructingBehavior(parentRef);
            else if (value.ToLower() == "shoot")
                return new ShootingBehavior(parentRef);
            else if (value.ToLower() == "behaviorcontroller")
                return new BehaviorController(parentRef);
            else if (value.ToLower() == "linear")
                return new LinearBehavior(parentRef);

            return null;
        }

        private void setBehaviors(string value)
        {
            XmlTag behavior = stack.Pop(); // Has to pop the current tag, in order to see it's parent
            EnemyPlaneObj enemy = enemyPlaneObjs[stack.Peek().Attributes["id"]];
            foreach (string input in getStringsFromValue(value, ','))
                enemy.Behaviors.Add(getBehaviorFromString(input, ref enemy));

            stack.Push(behavior); // Puts it back afterward
        }

        private void setPath(string value)
        {
            int whitespace = 0;
            char[] searchFor = new char[] { ' ', '\n', '\t', '\r' };
            List<int> xAndYs = new List<int>();
            Path path = paths[stack.Peek().Attributes["id"]];

            foreach (string input in getStringsFromValue(value, ','))
            {
                whitespace = input.IndexOfAny(searchFor);
                if (whitespace == -1)
                    xAndYs.Add(Int32.Parse(input));
                else
                    foreach (string xORy in getStringsFromValue(input, input[whitespace]))
                        xAndYs.Add(Int32.Parse(xORy));
            }

            if (xAndYs.Count % 2 == 0)
                for (int i = 0; i < xAndYs.Count; i += 2)
                        path.Enqueue(new Waypoint(xAndYs[i], xAndYs[i + 1]));
            
            paths[stack.Peek().Attributes["id"]] = path;
        }

        /* Should be improved to use polymorphism, such that tags[element.tagId].convertTag() is possible. */
        private void convertTagToValue(XmlTag element)
        {
            switch ((TagID)element.tagId)
            {
                case TagID.Body:
                    convertBody(element);
                    break;
                case TagID.Head:
                    break;
                case TagID.Title:
                    break;
                case TagID.Meta:
                    convertMeta(element);
                    break;
                case TagID.Obj:
                    convertObj(element);
                    break;
                case TagID.MoveableSpriteObj:
                    convertMoveableSpriteObj(element);
                    break;
                case TagID.PlaneObj:
                    convertPlaneObj(element);
                    break;
                case TagID.BulletObj:
                    break;
                case TagID.Details:
                    convertDetails(element);
                    break;
                case TagID.BulletType:
                    break;
                case TagID.EnemyPlaneObj:
                    convertEnemyPlaneObj(element);
                    break;
                case TagID.Behaviors:
                    convertBehaviors(element);
                    break;
                case TagID.HeroPlaneObj:
                    convertHeroPlaneObj(element);
                    break;
                case TagID.Object:
                    convertObject(element);
                    break;
                case TagID.EnemyPlaneObject:
                    convertEnemyPlaneObject(element);
                    break;
                case TagID.HeroPlaneObject:
                    convertHeroPlaneObject(element);
                    break;
                case TagID.Map:
                    break;
                case TagID.Event:
                    break;
                case TagID.CreateObj:
                    convertCreateObj(element);
                    break;
                case TagID.Path:
                    convertPath(element);
                    break;
            }
        }

        private void convertPath(XmlTag element) 
        {
            string key = "", pathId = "";
            bool xReverse = false,
                yReverse = false;

            Path value = null;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "xreverse")
                    xReverse = bool.Parse(attrib.Value);
                else if (attrib.Key == "yreverse")
                    yReverse = bool.Parse(attrib.Value);
                else if (attrib.Key == "path")
                    pathId = attrib.Value;
            }

            if (pathId != "")
            {
                if (paths.ContainsKey(pathId)){
                    value = paths[pathId].createMirror(xReverse, yReverse);
                }
                else
                    throw new Exception("Invalid path id: " + pathId);
            }

            if (value == null)
                value = new Path();

            paths.Add(key, value);
        }

        private void convertCreateObj(XmlTag element)
        {
            int time = 0;
            string obj = "";

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "time")
                    time = Int32.Parse(attrib.Value);
                else if (attrib.Key == "obj")
                    obj = attrib.Value;
            }

            events.Add(new Event(time, obj));
        }

        private void convertBody(XmlTag element)
        {
            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "background")
                    background = (attrib.Value != "") ?
                        (Texture2D)Texture.FromFile(_GLOBAL.Graphics.GraphicsDevice, attrib.Value) : // This allows for dynamic loading of Texture
                        _GLOBAL.ContentManager.Load<Texture2D>(@"sprites/background"); // Default value

                else if (attrib.Key == "bgmusic")
                    BGM = (attrib.Value != "") ?
                        _GLOBAL.ContentManager.Load<SoundEffect>(attrib.Value) : // Needs to find a way to load dynamic soundeffects
                        _GLOBAL.ContentManager.Load<SoundEffect>(@"audio/effect/onCursorMoveEffect"); // Default value

                else if (attrib.Key == "scrollspeed")
                    scrollSpeed = (attrib.Value != "") ? float.Parse(attrib.Value) : 0.65f; // default value
            }
        }

        private void convertMeta(XmlTag element)
        {
            foreach (KeyValuePair<string, string> attrib in element.Attributes)
                if (attrib.Key == "content")
                    objective = attrib.Value;
        }

        private void convertObj(XmlTag element)
        {
            string key = "";
            Texture2D baseObj = null;
            Obj value;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "src")
                    baseObj = (Texture2D)Texture.FromFile(_GLOBAL.Graphics.GraphicsDevice, attrib.Value); // dynamic loading
            }

            value = new Obj(baseObj, 0, 0);
            value.ID = key;
            objs.Add(key, value);
        }

        private void convertMoveableSpriteObj(XmlTag element)
        {
            string key = "";
            Obj baseObj = null;
            MoveableSpriteObj value;
            int vx, vy, width, height, moverate;
            vx = vy = width = height = moverate = 0;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "obj")
                    baseObj = objs[attrib.Value];
                else if (attrib.Key == "vx")
                    vx = Int32.Parse(attrib.Value);
                else if (attrib.Key == "vy")
                    vy = Int32.Parse(attrib.Value);
                else if (attrib.Key == "width")
                    width = (attrib.Value == "default") ? baseObj.srcRect.Width : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "height")
                    height = (attrib.Value == "default") ? baseObj.srcRect.Height : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "moverate")
                    moverate = Int32.Parse(attrib.Value);
            }

            value = new MoveableSpriteObj(baseObj.sprite, 0, 0, vx, vy, width, height, moverate);
            value.ID = key;
            moveableSpriteObjs.Add(key, value);
        }

        private void convertPlaneObj(XmlTag element)
        {
            string key = "";
            MoveableSpriteObj baseObj = null;
            PlaneObj value = null;
            int hp = 0;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "moveablespriteobj")
                    baseObj = moveableSpriteObjs[attrib.Value];
                else if (attrib.Key == "hp")
                    hp = Int32.Parse(attrib.Value);
            }

            value = new PlaneObj(baseObj.sprite, 0, 0, (int)baseObj.velocity.X, (int)baseObj.velocity.Y, baseObj.srcRect.Width, baseObj.srcRect.Height, baseObj.MoveRate, hp);
            value.ID = key;
            planeObjs.Add(key, value);
        }

        private void convertDetails(XmlTag element)
        {
            XmlTag details = stack.Pop();
            string key = stack.Peek().Attributes["id"];
            MoveableSpriteObj baseObj = moveableSpriteObjs[stack.Peek().Attributes["moveablespriteobj"]];

            bool killable = bool.Parse(stack.Peek().Attributes["killable"]),
                bounceable = bool.Parse(stack.Peek().Attributes["bounceable"]);

            BulletObj value = new BulletObj(baseObj.sprite, 0, 0, (int)baseObj.velocity.X, (int)baseObj.velocity.Y, null, killable, bounceable, 0.0f, 1.0f);

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key += attrib.Value;
                else if (attrib.Key == "amount")
                    value.BulletAmount = Int32.Parse(attrib.Value);
                else if (attrib.Key == "separation")
                    value.BulletDensity = (Int32.Parse(attrib.Value) == 0) ? 0.5f : Int32.Parse(attrib.Value);
                else if (attrib.Key == "interval")
                    value.BulletInterval = Int32.Parse(attrib.Value);
            }

            value.ID = key;
            bulletObjs.Add(key, value);
            stack.Push(details);
        }

        private void convertEnemyPlaneObj(XmlTag element)
        {
            string key = "";
            PlaneObj baseObj = null;
            EnemyPlaneObj value = null;
            int x, y;
            x = y = 0;
            Path enemyPath = null;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "planeobj")
                    baseObj = planeObjs[attrib.Value];
                else if (attrib.Key == "x")
                    x = Int32.Parse(attrib.Value);
                else if (attrib.Key == "y")
                    y = Int32.Parse(attrib.Value);
                else if (attrib.Key == "path")
                    if(paths.ContainsKey(attrib.Value))
                        enemyPath = paths[attrib.Value].createMirror(false, false);
            }

            value = new EnemyPlaneObj(baseObj.sprite, x, y, (int)baseObj.velocity.X, (int)baseObj.velocity.Y, baseObj.srcRect.Width, baseObj.srcRect.Height, baseObj.MoveRate, baseObj.hp, enemyPath);

            foreach (BulletObj bullet in baseObj.BulletType)
                value.addBulletType(bullet);

            value.ID = key;
            enemyPlaneObjs.Add(key, value);
        }

        private void convertBehaviors(XmlTag element)
        {
            XmlTag tag = stack.Pop();
            EnemyPlaneObj enemy = enemyPlaneObjs[stack.Peek().Attributes["id"]];
            string behavior = "behaviorcontroller";

            enemy.currentBehavior = getBehaviorFromString(behavior, ref enemy);
            stack.Push(tag);
        }

        // Not used
        private void convertHeroPlaneObj(XmlTag element)
        {
            string key = "";
            PlaneObj baseObj = null;
            int x, y;
            x = y = 0;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "planeobj")
                    baseObj = planeObjs[attrib.Value];
                else if (attrib.Key == "x")
                    x = Int32.Parse(attrib.Value);
                else if (attrib.Key == "y")
                    y = Int32.Parse(attrib.Value);
            }

            hero = new HeroPlaneObj(baseObj.sprite, x, y, (int)baseObj.velocity.X, (int)baseObj.velocity.Y, baseObj.srcRect.Width, baseObj.srcRect.Height, baseObj.MoveRate, baseObj.hp);
        }

        private void convertObject(XmlTag element)
        {
            string key = "";
            Texture2D img = null;
            int x, y, width, height;
            x = y = width = height = 0;
            Obj value = null;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "id")
                    key = attrib.Value;
                else if (attrib.Key == "src")
                    img = (Texture2D)Texture.FromFile(_GLOBAL.Graphics.GraphicsDevice, attrib.Value); // dynamic loading
                else if (attrib.Key == "x")
                    x = Int32.Parse(attrib.Value);
                else if (attrib.Key == "y")
                    y = Int32.Parse(attrib.Value);
                else if (attrib.Key == "width")
                    width = (attrib.Value == "default") ? img.Width : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "height")
                    height = (attrib.Value == "default") ? img.Height : Int32.Parse(attrib.Value); // default value
            }

            value = new Obj(img, x, y, img.Width, img.Height, width * height / (img.Width * img.Height));

            if (key != "")  // Key is not required for Object. If not specified, use the one generated by the Obj class
                value.ID = key;
            objects.Add(value.ID, value);
        }

        private void convertEnemyPlaneObject(XmlTag element)
        {
            Texture2D img = null;
            int x, y, vx, vy, moverate, hp, width, height;
            x = y = vx = vy = moverate = hp = width = height = 0;
            EnemyPlaneObj value = null;
            Path enemyPath = null;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "src")
                    img = (Texture2D)Texture.FromFile(_GLOBAL.Graphics.GraphicsDevice, attrib.Value);// dynamic loading
                else if (attrib.Key == "x")
                    x = Int32.Parse(attrib.Value);
                else if (attrib.Key == "y")
                    y = Int32.Parse(attrib.Value);
                else if (attrib.Key == "vx")
                    vx = Int32.Parse(attrib.Value);
                else if (attrib.Key == "vy")
                    vy = Int32.Parse(attrib.Value);
                else if (attrib.Key == "width")
                    width = (attrib.Value == "default") ? img.Width : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "height")
                    height = (attrib.Value == "default") ? img.Height : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "moverate")
                    moverate = Int32.Parse(attrib.Value);
                else if (attrib.Key == "hp")
                    hp = Int32.Parse(attrib.Value);
                else if (attrib.Key == "path")
                    enemyPath = paths[attrib.Value].createMirror(false, false);
            }

            value = new EnemyPlaneObj(img, x, y, vx, vy, width, height, moverate, hp, enemyPath);

            enemyPlaneObjs.Add(value.ID, value);
        }

        private void convertHeroPlaneObject(XmlTag element)
        {
            Texture2D img = null;
            int x, y, vx, vy, moverate, hp, width, height;
            x = y = vx = vy = moverate = hp = width = height = 0;

            foreach (KeyValuePair<string, string> attrib in element.Attributes)
            {
                if (attrib.Key == "src")
                    img = (Texture2D)Texture.FromFile(_GLOBAL.Graphics.GraphicsDevice, attrib.Value); // dynamic loading
                else if (attrib.Key == "x")
                    x = Int32.Parse(attrib.Value);
                else if (attrib.Key == "y")
                    y = Int32.Parse(attrib.Value);
                else if (attrib.Key == "vx")
                    vx = Int32.Parse(attrib.Value);
                else if (attrib.Key == "vy")
                    vy = Int32.Parse(attrib.Value);
                else if (attrib.Key == "width")
                    width = (attrib.Value == "default") ? img.Width : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "height")
                    height = (attrib.Value == "default") ? img.Height : Int32.Parse(attrib.Value); // default value
                else if (attrib.Key == "moverate")
                    moverate = Int32.Parse(attrib.Value);
                else if (attrib.Key == "hp")
                    hp = Int32.Parse(attrib.Value);
            }

            hero = new HeroPlaneObj(img, x, y, vx, vy, width, height, moverate, hp);
        }
    }
}