using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ExampleMod;

internal class ExampleMachineData : IModData
{


    public static StaticEntityProto.ID GetZipperIdFor(int height, IoPortShapeProto.ID shapeId)
    {
        return new StaticEntityProto.ID($"VerticalZipper_{height}_{shapeId.Value}");
    }

    //    "   D?+   ", 
    //"E?+[2]+?B", 
    //"F?+[2]+?A", 
    //"   C?+   "));
    //setPortChar(portShape.LayoutChar,
    //    "   D?+C?+   ",
    //    "E?+[1][1]+?B",
    //    "F?+[1][1]+?A",
    //    "   G?+H?+   ")

    //"   [3][4][3][3][3][3]   ", 
    //"~3 >3A[4][3][3][3]X3> ~ ", 
    //"   [3][4][3][3][3][3]   ", 
    //"   [2][3][2][2]         "


    private static EntityLayout BuildElevatorLayout(ProtosDb db, IoPortShapeProto portShape, int height)
    {
        if (portShape == null)
        {
            throw new InvalidOperationException("Invalid Port Shape char");
        }

        var layoutParams = new EntityLayoutParams(useNewLayoutSyntax: true);
        var initLayout = new EntityLayoutParser(db).ParseLayoutOrThrow(layoutParams, $"[{height}]");

        List<IoPortTemplate> ports = new List<IoPortTemplate>();

        ports.Add(new IoPortTemplate(new PortSpec('A', IoPortType.Any, portShape, true), new RelTile3i(0, 0, 0), Direction90.PlusX));
        ports.Add(new IoPortTemplate(new PortSpec('B', IoPortType.Any, portShape, true), new RelTile3i(0, 0, 0), Direction90.MinusX));

        if(height > 1)
        {
            ports.Add(new IoPortTemplate(new PortSpec('C', IoPortType.Any, portShape, true), new RelTile3i(0, 0, height - 1), Direction90.PlusY));
            ports.Add(new IoPortTemplate(new PortSpec('D', IoPortType.Any, portShape, true), new RelTile3i(0, 0, height - 1), Direction90.MinusY));
        }

        return new EntityLayout("", initLayout.LayoutTiles, initLayout.TerrainVertices, ports.ToImmutableArray(), layoutParams, initLayout.CollapseVerticesThreshold);
    }

    private void AddZippers(ProtoRegistrator registrator, string name, IoPortShapeProto portShape, EntityCostsTpl costs, int height, string prefabPath)
    {
        
        StaticEntityProto.ID zipperIdFor = GetZipperIdFor(height, portShape.Id);
        Log.Info(zipperIdFor.Value);
        ProtosDb prototypesDb = registrator.PrototypesDb;
        Proto.Str strings = Proto.CreateStr(zipperIdFor, name, "Allows distributing and prioritizing products using any of its two input and output ports.", "small machine that allows splitting and merging of transports");

        Electricity requiredPower = (registrator.DisableAllProtoCosts ? Electricity.Zero : 1.Kw());
        EntityLayout layout = BuildElevatorLayout(registrator.PrototypesDb, portShape, height);

        ImmutableArray<ToolbarCategoryProto> categories = registrator.GetCategoriesProtos(Ids.ToolbarCategories.Transports);
        prototypesDb.Add(new ZipperProto(zipperIdFor, strings, layout, costs.MapToEntityCosts(registrator), requiredPower, new LayoutEntityProto.Gfx(prefabPath, default(RelTile3f), default(Option<string>), ColorRgba.White, hideBlockedPortsIcon: true, null, categories, useInstancedRendering: true, 2)));
    }

