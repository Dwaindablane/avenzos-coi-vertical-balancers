using Mafi.Base;
using Mafi.Core.Research;
using ResNodeID = Mafi.Core.Research.ResearchNodeProto.ID;

namespace ExampleMod;

public partial class ExampleModIds {


	public partial class Research {

        [ResearchCosts(4)]
        public static readonly ResNodeID UnlockVerticalBalancers = Ids.Research.CreateId("UnlockVerticalBalancers");

	}

}