using Mafi.Base;
using Mafi.Core.Research;
using ResNodeID = Mafi.Core.Research.ResearchNodeProto.ID;

namespace Avenzos.VerticalBalancers;

public partial class Ids
{


	public partial class Research 
    {

        [ResearchCosts(4)]
        public static readonly ResNodeID BalancersVertical = Mafi.Base.Ids.Research.CreateId("BalancersVertical");

	}

}