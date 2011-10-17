using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.assets
{

    // We use 1 for the first value because C# defaults uninitialized 
    // enums to 0. This lets us catch any uninitizlized enums (this
    // caused an annoying bug!)

    enum RosterName
    {
        test = 1
    }

    enum ArsenalName
    {
        test = 1
    }

    enum ModelName
    {
        testcube = 1,
        XNAface,
        playerScene
        
    }
    enum EffectName
    {
        particle = 1,
        light0,
        terrain
    }
    enum FontName
    {
        font0 = 1
    }
    enum TextureName
    {
        sprite = 1,
        cursor,
        dot,
        shot,
        collectables,
        bg,
        wang8,
        fractal,
        face
    }

    enum SpriteName
    {
        life = 1,
        bomb,
        cursor
    }

    enum ScriptName
    {
        playerspawner0 = 1,
        enemyspawner0,
        test,
        playerspawn,
        item,
        playershot0,
        death0
    }

    enum GibsName
    {
        gibtest
    }

    //TODO deathscripts
    class AssetNames
    {

    }
}
