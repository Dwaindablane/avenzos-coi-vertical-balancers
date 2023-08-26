using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;

namespace Avenzos.VerticalBalancers;

internal class BalancersResearchData : IResearchNodesData {

	public void RegisterData(ProtoRegistrator registrator) {

        ResearchNodeProto nodeProto = registrator.ResearchNodeProtoBuilder
            .Start(Titles.Balancers.BalancersVertical, Ids.Research.BalancersVertical)
            .Description("Withdrawing resources from main bus become more pleasant than ever!")
            .SetCosts(new ResearchCostsTpl(4))
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalFlatI1)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalFlatI2)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalFlatI3)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalFlatI4)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalLooseI1)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalLooseI2)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalLooseI3)
            .AddLayoutEntityToUnlock(Ids.Balancers.BalancerVerticalLooseI4)
            .BuildAndAdd();

        nodeProto.GridPosition = new Vector2i(24, 4);
        nodeProto.AddParent(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Mafi.Base.Ids.Research.ConveyorBelts));

    }

}