    public void RegisterData(ProtoRegistrator registrator)
    {

        //MINI_ZIP_TITLE = new Proto.Str(Loc.Str("MiniZip_all", "Connector", "small box that allows splitting and merging of transports"));
        IoPortShapeProto shapePipe = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.Pipe);
        //AddZippers(registrator, "Pipe Mini Balancer", shapePipe, Costs.Transports.FluidZipper, 1, "Assets/Base/MiniZippers/ConnectorFluid.prefab");
        AddZippers(registrator, "Pipe Vertical Balancer", shapePipe, Costs.Transports.FluidZipper, 2, "Assets/ExampleMod/PipeVerticalBalancer2h.prefab");
        //AddZippers(registrator, "Pipe Vertical Balancer", shapePipe, Costs.Transports.FluidZipper, 3, "Assets/Base/MiniZippers/ConnectorFluid.prefab");
        //AddZippers(registrator, "Pipe Vertical Balancer", shapePipe, Costs.Transports.FluidZipper, 4, "Assets/Base/MiniZippers/ConnectorFluid.prefab");

        
        IoPortShapeProto shapeFlat = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.FlatConveyor);
        //AddZippers(registrator, "Flat Mini Balancer", shapeFlat, Costs.Transports.FlatZipper, 1, "Assets/Base/MiniZippers/ConnectorFlat.prefab");
        AddZippers(registrator, "Flat Vertical Balancer", shapeFlat, Costs.Transports.FlatZipper, 2, "Assets/ExampleMod/FlatVerticalBalancer2h.prefab");
        //AddZippers(registrator, "Flat Vertical Balancer", shapeFlat, Costs.Transports.FlatZipper, 3, "Assets/Base/MiniZippers/ConnectorFlat.prefab");
        //AddZippers(registrator, "Flat Vertical Balancer", shapeFlat, Costs.Transports.FlatZipper, 4, "Assets/Base/MiniZippers/ConnectorFlat.prefab");

        IoPortShapeProto shapeLoose = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.LooseMaterialConveyor);
        //AddZippers(registrator, "Loose Mini Balancer", shapeLoose, Costs.Transports.LooseZipper, 1, "Assets/Base/MiniZippers/ConnectorUShape.prefab");
        AddZippers(registrator, "Loose Vertical Balancer", shapeLoose, Costs.Transports.LooseZipper, 2, "Assets/ExampleMod/LooseVerticalBalancer2h.prefab");
        //AddZippers(registrator, "Loose Vertical Balancer", shapeLoose, Costs.Transports.LooseZipper, 3, "Assets/Base/MiniZippers/ConnectorUShape.prefab");
        AddZippers(registrator, "Loose Vertical Balancer h4", shapeLoose, Costs.Transports.LooseZipper, 4, "TODO");

        registrator.MachineProtoBuilder
            .Start("Example furnace", ExampleModIds.Machines.ExampleFurnace)
            .Description("Testing furnace")
            .SetCost(Costs.Build.CP(80).Workers(10))
            // For examples of layouts see `Mafi.Base.BaseMod` and `EntityLayoutParser`.
            .SetLayout(new EntityLayoutParams(useNewLayoutSyntax: true),
                "   [1]>~Y",
                "   [1]   ",
                "A~>[1]>'V",
                "B~>[1]>'W",
                "   [1]   ",
                "   [1]>@E")
            .SetCategories(Ids.ToolbarCategories.MachinesMetallurgy)
            .SetPrefabPath("Assets/ExampleMod/BlastFurnace.prefab")
            .SetAnimationParams(
                animParams: AnimationParams.RepeatTimes(Duration.FromKeyframes(360),
                times: 2,
                changeSpeedToFit: true))
            .BuildAndAdd();


       

        //Example of a new furnace recipe.
       registrator.RecipeProtoBuilder
           .Start(name: "Example smelting",
               recipeId: ExampleModIds.Recipes.ExampleSmelting,
               machineId: ExampleModIds.Machines.ExampleFurnace)
           .AddInput(8, ExampleModIds.Products.ExampleLooseProduct)
           .AddInput(2, Ids.Products.Coal)
           .SetDuration(20.Seconds())
           .AddOutput(8, ExampleModIds.Products.ExampleMoltenProduct)
           .AddOutput(24, Ids.Products.Exhaust, outputAtStart: true)
           .BuildAndAdd();

    }
}