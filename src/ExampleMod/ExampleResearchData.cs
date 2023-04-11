using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;

namespace ExampleMod;

internal class ExampleResearchData : IResearchNodesData {

	public void RegisterData(ProtoRegistrator registrator) {

		//ResearchNodeProto nodeProto = registrator.ResearchNodeProtoBuilder
		//	.Start("Unlock Vertical Balancers", ExampleModIds.Research.UnlockVerticalBalancers)
		//	.Description("Withdrawing resources from main bus become more pleasant than ever!")
		//	.AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalFlatI2)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalFlatI3)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalFlatI4)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalLooseI2)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalLooseI3)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalLooseI4)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalPipeI2)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalPipeI3)
  //          .AddLayoutEntityToUnlock(ExampleModIds.Balancers.BalancerVerticalPipeI4)
  //          .BuildAndAdd();

		//nodeProto.GridPosition = new Vector2i(4, 31);
  //      nodeProto.AddParent(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ConveyorBelts));

	}

}