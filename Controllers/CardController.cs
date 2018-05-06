using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CardAPI.Model;

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private CardContext cardContext = new CardContext();
        
        [HttpGet]
        public IEnumerable<Card> Get() {
            return cardContext.Cards.Include(x => x.Options).ToList();
        }
        [HttpPost]
        public void PostNewCard([FromBody] List<Card> newCards) 
        {
            newCards.ForEach(x => 
            {
                cardContext.Add(x);
                cardContext.AddRange(x.Options);
            }
            );
            cardContext.SaveChangesAsync();
        }
        [HttpDelete]
        public void DeleteCards () {
            cardContext.CardOptions.RemoveRange(cardContext.CardOptions.ToList());
            cardContext.Cards.RemoveRange(cardContext.Cards.ToList());
            cardContext.SaveChanges();
        }
    }
}