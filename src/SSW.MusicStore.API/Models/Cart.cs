using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.MusicStore.API.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new Collection<CartItem>();
        }

        [Required]
        public string CartId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public int GetCount()
        {
            return CartItems.Select(c => c.Count).Sum();
        }

        public decimal GetTotal()
        {
            return CartItems.Select(c => c.Count * c.Album.Price).Sum();
        }
    }
}
