using System;
using System.Collections.Generic;
using System.Text;
using LuaInterface;

namespace SnailsPace.Core
{
    class GameLua : Lua
    {
        public GameLua()
            : base()
        {
            #region Lua initialization of C# classes
            String initCode = @" 

using = luanet.load_assembly;
import = luanet.import_type;

using('Microsoft.Xna.Framework');

Rectangle = import('Microsoft.Xna.Framework.Rectangle');
Vector2 = import('Microsoft.Xna.Framework.Vector2');
Vector3 = import('Microsoft.Xna.Framework.Vector3');

using('SnailsPace');

SnailsPace = import('SnailsPace.SnailsPace')
SnailsPace = SnailsPace.getInstance()

Trigger = import('SnailsPace.Objects.Trigger');

Image = import('SnailsPace.Objects.Image');
Sprite = import('SnailsPace.Objects.Sprite');
Text = import('SnailsPace.Objects.Text');

GameObject = import('SnailsPace.Objects.GameObject');
GameObjectBounds = import('SnailsPace.Objects.GameObjectBounds');
Bullet = import('SnailsPace.Objects.Bullet');
Character = import('SnailsPace.Objects.Character');
Helix = import('SnailsPace.Objects.Helix');

Map = import('SnailsPace.Objects.Map');
            
            ";
            this.DoString(initCode);
            #endregion
        }

        public void Call(System.Object obj, String function, params object[] args) 
        {
            this["this"] = obj;
            String call = "this:" + function + "(";

            for (int i = 0; i < args.Length; i++)
            {
                String varname = "arg" + i;
                this[varname] = args[i];

                call += (i == 0 ? "" : ",") + varname;
            }

            call += ")";
            DoString(call);
        }
    }
}
