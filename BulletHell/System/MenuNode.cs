using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace BulletHell.System
{
    #region Class Vs. Struct
    /*
     * Here are just some of the differences between a struct and a class:
    *     Classes are reference types and structs are value types. Since classes are reference type, 
    *         a class variable can be assigned null, but we cannot assign null to a struct variable, since structs are value type.
    *     You will always be dealing with reference to an object ( instance ) of a class. 
     *         But you will not be dealing with references to an instance of a struct, you will be dealing directly with struct
    *     Classes must be instantiated using the new operator, but structs can be instantiated without using the new operator.
    *     Classes support inheritance.But there is no inheritance for structs, (structs don't support inheritance polymorphism )
    *     It is not mandatory to initialize all Fields inside the constructor of a class. But all the Fields of a struct must be 
     *         fully initialized inside the constructor.
    *     Structs object is allocated in the stack, and the class object is allocated in the heap.
     */
    #endregion

    public class MenuNode
    {
        public string Title { get; private set; }
        public GameState GameState { get; set; }
        public bool Active { get; set; }

        public MenuNode(string title, bool active){
            Title = title;
            Active = active;
        }
    }
}