using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class GamerPrompt_Handler : AMActorRpcHandler<Gamer, Actor_GamerPrompt_Req, Actor_GamerPrompt_Back>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GamerPrompt_Req request, Actor_GamerPrompt_Back response, Action reply)
        {
            try
            {
                Room room = Game.Scene.GetComponent<LandMatchComponent>().GetGamingRoom(gamer);
                OrderControllerComponent orderController = room.GetComponent<OrderControllerComponent>();
                DeskCardsCacheComponent deskCardsCache = room.GetComponent<DeskCardsCacheComponent>();

                List<Card> handCards = new List<Card>(gamer.GetComponent<HandCardsComponent>().GetAll());
                CardHelper.SortCards(handCards);

                if (gamer.UserID == orderController.Biggest)
                {
                    response.Cards = To.RepeatedField(handCards.Where(card => card.CardWeight == handCards[handCards.Count - 1].CardWeight).ToArray());
                }
                else
                {
                    List<Card[]> result = await CardHelper.GetPrompt(handCards, deskCardsCache, deskCardsCache.Rule);

                    if (result.Count > 0)
                    {
                        response.Cards = To.RepeatedField(result[RandomHelper.RandomNumber(0, result.Count)]);
                    }
                }

                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}